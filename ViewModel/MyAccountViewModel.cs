using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI_Tutorial1_TodoList.Models;
using MAUI_Tutorial1_TodoList.Services;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System;
using System.Linq;
using MAUI_Tutorial1_TodoList.Helpers;
using System.Text.Json;
using System.Diagnostics;

namespace MAUI_Tutorial1_TodoList.ViewModel
{
    public partial class MyAccountViewModel : ObservableObject
    {
        private readonly UserService _userService;

        public MyAccountViewModel()
        {
            var handler = new HttpClientHandler
            {
                // WARNING: In production, do not ignore certificate errors.
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            HttpClient httpClient = new HttpClient(handler);
            _userService = new UserService(httpClient);

            // Optionally, initialize login/register visibility.
            IsLoginVisible = true;
        }

        // Registration Properties
        [ObservableProperty] private string registerFirstName;
        [ObservableProperty] private string registerLastName;
        [ObservableProperty] private string registerEmail;
        [ObservableProperty] private string registerPassword;

        // Login Properties
        [ObservableProperty] private string loginEmail;
        [ObservableProperty] private string loginPassword;

        // User Info Properties
        [ObservableProperty] private string username;
        [ObservableProperty] private string email;
        [ObservableProperty] private UserDTO currentUser;
        [ObservableProperty] private ObservableCollection<UserDTO> users;

        // Profile Image Property
        [ObservableProperty]
        private ImageSource profileImage = ImageSource.FromFile("default_profile.png");

        // Controls whether the login form is visible (if false, show register form)
        [ObservableProperty]
        private bool isLoginVisible;

        // Computed property: user is logged in if CurrentUser is not null.
        public bool IsLoggedIn => CurrentUser != null;

        partial void OnCurrentUserChanged(UserDTO value)
        {
            OnPropertyChanged(nameof(IsLoggedIn));
        }

        // Command to toggle between login and register forms.
        [RelayCommand]
        private void ToggleLoginRegister()
        {
            IsLoginVisible = !IsLoginVisible;
        }

        [RelayCommand]
        private async Task Register()
        {
            try
            {
                // Create the new user payload including the current filter settings.
                var newUser = new UserDTO
                {
                    FirstName = RegisterFirstName,
                    LastName = RegisterLastName,
                    Email = RegisterEmail,
                    Password = RegisterPassword,
                    FilterAnimalType = GlobalFilterSettings.CurrentFilters.AnimalType, // string
                    FilterBreed = GlobalFilterSettings.CurrentFilters.Breed,           // List<string>
                                                                                       // Convert the Gender list to a string (the backend expects a string)
                    FilterGender = GlobalFilterSettings.CurrentFilters.Gender,
                    FilterAge = GlobalFilterSettings.CurrentFilters.Age,               // List<string>
                    FilterSize = GlobalFilterSettings.CurrentFilters.Size,             // List<string>
                    FilterHousehold = GlobalFilterSettings.CurrentFilters.Household,   // List<string>
                    FilterCoatLength = GlobalFilterSettings.CurrentFilters.CoatLength, // List<string>
                    FilterColor = GlobalFilterSettings.CurrentFilters.Color,           // List<string>
                    FilterDaysOnPetfinder = GlobalFilterSettings.CurrentFilters.DaysOnPetfinder, // List<string>
                    FilterShelter = GlobalFilterSettings.CurrentFilters.Shelter,       // List<string>
                    FilterAttribute = GlobalFilterSettings.CurrentFilters.Attribute      // List<string>
                };

                var success = await _userService.RegisterAsync(newUser);
                if (success)
                {
                    // Clear the registration fields.
                    RegisterFirstName = string.Empty;
                    RegisterLastName = string.Empty;
                    RegisterEmail = string.Empty;
                    RegisterPassword = string.Empty;
                    await App.Current.MainPage.DisplayAlert("Success", "Registration successful!", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Registration failed.", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }


        [RelayCommand]
        private async Task Login()
        {
            try
            {
                var token = await _userService.LoginAsync(new Services.LoginDTO
                {
                    Email = LoginEmail,
                    Password = LoginPassword
                });

                if (!string.IsNullOrEmpty(token))
                {
                    await SecureStorage.SetAsync("auth_token", token);
                    LoginEmail = string.Empty;
                    LoginPassword = string.Empty;
                    await LoadUserDataAsync(token);
                    await App.Current.MainPage.DisplayAlert("Success", "Login successful!", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Invalid credentials.", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task Logout()
        {
            SecureStorage.Remove("auth_token");
            CurrentUser = null;
            Username = string.Empty;
            Email = string.Empty;
            ProfileImage = ImageSource.FromFile("default_profile.png");
            await App.Current.MainPage.DisplayAlert("Logged out", "You have been logged out", "OK");
        }

        public async Task LoadUserDataAsync(string token = null)
        {
            try
            {
                // Get the raw JSON response from the backend.
                var rawJson = await _userService.GetUsersRawAsync();

                // Set options to allow case-insensitive matching.
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                Debug.WriteLine(rawJson);
                // Deserialize the JSON into a list of UserDTO objects.
                var users = JsonSerializer.Deserialize<List<UserDTO>>(rawJson, options);
                if (users != null && users.Count > 0)
                {
                    // If only one user is returned (the logged-in user), pick that user.
                    var user = users.First();
                    Debug.WriteLine(user.UserID);

                    // If needed, adjust FilterGender conversion if the JSON returns a string:
                    // (Uncomment the block below if your JSON for FilterGender comes as a string instead of a list.)
                    /*
                    using var doc = JsonDocument.Parse(rawJson);
                    var root = doc.RootElement;
                    // Assuming the first user corresponds to index 0 in the array:
                    if (root[0].TryGetProperty("filterGender", out JsonElement genderElement) &&
                        genderElement.ValueKind == JsonValueKind.String)
                    {
                        var genderString = genderElement.GetString();
                        if (user.FilterGender == null || user.FilterGender.Count == 0)
                        {
                            user.FilterGender = new List<string>();
                            if (!string.IsNullOrEmpty(genderString))
                            {
                                user.FilterGender.Add(genderString);
                            }
                        }
                    }
                    */

                    // Set the view model properties.
                    CurrentUser = user;
                    Username = user.FirstName;
                    Email = user.Email;
                   
                    Users = new ObservableCollection<UserDTO>(users);
                    CurrentUserSettings.CurrentUser = user; // 'user' is the logged in user returned from the backend

                    // Update profile image if available.
                    if (!string.IsNullOrEmpty(user.ProfilePhoto) &&
                        Uri.TryCreate(user.ProfilePhoto, UriKind.Absolute, out var validUri))
                    {
                        ProfileImage = ImageSource.FromUri(validUri);
                    }
                    else
                    {
                        ProfileImage = ImageSource.FromFile("default_profile.png");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No user was returned from the backend.");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }




        public async Task CheckIfLoggedInAsync()
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (!string.IsNullOrEmpty(token))
            {
                await LoadUserDataAsync(token);
            }
        }
    }
}
