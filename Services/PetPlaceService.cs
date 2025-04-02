using MAUI_Tutorial1_TodoList.Helpers;
using MAUI_Tutorial1_TodoList.Helpers.Models;
using MAUI_Tutorial1_TodoList.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAUI_Tutorial1_TodoList.Services
{
    public class PetPlaceService : IPetplaceService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private List<string> MapAge(List<string> ages)
        {
            var mapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Baby", "PK" },
                { "Young", "Y" },
                { "Adult", "A" },
                { "Senior", "S" }
            };

            return ages.Select(age => mapping.TryGetValue(age, out var code) ? code : age).ToList();
        }

        private List<string> MapSize(List<string> sizes)
        {
            var mapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Small", "S" },
                { "Medium", "M" },
                { "Large", "L" },
                { "Extra Large", "XL" }
            };

            return sizes.Select(size => mapping.TryGetValue(size, out var code) ? code : size).ToList();
        }

        // Map gender values: Male -> M, Female -> F.
        private List<string> MapGender(List<string> genders)
        {
            var mapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Male", "M" },
                { "Female", "F" }
            };

            return genders.Select(gender => mapping.TryGetValue(gender, out var code) ? code : gender).ToList();
        }
        public PetPlaceService(HttpClient client) => _client = client;
 
        public async Task<IEnumerable<Animal>> GetAnimalsAsync()
        {
            // Build the locationInformation section.
            var locationInformation = new
            {
                zipPostal = GlobalFilterSettings.PostalCode,
                milesRadius = GlobalFilterSettings.RadiusMiles.ToString()
            };

            // Build the animalFilters section dynamically.
            var animalFilters = new Dictionary<string, object>
            {
                ["startIndex"] = GlobalFilterSettings.StartIndex.ToString()
            };

            if (!string.IsNullOrWhiteSpace(GlobalFilterSettings.FilterAnimalType))
                animalFilters["filterAnimalType"] = GlobalFilterSettings.FilterAnimalType;

            if (GlobalFilterSettings.CurrentFilters.Breed != null && GlobalFilterSettings.CurrentFilters.Breed.Any())
                animalFilters["filterBreed"] = GlobalFilterSettings.CurrentFilters.Breed;

            if (GlobalFilterSettings.CurrentFilters.Gender != null && GlobalFilterSettings.CurrentFilters.Gender.Any())
{
                var mappedGenders = MapGender(GlobalFilterSettings.CurrentFilters.Gender);
                if (mappedGenders.Any())
                {
                    animalFilters["filterGender"] = mappedGenders[0];
                }
            }
            if (GlobalFilterSettings.CurrentFilters.Age != null && GlobalFilterSettings.CurrentFilters.Age.Any())
                animalFilters["filterAge"] = MapAge(GlobalFilterSettings.CurrentFilters.Age);

            if (GlobalFilterSettings.CurrentFilters.Size != null && GlobalFilterSettings.CurrentFilters.Size.Any())
                animalFilters["filterSize"] = MapSize(GlobalFilterSettings.CurrentFilters.Size);

            var payload = new Dictionary<string, object>
            {
                ["locationInformation"] = locationInformation,
                ["animalFilters"] = animalFilters
            };

            // Debug: Print the final payload.
            string jsonPayload = JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true });
            Debug.WriteLine("PetPlace Payload: " + jsonPayload);

            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(PetPlaceSettings.Url, content);
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"PetPlace request failed: {response.StatusCode}");
                return Enumerable.Empty<Animal>();
            }

            var body = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("PetPlace Response Body: " + body);

            if (string.IsNullOrWhiteSpace(body))
                return Enumerable.Empty<Animal>();

            var wrapper = JsonSerializer.Deserialize<PetPlaceResponse>(body, _opts);
            if (wrapper == null)
            {
                Debug.WriteLine("Deserialization of PetPlaceResponse returned null.");
                return Enumerable.Empty<Animal>();
            }

            // Use either the 'ppRequired' or 'animal' property.
            List<PetPlaceAnimal> petPlaceAnimals = null;
            if (wrapper.ppRequired != null && wrapper.ppRequired.Any())
            {
                petPlaceAnimals = wrapper.ppRequired;
            }
            else if (wrapper.animal != null && wrapper.animal.Any())
            {
                petPlaceAnimals = wrapper.animal;
            }
            else
            {
                petPlaceAnimals = new List<PetPlaceAnimal>();
            }

            Debug.WriteLine($"PetPlace returned {petPlaceAnimals.Count} animals.");

            var animals = petPlaceAnimals.Select(pp =>
            {
                var animal = MapPetPlaceAnimalToAnimal(pp);

                // Map primary photo:
                if (wrapper.ImageURL != null && wrapper.ImageURL.Any())
                {
                    animal.ImageUrls = wrapper.ImageURL;
                    animal.PrimaryPhotoUrl = animal.ImageUrls.First();
                }
                else if (!string.IsNullOrWhiteSpace(pp.CoverImagePath))
                {
                    animal.PrimaryPhotoUrl = pp.CoverImagePath;
                    animal.ImageUrls = new List<string> { pp.CoverImagePath };
                }
                else
                {
                    animal.ImageUrls = new List<string>();
                }

                //Debug.WriteLine("Mapped Animal: " + JsonSerializer.Serialize(pp, new JsonSerializerOptions { WriteIndented = true }));
                return animal;
            });

            return animals;
        }


           

        public Animal MapPetPlaceAnimalToAnimal(PetPlaceAnimal pp)
        {
            return new Animal
            {
                OldId = pp.AnimalId,
                OldName = pp.PetName,
                Name = pp.Name,
                OldBreed = pp.PrimaryBreed,
                BreedsLabel = pp.PrimaryBreed, // Using PrimaryBreed as the display label
                SecondaryBreed = pp.SecondaryBreed ?? string.Empty,
                ClientId = pp.ClientId,

                // Map basic attributes.
                Age = pp.Age,
                Sex = pp.Gender,
                Size = pp.SizeCategory,
                Type = pp.AnimalType,
                Description = pp.Description,

                // Map organization and location.
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

                // Initialize ImageUrls.
                ImageUrls = new List<string>()
            };
        }
    }
}
