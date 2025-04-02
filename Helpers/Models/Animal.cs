using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MAUI_Tutorial1_TodoList.Models
{
    public class Animal
    {
        [JsonPropertyName("id")]
        public string AnimalId { get; set; }  // For Petfinder

        // Fields from PetPlace:
        [JsonPropertyName("AnimalId")]
        public string OldId { get; set; }

        [JsonPropertyName("Pet Name")]
        public string OldName { get; set; }

        [JsonPropertyName("Primary Breed")]
        public string OldBreed { get; set; }

        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [JsonPropertyName(" Secondary Breed")]
        public string SecondaryBreed { get; set; }

        // Common properties:
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("breeds_label")]
        public string BreedsLabel { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("primary_photo_url")]
        public string PrimaryPhotoUrl { get; set; }

        [JsonPropertyName("primary_photo_cropped_url")]
        public string PrimaryPhotoCroppedUrl { get; set; }


        [JsonPropertyName("Located at")]
        public string LocatedAt { get; set; }


        [JsonPropertyName("Shelter Name")]
        public string ShelterName { get; set; }

        [JsonPropertyName("Shelter Address")]
        public string ShelterAddress { get; set; }

        [JsonPropertyName("coverImagePath")]
        public string? CoverImagePath { get; set; }

        [JsonPropertyName("Pet Location")]
        public string PetLocation { get; set; }

        [JsonPropertyName("Pet Location Address")]
        public string PetLocationAddress { get; set; }

        [JsonPropertyName("Pet Location Phone")]
        public string PetLocationPhone { get; set; }

        [JsonPropertyName("City")]
        public string City { get; set; }

        [JsonPropertyName("State")]
        public string State { get; set; }

        [JsonPropertyName("Zip")]
        public string Zip { get; set; }

        [JsonPropertyName("Email")]
        public string Email { get; set; }

        [JsonPropertyName("Phone Number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("Website")]
        public string Website { get; set; }

        // Additional fields from PetPlace:
        public string Url { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Species { get; set; }
        public bool IsMixedBreed { get; set; }
        public string AdoptionStatus { get; set; }
        public DateTime PublishedAt { get; set; }

        // Filter values (as mapped by PetPlace)
        public string FilterAge { get; set; }
        public DateTime? FilterDOB { get; set; }
        public string FilterGender { get; set; }
        public string FilterSize { get; set; }
        public int FilterDaysOut { get; set; }
        public string FilterAnimalType { get; set; }
        public string FilterPrimaryBreed { get; set; }

        // Used for filtering in the app – not from JSON.
        [JsonIgnore]
        public string Source { get; set; }

        public List<string> ImageUrls { get; set; }
    
    }
}
