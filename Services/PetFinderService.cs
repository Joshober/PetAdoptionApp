using MAUI_Tutorial1_TodoList.Helpers;
using MAUI_Tutorial1_TodoList.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAUI_Tutorial1_TodoList.Services
{
    public class PetfinderService : IPetfinderService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _opts = new() { PropertyNameCaseInsensitive = true };

        public PetfinderService(HttpClient client) => _client = client;

        public async Task<IEnumerable<Animal>> GetAnimalsAsync()
        {
            var url = PetFinderSettings.BuildQueryString();

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            foreach (var header in PetFinderSettings.Headers)
                request.Headers.Add(header.Key, header.Value);

            var resp = await _client.SendAsync(request);
            if (!resp.IsSuccessStatusCode)
                return Enumerable.Empty<Animal>();

            var json = await resp.Content.ReadAsStringAsync();
            Debug.WriteLine("Petfinder JSON: " + json);

            var pfResponse = JsonSerializer.Deserialize<PetfinderResponse>(json, _opts);
            if (pfResponse?.Result?.Animals == null)
                return Enumerable.Empty<Animal>();

            // Map each AnimalDetails into your Animal object.
            var animals = pfResponse.Result.Animals.Select(item => MapAnimalDetailsToAnimal(item.Animal));
            return animals;
        }

        private Animal MapAnimalDetailsToAnimal(AnimalDetails details)
        {
            return new Animal
            {
                // Use the "id" from Petfinder as AnimalId.
                AnimalId = details.id.ToString(),
                // Optionally, set OldId using primary breed's slug (or adjust as needed).
                OldId = details.primary_breed?.slug ?? string.Empty,
                // Map the name and breed.
                OldName = details.name,
                OldBreed = details.primary_breed?.name ?? string.Empty,

                Name = details.name,
                BreedsLabel = details.breeds_label,
                Description = details.description,
                PrimaryPhotoUrl = !string.IsNullOrWhiteSpace(details.primary_photo_url)
                                    ? details.primary_photo_url
                                    : "default_photo.png",
                PrimaryPhotoCroppedUrl = details.primary_photo_cropped_url,

                // Extended mapping for additional fields:
                Age = details.age,           // e.g., "Young"
                Sex = details.sex,           // e.g., "Male"
                Size = details.size,         // e.g., "Medium"
                // Map nested objects:
                Type = details.type?.name ?? string.Empty,
                Species = details.species?.name ?? string.Empty,
                PublishedAt = DateTime.TryParse(details.published_at, out DateTime dt) ? dt : DateTime.MinValue,

                Source = "Petfinder",

                // Map photo_urls to ImageUrls (if available)
                ImageUrls = details.photo_urls ?? new List<string>()
            };
        }
    }
}
