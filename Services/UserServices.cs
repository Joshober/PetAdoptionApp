using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using MAUI_Tutorial1_TodoList.Models;
using Microsoft.Extensions.Logging;
using MAUI_Tutorial1_TodoList.Helpers;
using System.Diagnostics;
using MAUI_Tutorial1_TodoList.Helpers.Models;
using System.Text.Json;
using System;
using System.Text;
using PetProjectBackend.Models;
using Sprache;

namespace MAUI_Tutorial1_TodoList.Services
{
    public class UserService
    {
        private readonly JsonSerializerOptions _opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private readonly HttpClient _client;

        private readonly HttpClient _httpClient;
        internal readonly object HttpClient;
        private const string BaseUrl = "https://10.0.2.2:7291/api/Users";

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public UserService()
        {
        }

        // Get a list of users.
        public async Task<List<UserDTO>> GetUsersAsync()
        {
            var users = await _httpClient.GetFromJsonAsync<List<UserDTO>>($"{BaseUrl}/GetUsers");
            return users;
        }

        // Login endpoint: returns a token if login is successful.
        public async Task<string> LoginAsync(LoginDTO loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/Login", loginDto);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<LoginResult>();
            return result?.Token;
        }

        // Registration endpoint: returns true if registration is successful.
        public async Task<bool> RegisterAsync(UserDTO userDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/Registration", userDto);
            if (!response.IsSuccessStatusCode)
            {
                // Read the response content for debugging purposes.
                var errorContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"Registration failed: {errorContent}");
            }
            return response.IsSuccessStatusCode;
        }

        // 5) POST /api/Users/RequestPasswordReset
        public async Task<bool> RequestPasswordResetAsync(string email)
        {
            var dto = new PasswordResetRequestDTO { Email = email };
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/RequestPasswordReset", dto);
            return response.IsSuccessStatusCode;
        }

        // 6) POST /api/Users/ResetPassword
        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            var dto = new ResetPasswordDTO { Token = token, NewPassword = newPassword };
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/ResetPassword", dto);
            return response.IsSuccessStatusCode;
        }

        // 7) PUT /api/Users/UpdateAnimalUrls
        public async Task<bool> UpdateAnimalUrlsAsync(UpdateAnimalUrl urlob)
        {
            var token = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/UpdateAnimalUrl/{urlob.Url}", urlob);
            return response.IsSuccessStatusCode;
        }
        public async Task<HttpResponseMessage> GetifAnimalUrlsAsync(UpdateAnimalUrl urlob)
        {
            var token = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{BaseUrl}/GetAnimalUrlByUser/{urlob.UserID}/{urlob.Name}");
            Debug.WriteLine($"{BaseUrl}/GetAnimalUrlByUser/{urlob.UserID}/{urlob.Name}");
            return response;

        }

        // 8) POST /api/Users/AddAnimalUrl
        public async Task<bool> AddAnimalUrlAsync(UpdateAnimalUrl urlob)
        {
            // Get token from secure storage.
            var token = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Serialize the payload using our JSON options.
            var jsonPayload = JsonSerializer.Serialize(urlob, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            var currentuser = CurrentUserSettings.CurrentUser;
            urlob.UserID = currentuser.UserID;
            // Build the URL.
            var url = $"{BaseUrl}/AddAnimalUrl/";

            // Create the content with proper encoding and header.
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Post the JSON payload.
            var response = await _httpClient.PostAsync(url, content);
            Debug.WriteLine($"Response: {content}");

            return response.IsSuccessStatusCode;
        }
     
        // 9) DELETE /api/Users/DeleteAnimalUrl?url=someUrl
        public async Task<bool> DeleteAnimalUrlAsync(UpdateAnimalUrl urlob)
        {
            var token = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var url = $"{BaseUrl}/DeleteAnimalUrl/{urlob.Url}";

            var jsonPayload = JsonSerializer.Serialize(urlob, new JsonSerializerOptions { WriteIndented = true });
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");


            var response = await _httpClient.DeleteAsync($"{BaseUrl}/DeleteAnimalUrl/{urlob.Name}/{urlob.UserID}");
            Debug.WriteLine($"Response status: {response.StatusCode}");

            return response.IsSuccessStatusCode;
        }
        public async Task<AiPhotoResult> GetAiPhoto(string url)
        {
            // Build the URL.
            var relativeUrl = $"https://10.0.2.2:7291/api/Users/SearchPhoto/{url}/{GlobalFilterSettings.PostalCode}";

            // Get the HTTP response.
            HttpResponseMessage response = await _client.GetAsync(relativeUrl);

            // Check if the response is successful.
            if (!response.IsSuccessStatusCode)
                return null; // Or handle error as needed.

            // Read the content as a string.
            var json = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(json);
            // Deserialize the JSON into the AiPhotoResult model.
            var result = JsonSerializer.Deserialize<AiPhotoResult>(json, _opts);
            Debug.WriteLine(result);

            return result;
        }



        //// 11) DELETE /api/Users/RemoveUrlFromAllUsers?url=someUrl
        //public async Task<bool> RemoveUrlFromAllUsersAsync(string url)
        //{
        //    var requestUrl = $"{BaseUrl}/RemoveUrlFromAllUsers?url={Uri.EscapeDataString(url)}";
        //    var response = await _httpClient.DeleteAsync(requestUrl);
        //    return response.IsSuccessStatusCode;
        //}
        public async Task<List<UpdateAnimalUrl>> GetMyAnimalUrlsAsync(int id)
        {
            try
            {
                var token = await SecureStorage.GetAsync("auth_token");
                if (string.IsNullOrEmpty(token))
                    throw new InvalidOperationException("Auth token missing. User not logged in.");

                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Do NOT prepend BaseUrl again if BaseAddress is already set!
                var response = await _httpClient.GetAsync($"GetAnimalUrlsByUser/{id}");

                // CRITICAL: Ensure successful status or throw an exception immediately!
                response.EnsureSuccessStatusCode();

                var urls = await response.Content.ReadFromJsonAsync<List<UpdateAnimalUrl>>();

                Debug.WriteLine($"Response: {response.StatusCode}");
                Debug.WriteLine($"Data: {urls?.Count ?? 0} animals");

                return urls ?? new List<UpdateAnimalUrl>();
            }
            catch (HttpRequestException httpEx)
            {
                Debug.WriteLine($"HTTP Error: {httpEx.StatusCode} - {httpEx.Message}");
                throw; // Rethrow this to let your ViewModel handle and show clearly.
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected Error: {ex.Message}");
                throw; // Rethrow as well.
            }
        }

        // Call this method when the filter options change to update the user’s filters in your backend.
        public async Task<bool> UpdateUserAsync(UserDTO userDto)
        {
            var payload = new
            {
                firstName = userDto.FirstName,
                lastName = userDto.LastName,
                email = userDto.Email,
                password = userDto.Password,
                profilePhoto = userDto.ProfilePhoto,
                filterAnimalType = userDto.FilterAnimalType,
                filterBreed = userDto.FilterBreed,
                // Convert the gender string into a list if needed.
                filterGender = string.IsNullOrWhiteSpace(userDto.FilterGenderString) ? new List<string>() : new List<string> { userDto.FilterGenderString },
                filterAge = userDto.FilterAge,
                filterSize = userDto.FilterSize,
                filterHousehold = userDto.FilterHousehold,
                filterCoatLength = userDto.FilterCoatLength,
                filterColor = userDto.FilterColor,
                filterDaysOnPetfinder = userDto.FilterDaysOnPetfinder,
                filterShelter = userDto.FilterShelter,
                filterAttribute = userDto.FilterAttribute
            };
            var payloadJson = System.Text.Json.JsonSerializer.Serialize(payload);
            var token = await SecureStorage.GetAsync("auth_token");
            _httpClient.DefaultRequestHeaders.Authorization =new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            // This endpoint should be implemented on your backend to update the user's info including filters.
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/Update", userDto);
            Debug.WriteLine(response);

            return response.IsSuccessStatusCode;
        }
        public async Task<string> GetUsersRawAsync()
        {
            return await _httpClient.GetStringAsync($"{BaseUrl}/GetUsers");
        }

       
    }

    // DTOs used by the service





    public class PasswordResetRequestDTO
    {
        public string Email { get; set; }
    }

    public class ResetPasswordDTO
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

   
    public class UserUrlDTO
    {
        public int UserUrlID { get; set; }
        public string Url { get; set; }
        public int UserID { get; set; }

    }

    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResult
    {
        public string Token { get; set; }
    }




}
