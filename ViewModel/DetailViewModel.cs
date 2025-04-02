using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI_Tutorial1_TodoList.Helpers;
using MAUI_Tutorial1_TodoList.Helpers.Models;
using MAUI_Tutorial1_TodoList.Models;
using MAUI_Tutorial1_TodoList.Services;
using PetProjectBackend.Models;
using Microsoft.Maui.Storage;
using System.Windows.Input;

namespace MAUI_Tutorial1_TodoList.ViewModel
{
    public partial class DetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private Animal pet;

        [ObservableProperty]
        private bool isFavorited;

        [ObservableProperty]
        private string? adoptionFee;

        [ObservableProperty]
        private string? aiCodeText; // Holds the AI code (or result data) to display in an expander.

        [ObservableProperty]
        private string? includes;

        [ObservableProperty]
        private string? age;

        [ObservableProperty]
        private string? weight;

        [ObservableProperty]
        private ObservableCollection<Animal> aistuff = new();

        [ObservableProperty]
        private string? shelterContact;

        // NEW: AiAgeGroup property for binding (e.g., "🐾 ADULT 🐾")
        [ObservableProperty]
        private string? aiAgeGroup;

        // NEW: AiBreeds property for binding the collection of breed predictions
        [ObservableProperty]
        private ObservableCollection<BreedResult> aiBreeds = new();


        public ICommand OpenPhoneCommand => new Command<string>(async (phoneNumber) =>
        {
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                var uri = new Uri($"tel:{phoneNumber}");
                await Launcher.OpenAsync(uri);
            }
        });

        public ICommand OpenMapsCommand => new Command<string>(async (address) =>
        {
            if (!string.IsNullOrWhiteSpace(address))
            {
                // Use the geo URI format to launch the maps app.
                var uri = $"geo:0,0?q={Uri.EscapeDataString(address)}";
                await Launcher.OpenAsync(new Uri(uri));
            }
        });
        // Computed property for the image URL.
        public string DisplayImageUrl =>
            !string.IsNullOrWhiteSpace(Pet.PrimaryPhotoCroppedUrl)
                ? Pet.PrimaryPhotoCroppedUrl
                : Pet.PrimaryPhotoUrl;

        private UserService _userService;

        // Field to hold update info for the pet.
        private UpdateAnimalUrl updateAnimalUrl;

        public DetailViewModel(Animal pet)
        {
            if (pet == null)
                throw new ArgumentNullException(nameof(pet));

            Aistuff = new ObservableCollection<Animal>();

            // Initialize the user service (ensure UserService is set up for MAUI).
            _userService = new UserService();

            Pet = pet;
            LogPetObject();

            var currentUser = CurrentUserSettings.CurrentUser;
            if (currentUser == null)
                throw new InvalidOperationException("Current user is null.");

            if (Pet.ImageUrls == null)
                Pet.ImageUrls = new List<string>();

            updateAnimalUrl = new UpdateAnimalUrl
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
                Email = currentUser.Email,
                UserID = currentUser.UserID
            };
            string decoded = WebUtility.HtmlDecode(updateAnimalUrl.Website);
            Console.WriteLine("Decoded: " + decoded);

            // Use a regex to extract the URL from the href attribute.
            var match = Regex.Match(decoded, @"href\s*=\s*[""'](?<url>[^""']+)[""']");
            if (match.Success)
            {
                updateAnimalUrl.Website = match.Groups["url"].Value;
            }
            else
            {
                Console.WriteLine("No URL found.");
            }

            ExtractInfoFromDescription(Pet.Description);
        }

        private void LogPetObject()
        {
            try
            {
                var petJson = JsonSerializer.Serialize(Pet, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNameCaseInsensitive = true
                });
                Debug.WriteLine("===== PET OBJECT LOG =====");
                Debug.WriteLine(petJson);
                Debug.WriteLine("===== END OF PET OBJECT LOG =====");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error serializing pet object: {ex.Message}");
            }
        }

        private void ExtractInfoFromDescription(string? desc)
        {
            if (string.IsNullOrWhiteSpace(desc))
                return;

            // Adoption Fee: e.g., "Adoption Fee: $135"
            var feeMatch = Regex.Match(desc, @"Adoption\s+Fee:\s*\$(?<fee>\d+(\.\d{1,2})?)", RegexOptions.IgnoreCase);
            if (feeMatch.Success)
            {
                AdoptionFee = "$" + feeMatch.Groups["fee"].Value;
            }

            // Includes: ... Age:
            var includesMatch = Regex.Match(desc, @"Includes:\s*(?<includes>.*?)\s*Age:", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (includesMatch.Success)
            {
                Includes = includesMatch.Groups["includes"].Value.Trim();
                if (Includes.Length >= 4)
                {
                    // Using Substring:
                    Includes = Includes.Substring(0, Includes.Length - 8);

                    // Alternatively, using Remove:
                    // includes = includes.Remove(includes.Length - 4);
                }
            }

            // Age: e.g., "Age: 5 years old"
            var ageMatch = Regex.Match(desc, @"Age:\s*(?<age>\d+(?:[-\s]\w+)*old)\b", RegexOptions.IgnoreCase);
            if (ageMatch.Success)
            {
                Age = ageMatch.Groups["age"].Value.Trim();
            }

            // Weight: e.g., "Weight: 57 lbs"
            var weightMatch = Regex.Match(desc, @"Weight:\s*(?<weight>\d+\s*\w+)", RegexOptions.IgnoreCase);
            if (weightMatch.Success)
            {
                Weight = weightMatch.Groups["weight"].Value.Trim();
            }

            // Shelter Contact: e.g., "Shelter Contact: 816-969-1640"
            var shelterMatch = Regex.Match(desc, @"Shelter\s+Contact:\s*(?<contact>\d{3}-\d{3}-\d{4})", RegexOptions.IgnoreCase);
            if (shelterMatch.Success)
            {
                ShelterContact = shelterMatch.Groups["contact"].Value;
            }
            else
            {
                var phoneMatch = Regex.Match(desc, @"\b\d{3}-\d{3}-\d{4}\b");
                if (phoneMatch.Success)
                {
                    ShelterContact = phoneMatch.Value;
                }
            }
        }

        // Helper method to clean up the description.
        private string CleanDescription(string desc)
        {
            // Decode HTML entities (e.g., convert "\u003C" to "<")
            string decoded = WebUtility.HtmlDecode(desc);
            // Replace <br> tags (in any case) with newline characters.
            string withNewlines = Regex.Replace(decoded, @"<br\s*/?>", "\n", RegexOptions.IgnoreCase);
            // Remove any remaining HTML tags.
            string cleaned = Regex.Replace(withNewlines, "<.*?>", string.Empty);
            return cleaned;
        }

        public FormattedString ParsedDescription => ParseDescription(Pet.Description);

        // Use CleanDescription to process the description before formatting it.
        private FormattedString ParseDescription(string? desc)
        {
            var fs = new FormattedString();
            if (string.IsNullOrWhiteSpace(desc))
                return fs;

            string cleanedDesc = CleanDescription(desc);
            var lines = cleanedDesc.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (var rawLine in lines)
            {
                var line = rawLine.Trim();
                if (string.IsNullOrEmpty(line))
                {
                    fs.Spans.Add(new Span { Text = Environment.NewLine });
                    continue;
                }

                int colonIndex = line.IndexOf(':');
                if (colonIndex > 0)
                {
                    var label = line.Substring(0, colonIndex + 1).Trim();
                    var value = line.Substring(colonIndex + 1).Trim();

                    fs.Spans.Add(new Span
                    {
                        Text = label + " ",
                        FontAttributes = FontAttributes.Bold
                    });
                    fs.Spans.Add(new Span
                    {
                        Text = value + Environment.NewLine
                    });
                }
                else
                {
                    fs.Spans.Add(new Span { Text = line + Environment.NewLine });
                }
            }
            return fs;
        }

        [RelayCommand]
        private async Task ToggleAi()
        {
            try
            {
                Debug.WriteLine("ToggleAi started.");

                // Check if updateAnimalUrl and its PrimaryPhotoUrl are available.
                if (updateAnimalUrl == null || string.IsNullOrWhiteSpace(updateAnimalUrl.PrimaryPhotoUrl))
                {
                    AiCodeText = "Primary photo URL is missing.";
                    Debug.WriteLine("updateAnimalUrl or its PrimaryPhotoUrl is missing.");
                    return;
                }
                Debug.WriteLine($"PrimaryPhotoUrl: {updateAnimalUrl.PrimaryPhotoUrl}");

                // Retrieve the token from SecureStorage.
                var token = await SecureStorage.GetAsync("auth_token");
                if (string.IsNullOrWhiteSpace(token))
                {
                    AiCodeText = "Authentication token is missing.";
                    Debug.WriteLine("Authentication token is missing.");
                    return;
                }
                Debug.WriteLine("Authentication token retrieved.");

                // Create an HTTP client with a handler that ignores certificate errors (for development only).
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };
                using HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                Debug.WriteLine("HTTP client created.");

                // Encode the primary photo URL to safely include it in the query.
                var encodedPhotoUrl = WebUtility.UrlEncode(updateAnimalUrl.PrimaryPhotoUrl);
                var relativeUrl = $"https://10.0.2.2:7291/api/Users/SearchPhoto/{encodedPhotoUrl}/{GlobalFilterSettings.PostalCode}";
                Debug.WriteLine($"Relative URL built: {relativeUrl}");

                // Get the HTTP response.
                HttpResponseMessage response = await httpClient.GetAsync(relativeUrl);
                Debug.WriteLine($"HTTP GET response status: {response.StatusCode}");

                // Read the content as a string.
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("Raw JSON response from GET:");
                Debug.WriteLine(json);

                // Directly deserialize the JSON into an AiPhotoResult.
                var aiResult = JsonSerializer.Deserialize<AiPhotoResult>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (aiResult != null)
                {
                    Debug.WriteLine("Deserialized AI result:");
                    Debug.WriteLine(JsonSerializer.Serialize(aiResult, new JsonSerializerOptions { WriteIndented = true }));

                    // Guard against a null Age_Group.
                    string ageGroup = aiResult.Age_Group ?? "UNKNOWN";
                    AiAgeGroup = $"🐾 {ageGroup.ToUpperInvariant()} 🐾";
                    Debug.WriteLine($"AiAgeGroup set to: {AiAgeGroup}");

                    // Update AiBreeds collection.
                    if (aiResult.Breeds != null)
                    {
                        AiBreeds = new ObservableCollection<BreedResult>(aiResult.Breeds);
                        Debug.WriteLine($"AiBreeds collection updated with {aiResult.Breeds.Count} items.");
                    }
                    else
                    {
                        AiBreeds.Clear();
                        Debug.WriteLine("AiBreeds collection cleared (no breeds found).");
                    }

                    // Build a fun summary string for AiCodeText.
                    var sb = new System.Text.StringBuilder();
                    sb.AppendLine($"🐾 Age Group: {ageGroup.ToUpperInvariant()} 🐾");
                    sb.AppendLine();
                    sb.AppendLine("Breeds Analysis:");

                    if (aiResult.Breeds != null)
                    {
                        foreach (var breed in aiResult.Breeds)
                        {
                            if (breed == null)
                                continue;
                            string label = breed.Label?.Replace("_", " ") ?? "Unknown Breed";
                            int starCount = (int)(breed.Percent / 5);
                            if (starCount > 20)
                                starCount = 20;
                            string progressBar = new string('★', starCount) + new string('☆', 20 - starCount);
                            sb.AppendLine($"{label}: {breed.Percent:F2}%  {progressBar}");
                        }
                    }
                    AiCodeText = sb.ToString();
                    Debug.WriteLine("Final AiCodeText summary built:");
                    Debug.WriteLine(AiCodeText);
                }
                else
                {
                    AiCodeText = "No AI code data returned.";
                    Debug.WriteLine("Deserialized AI result is null.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ToggleAi error: {ex.Message}");
                AiCodeText = $"Error: {ex.Message}";
            }
        }

        [RelayCommand]
        private async Task ToggleFavorite()
        {
            var handler = new HttpClientHandler
            {
                // WARNING: In production, do not ignore certificate errors.
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            var token = await SecureStorage.GetAsync("auth_token");
            HttpClient httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            _userService = new UserService(httpClient);
            var currentUser = CurrentUserSettings.CurrentUser;

            if (!IsFavorited)
            {
                bool updateSuccess = await _userService.AddAnimalUrlAsync(updateAnimalUrl);
                if (!updateSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to update filter options.", "OK");
                }
            }
            else
            {
                bool updateSuccess = await _userService.DeleteAnimalUrlAsync(updateAnimalUrl);
                if (!updateSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Tried to delete!.", "OK");
                }
            }
            IsFavorited = !IsFavorited;
        }
    }
}
