using MAUI_Tutorial1_TodoList.Helpers;
using MAUI_Tutorial1_TodoList.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using MAUI_Tutorial1_TodoList.Helpers.Models;
using MAUI_Tutorial1_TodoList.Extensions;

namespace MAUI_Tutorial1_TodoList.Services
{
    public class PetBackendService : IPetBackendService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public PetBackendService(HttpClient client)
        {
            _client = client;
        }


        public async Task<IEnumerable<Animal>> GetAllAnimalsAsync()
        {
            // Build query parameters using GlobalFilterSettings.
            var queryParams = new Dictionary<string, string>
    {
        { "postalCode", GlobalFilterSettings.PostalCode },
        { "radiusMiles", GlobalFilterSettings.RadiusMiles.ToString() },
        { "startIndex", GlobalFilterSettings.StartIndex.ToString() }
    };

            if (!string.IsNullOrWhiteSpace(GlobalFilterSettings.FilterAnimalType))
            {
                queryParams.Add("filterAnimalType", GlobalFilterSettings.FilterAnimalType);
            }

            // Optionally add other filters from CurrentFilters.
            if (GlobalFilterSettings.CurrentFilters.Gender != null && GlobalFilterSettings.CurrentFilters.Gender.Any())
            {
                // For example, use the first gender in the list.
                queryParams.Add("filterGender", GlobalFilterSettings.CurrentFilters.Gender.First());
            }
            if (GlobalFilterSettings.CurrentFilters.Breed != null && GlobalFilterSettings.CurrentFilters.Breed.Any())
            {
                queryParams.Add("filterBreed", GlobalFilterSettings.CurrentFilters.Breed.First());
            }
            if (GlobalFilterSettings.CurrentFilters.Age != null && GlobalFilterSettings.CurrentFilters.Age.Any())
            {
                queryParams.Add("filterAge", GlobalFilterSettings.CurrentFilters.Age.First());
            }
            if (GlobalFilterSettings.CurrentFilters.Size != null && GlobalFilterSettings.CurrentFilters.Size.Any())
            {
                queryParams.Add("filterSize", GlobalFilterSettings.CurrentFilters.Size.First());
            }

            // Build the full URL (assuming your backend controller route is "api/petservice/all")
            var relativeUrl = QueryHelpers.AddQueryString("api/petservice/all", queryParams);
            var fullUrl = new Uri(_client.BaseAddress, relativeUrl);

            // Print out the final URL and headers
            System.Diagnostics.Debug.WriteLine($"[PetBackendService] Sending GET request to: {fullUrl}");
            System.Diagnostics.Debug.WriteLine("[PetBackendService] Request Headers:");
            foreach (var header in _client.DefaultRequestHeaders)
            {
                System.Diagnostics.Debug.WriteLine($"   {header.Key}: {string.Join(", ", header.Value)}");
            }

            // Send the GET request.
            var response = await _client.GetAsync(relativeUrl);
            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<Animal>();

            var json = await response.Content.ReadAsStringAsync();
            var animals = JsonSerializer.Deserialize<IEnumerable<Animal>>(json, _opts);
            return animals ?? Enumerable.Empty<Animal>();
        }
        public async Task<IEnumerable<IEnumerable<Animal>>> GetAllCloseAnimalsAsync()
        {
            // Build query parameters using GlobalFilterSettings.
            var queryParams = new Dictionary<string, string>
    {
        { "postalCode", GlobalFilterSettings.PostalCode },
        { "radiusMiles", GlobalFilterSettings.RadiusMiles.ToString() },
        { "startIndex", GlobalFilterSettings.StartIndex.ToString() }
    };

            if (!string.IsNullOrWhiteSpace(GlobalFilterSettings.FilterAnimalType))
            {
                queryParams.Add("filterAnimalType", GlobalFilterSettings.FilterAnimalType);
            }

            // Optionally add other filters from CurrentFilters.
            if (GlobalFilterSettings.CurrentFilters.Gender != null && GlobalFilterSettings.CurrentFilters.Gender.Any())
            {
                // For example, use the first gender in the list.
                queryParams.Add("filterGender", GlobalFilterSettings.CurrentFilters.Gender.First());
            }
            if (GlobalFilterSettings.CurrentFilters.Breed != null && GlobalFilterSettings.CurrentFilters.Breed.Any())
            {
                queryParams.Add("filterBreed", GlobalFilterSettings.CurrentFilters.Breed.First());
            }
            if (GlobalFilterSettings.CurrentFilters.Age != null && GlobalFilterSettings.CurrentFilters.Age.Any())
            {
                queryParams.Add("filterAge", GlobalFilterSettings.CurrentFilters.Age.First());
            }
            if (GlobalFilterSettings.CurrentFilters.Size != null && GlobalFilterSettings.CurrentFilters.Size.Any())
            {
                queryParams.Add("filterSize", GlobalFilterSettings.CurrentFilters.Size.First());
            }

            //Build the full URL(assuming your backend controller route is "api/petservice/all")
            var relativeUrl = QueryHelpers.AddQueryString("api/petservice/allclose", queryParams);
            var fullUrl = new Uri(_client.BaseAddress, relativeUrl);

            // Print out the final URL and headers
            System.Diagnostics.Debug.WriteLine($"[PetBackendService] Sending GET request to: {fullUrl}");
            System.Diagnostics.Debug.WriteLine("[PetBackendService] Request Headers:");
            foreach (var header in _client.DefaultRequestHeaders)
            {
                System.Diagnostics.Debug.WriteLine($"   {header.Key}: {string.Join(", ", header.Value)}");
            }

            // Send the GET request.
            var response = await _client.GetAsync(relativeUrl);
            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<IEnumerable<Animal>>();

            var json = await response.Content.ReadAsStringAsync();
            var nestedAnimals = JsonSerializer.Deserialize<IEnumerable<IEnumerable<Animal>>>(json, _opts);
            return nestedAnimals ?? Enumerable.Empty<IEnumerable<Animal>>();
        }
        public async Task<IEnumerable<Animal>> GetFavorites()
        {
            // Build the full URL (assuming your backend controller route is "api/petservice/all")
            var currentuser = CurrentUserSettings.CurrentUser;
            var relativeUrl = ($"api/Users/GetAnimalUrlsByUser/{currentuser.UserID}");
            var fullUrl = new Uri(_client.BaseAddress, relativeUrl);

          

            // Send the GET request.
            var response = await _client.GetAsync(relativeUrl);
            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<Animal>();

            var json = await response.Content.ReadAsStringAsync();
            var animals = JsonSerializer.Deserialize<IEnumerable<Animal>>(json, _opts);
            return animals ?? Enumerable.Empty<Animal>();
        }

        public async Task<IEnumerable<Animal>> GetReccomendation()
        {
           
      
            var currentuser = CurrentUserSettings.CurrentUser;
            // Build the URL.
            var relativeUrl = $"api/Users/GetReccomendations/{GlobalFilterSettings.PostalCode}/{currentuser.UserID}/{GlobalFilterSettings.RadiusMiles}";
            var fullUrl = new Uri(_client.BaseAddress, relativeUrl);
            var response = await _client.GetAsync(relativeUrl);
            Debug.WriteLine(response);
      

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<Animal>();

            var json = await response.Content.ReadAsStringAsync();
            var updateList = JsonSerializer.Deserialize<IEnumerable<UpdateAnimalUrl>>(json, _opts);
            var animalList = updateList?.Select(u => u.ToAnimal()) ?? Enumerable.Empty<Animal>();

            return animalList ?? Enumerable.Empty<Animal>();
        }

        public async Task<IEnumerable<Animal>> GetUnofficalPets()
        {


            var currentuser = CurrentUserSettings.CurrentUser;
            // Build the URL.
            var relativeUrl = $"/api/Users/SearchingPhotos/{GlobalFilterSettings.PostalCode}";
            var fullUrl = new Uri(_client.BaseAddress, relativeUrl);
            var response = await _client.GetAsync(relativeUrl);



            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<Animal>();

            var json = await response.Content.ReadAsStringAsync();
            var animals = JsonSerializer.Deserialize<IEnumerable<Animal>>(json, _opts);
            return animals ?? Enumerable.Empty<Animal>();
        }

        public async Task<Animal> GetDetailAsync(Animal basic)
        {
            // If the animal is not from Petplace, just print and return it.
            //if (basic.Source != "Petplace")
            //{
            //    var options = new JsonSerializerOptions { WriteIndented = true };
            //    Debug.WriteLine("DetailService (non-Petplace): " + JsonSerializer.Serialize(basic, options));
            //    return basic;
            //}

            var jsonPayload = JsonSerializer.Serialize(basic, new JsonSerializerOptions { WriteIndented = true });
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var handler = new HttpClientHandler
            {
                // WARNING: In production, do not ignore certificate errors.
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            //var token = await SecureStorage.GetAsync("auth_token");
            //_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var relativeUrl=($"api/PetService/details/{basic.OldId}/{basic.ClientId}");
            var fullUrl = new Uri(_client.BaseAddress, relativeUrl);
            //var url = $"{BaseUrl}/{basic.OldId}/client/{basic.ClientId}";
            Debug.WriteLine(fullUrl);

            var response = await _client.GetAsync(fullUrl);
            Debug.WriteLine(response);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed ({response.StatusCode})");

            var json = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(json);

            var wrapper = JsonSerializer.Deserialize<Animal>(json, _opts);
            Debug.WriteLine(wrapper);


            return wrapper;
        }
    }
}
