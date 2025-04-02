using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI_Tutorial1_TodoList.Models;
using MAUI_Tutorial1_TodoList.Services;
using MAUI_Tutorial1_TodoList.Helpers;
using MAUI_Tutorial1_TodoList.Helpers.Models;
using MAUI_Tutorial1_TodoList.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Maui.Views;
using System.Text.RegularExpressions;

namespace MAUI_Tutorial1_TodoList.ViewModel
    {
        public partial class FavoritesViewModel : ObservableObject
        {
            private readonly IPetfinderService _petfinder;
            private readonly IPetplaceService _petplace;
            private readonly IAnimalDetailService _detailService;
            private readonly IPetBackendService _petBackendService;
            private UserService _userService;

            [ObservableProperty]
            private ObservableCollection<Animal> pets = new();

            [ObservableProperty]
            private bool isRefreshing;

            [ObservableProperty]
            private PetSource selectedSource = PetSource.All;

            public IAsyncRelayCommand LoadCommand { get; }
            public event EventHandler? OpenChatRequested;

            public FavoritesViewModel(
            IPetBackendService petBackendService,// Inject the backend service here
                IAnimalDetailService detailService)

            {
                _petBackendService = petBackendService;

                _detailService = detailService;
                LoadCommand = new AsyncRelayCommand(LoadAllPetsAsync);
            }

            [RelayCommand]
            void OpenChat() => OpenChatRequested?.Invoke(this, EventArgs.Empty);

            [RelayCommand]
            async Task OpenFilterPopup()
            {
                var popup = new FilterPopup();
                popup.FiltersApplied += async (_, e) =>
                {
                    // The FilterPopup already updates GlobalFilterSettings.CurrentFilters.
                    await LoadCommand.ExecuteAsync(null);
                };

                await Shell.Current.ShowPopupAsync(popup);
            }

            public enum PetSource { All, Petfinder, Petplace }

            private async Task LoadAllPetsAsync()
            {

                IsRefreshing = true;
                var backendAnimals = await _petBackendService.GetFavorites();


                Pets = new ObservableCollection<Animal>(backendAnimals);

                IsRefreshing = false;
            }

        [RelayCommand]
        private async Task SelectPet(Animal pet)
        {
            if (pet is null) return;

            Animal selected = pet;
         
            try
            {
                string valueInside = "";
                string pattern = @"\((.*?)\)"; // Matches text between parentheses
                var match = Regex.Match(pet.Name, pattern);
                if (match.Success)
                {
                    valueInside = match.Groups[1].Value;
                    Console.WriteLine("Value inside parentheses: " + valueInside);
                    pet.OldId = valueInside;
                }
                else
                {
                    Console.WriteLine("No match found.");
                }
                pet = await _petBackendService.GetDetailAsync(pet);
                var handler = new HttpClientHandler
                {
                    // WARNING: In production, do not ignore certificate errors.
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };
                var token = await SecureStorage.GetAsync("auth_token");
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                _userService = new UserService(httpClient);
                var currentUser = CurrentUserSettings.CurrentUser;
                UpdateAnimalUrl a = new UpdateAnimalUrl
                {
                    OldAnimalId = pet.AnimalId != null ? pet.AnimalId.ToString() : string.Empty,
                    OldName = pet.Name,
                    OldBreed = pet.OldBreed,
                    ClientId = pet.ClientId,
                    SecondaryBreed = pet.SecondaryBreed,
                    Name = pet.Name,
                    BreedsLabel = pet.BreedsLabel,
                    Description = pet.Description,
                    PrimaryPhotoUrl = pet.PrimaryPhotoUrl,
                    PrimaryPhotoCroppedUrl = pet.PrimaryPhotoCroppedUrl,
                    ShelterName = pet.ShelterName,
                    ShelterAddress = pet.ShelterAddress,
                    CoverImagePath = pet.CoverImagePath,
                    PetLocation = pet.PetLocation,
                    PetLocationAddress = pet.PetLocationAddress,
                    PetLocationPhone = pet.PetLocationPhone,
                    City = pet.City,
                    State = pet.State,
                    Zip = pet.Zip,
                    PhoneNumber = pet.PhoneNumber,
                    Website = pet.Website,
                    Url = pet.Url,
                    Age = pet.Age,
                    Sex = pet.Sex,
                    Size = pet.Size,
                    Type = pet.Type,
                    Species = pet.Species,
                    IsMixedBreed = pet.IsMixedBreed,
                    AdoptionStatus = pet.AdoptionStatus,
                    PublishedAt = pet.PublishedAt,
                    FilterAge = pet.FilterAge,
                    FilterDOB = pet.FilterDOB,
                    FilterGender = pet.FilterGender,
                    FilterSize = pet.FilterSize,
                    FilterDaysOut = pet.FilterDaysOut,
                    FilterAnimalType = pet.FilterAnimalType,
                    FilterPrimaryBreed = pet.FilterPrimaryBreed,
                    Source = pet.Source,
                    ImageUrls = pet.ImageUrls,
                    Email = currentUser.Email,  // Use the current user's email.
                    UserID = currentUser.UserID     // Assuming currentUser has an "id" property.
                };
                // If not already favorited, add the animal URL using the service.
                var response = await _userService.GetifAnimalUrlsAsync(a);
                bool updateSuccess = response.IsSuccessStatusCode;

                Debug.WriteLine(a.UserID);
                Debug.WriteLine(a.Name);

                Debug.WriteLine(response);
                if (updateSuccess)
                {
                    Debug.WriteLine(updateSuccess);
                    await Shell.Current.GoToAsync(nameof(DetailPage),
                               new System.Collections.Generic.Dictionary<string, object> { ["Animal"] = pet, ["Favorite"] = true });
                }
                else
                {
                    await Shell.Current.GoToAsync(nameof(DetailPage),
                  new System.Collections.Generic.Dictionary<string, object> { ["Animal"] = pet, ["Favorite"] = false });
                }

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                return;
            }


        }
    }
}

