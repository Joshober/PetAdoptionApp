using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MAUI_Tutorial1_TodoList.Helpers;
using MAUI_Tutorial1_TodoList.Models;

namespace MAUI_Tutorial1_TodoList.Services
{
    public interface IAnimalDetailService
    {
        Task<Animal> GetDetailAsync(Animal basic);
    }

    public class AnimalDetailService : IAnimalDetailService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true };
        private readonly HttpClient _httpClient;
        internal readonly object HttpClient;
        private const string BaseUrl = "https://10.0.2.2:7291/api/PetService/details";

  
        public AnimalDetailService(HttpClient client) => _client = client;

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
            HttpClient _client = new HttpClient(handler);
            //var token = await SecureStorage.GetAsync("auth_token");
            //_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var url = $"{BaseUrl}/{basic.OldId}/client/{basic.ClientId}";
            Debug.WriteLine(url);

            var response = await _client.PostAsync(url, content);
            Debug.WriteLine(response);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed ({response.StatusCode})");

            var json = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(json);

            var wrapper = JsonSerializer.Deserialize<Animal>(json, _opts);
            Debug.WriteLine(wrapper);


            return wrapper;
        }

        private Animal MapPetPlaceAnimalToAnimal(PetPlaceAnimal pp)
        {
            return new Animal
            {
                OldId = pp.AnimalId,
                OldName = pp.PetName,
                Name = pp.PetName,
                OldBreed = pp.PrimaryBreed,
                SecondaryBreed = pp.SecondaryBreed ?? string.Empty,
                ClientId = pp.ClientId,

                // Map additional fields.
                Age = pp.Age,
                Sex = pp.Gender,
                Size = pp.SizeCategory,
                Type = pp.AnimalType,
                Description = pp.Description,

                // Organization and location mapping.
                ShelterName = pp.ShelterName,
                ShelterAddress = pp.ShelterAddress,
                City = pp.City,
                State = pp.State,
                Zip = pp.Zip,
                Email = pp.Email ?? string.Empty,
                PhoneNumber = pp.PhoneNumber,
                Website = pp.Website,

                // Map filter fields.
                FilterAge = pp.FilterAge,
                FilterDOB = pp.FilterDOB,
                FilterGender = pp.FilterGender,
                FilterSize = pp.FilterSize,
                FilterDaysOut = pp.FilterDaysOut,
                FilterAnimalType = pp.FilterAnimalType,
                FilterPrimaryBreed = pp.FilterPrimaryBreed,
                // Mark the source.
                Source = "Petplace",

                // Initialize the image list; it will be updated if wrapper.ImageURL is present.
                ImageUrls = new System.Collections.Generic.List<string>()
            };
        }
    }
}
