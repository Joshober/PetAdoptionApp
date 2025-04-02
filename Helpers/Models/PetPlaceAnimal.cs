using System;
using System.Text.Json.Serialization;

namespace MAUI_Tutorial1_TodoList.Models
{
    public class PetPlaceAnimal
    {
        [JsonPropertyName("AnimalId")]
        public string AnimalId { get; set; }

        [JsonPropertyName("Pet Name")]
        public string PetName { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Animal Type")]
        public string AnimalType { get; set; }

        [JsonPropertyName("Breed")]
        public string PrimaryBreed { get; set; }

        [JsonPropertyName("Secondary Breed")]
        public string SecondaryBreed { get; set; }
        [JsonPropertyName("Located at")]
        public string LocatedAt { get; set; }

        [JsonPropertyName("Age")]
        public string Age { get; set; }

        [JsonPropertyName("Gender")]
        public string Gender { get; set; }

        [JsonPropertyName("Size Category")]
        public string SizeCategory { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("Pet Location")]
        public string PetLocation { get; set; }

        [JsonPropertyName("Pet Location Address")]
        public string PetLocationAddress { get; set; }

        [JsonPropertyName("Pet Location Phone")]
        public string PetLocationPhone { get; set; }

        [JsonPropertyName("ClientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("Shelter Name")]
        public string ShelterName { get; set; }

        [JsonPropertyName("Shelter Address")]
        public string ShelterAddress { get; set; }

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

        [JsonPropertyName("filterAge")]
        public string FilterAge { get; set; }

        [JsonPropertyName("filterDOB")]
        public DateTime FilterDOB { get; set; }

        [JsonPropertyName("filterGender")]
        public string FilterGender { get; set; }

        [JsonPropertyName("filterSize")]
        public string FilterSize { get; set; }

        [JsonPropertyName("filterDaysOut")]
        public int FilterDaysOut { get; set; }

        [JsonPropertyName("filterAnimalType")]
        public string FilterAnimalType { get; set; }

        [JsonPropertyName("filterPrimaryBreed")]
        public string FilterPrimaryBreed { get; set; }

        [JsonPropertyName("coverImagePath")]
        public string CoverImagePath { get; set; }
    }
}
