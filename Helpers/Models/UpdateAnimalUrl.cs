using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MAUI_Tutorial1_TodoList.Helpers.Models
{
    public class UpdateAnimalUrl
    {
        // EF Core key – not exposed in JSON.
        [JsonIgnore]
        public int Id { get; set; }

        // Internal property – not exposed.
        [JsonIgnore]
        public int AnimalId { get; set; }

        // Mapped from incoming JSON "AnimalId"
        [JsonPropertyName("AnimalId")]
        public string? OldAnimalId { get; set; }

        [JsonPropertyName("Pet Name")]
        public string? OldName { get; set; }

        [JsonPropertyName("Primary Breed")]
        public string? OldBreed { get; set; }

        [JsonPropertyName("clientId")]
        public string? ClientId { get; set; }

        // Note the leading space in the JSON property name
        [JsonPropertyName(" Secondary Breed")]
        public string? SecondaryBreed { get; set; }
        [JsonPropertyName("Located at")]
        public string LocatedAt { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("breeds_label")]
        public string? BreedsLabel { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("primary_photo_url")]
        public string? PrimaryPhotoUrl { get; set; }

        [JsonPropertyName("primary_photo_cropped_url")]
        public string? PrimaryPhotoCroppedUrl { get; set; }

        [JsonPropertyName("Shelter Name")]
        public string? ShelterName { get; set; }

        [JsonPropertyName("Shelter Address")]
        public string? ShelterAddress { get; set; }

        [JsonPropertyName("coverImagePath")]
        public string? CoverImagePath { get; set; }

        [JsonPropertyName("Pet Location")]
        public string? PetLocation { get; set; }

        [JsonPropertyName("Pet Location Address")]
        public string? PetLocationAddress { get; set; }

        [JsonPropertyName("Pet Location Phone")]
        public string? PetLocationPhone { get; set; }

        [JsonPropertyName("City")]
        public string? City { get; set; }

        [JsonPropertyName("State")]
        public string? State { get; set; }

        [JsonPropertyName("Zip")]
        public string? Zip { get; set; }

        [JsonPropertyName("Email")]
        public string? Email { get; set; }

        [JsonPropertyName("Phone Number")]
        public string? PhoneNumber { get; set; }

        [JsonPropertyName("Website")]
        public string? Website { get; set; }

        // Additional fields from PetPlace:
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("age")]
        public string? Age { get; set; }

        [JsonPropertyName("sex")]
        public string? Sex { get; set; }

        [JsonPropertyName("size")]
        public string? Size { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("species")]
        public string? Species { get; set; }

        [JsonPropertyName("isMixedBreed")]
        public bool IsMixedBreed { get; set; }

        [JsonPropertyName("adoptionStatus")]
        public string? AdoptionStatus { get; set; }

        [JsonPropertyName("publishedAt")]
        public DateTime PublishedAt { get; set; }

        // Filter values:
        [JsonPropertyName("filterAge")]
        public string? FilterAge { get; set; }

        [JsonPropertyName("filterDOB")]
        public DateTime? FilterDOB { get; set; }

        [JsonPropertyName("filterGender")]
        public string? FilterGender { get; set; }

        [JsonPropertyName("filterSize")]
        public string? FilterSize { get; set; }

        [JsonPropertyName("filterDaysOut")]
        public int FilterDaysOut { get; set; }

        [JsonPropertyName("filterAnimalType")]
        public string? FilterAnimalType { get; set; }

        [JsonPropertyName("filterPrimaryBreed")]
        public string? FilterPrimaryBreed { get; set; }

        // Used for filtering in the app – not part of JSON.
        [JsonIgnore]
        public string? Source { get; set; }

        [JsonPropertyName("imageUrls")]
        public List<string>? ImageUrls { get; set; }

        [JsonPropertyName("userID")]
        public int UserID { get; set; }
    }
}
