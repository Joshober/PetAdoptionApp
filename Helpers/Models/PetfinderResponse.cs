using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MAUI_Tutorial1_TodoList.Models
{
    public class PetfinderResponse
    {
        [JsonPropertyName("result")]
        public PetfinderResult Result { get; set; }
    }

    public class PetfinderResult
    {
        [JsonPropertyName("animals")]
        public List<PetfinderAnimalWrapper> Animals { get; set; }
    }

    public class PetfinderAnimalWrapper
    {
        [JsonPropertyName("animal")]
        public AnimalDetails Animal { get; set; }
    }

    public class AnimalDetails
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("type")]
        public TypeInfo type { get; set; }

        [JsonPropertyName("species")]
        public SpeciesInfo species { get; set; }

        [JsonPropertyName("breeds_label")]
        public string breeds_label { get; set; }

        public BreedInfo BreedsLabel { get; set; }

        [JsonPropertyName("primary_breed")]
        public BreedInfo primary_breed { get; set; }
        [JsonPropertyName("age")]
        public string age { get; set; }

        [JsonPropertyName("sex")]
        public string sex { get; set; }

        [JsonPropertyName("size")]
        public string size { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("description")]
        public string description { get; set; }

        [JsonPropertyName("primary_photo_url")]
        public string primary_photo_url { get; set; }

        [JsonPropertyName("primary_photo_cropped_url")]
        public string primary_photo_cropped_url { get; set; }

        [JsonPropertyName("published_at")]
        public string published_at { get; set; }

        [JsonPropertyName("photo_urls")]
        public List<string> photo_urls { get; set; }
    }

    public class TypeInfo
    {
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("name")]
        public string name { get; set; }
    }

    public class SpeciesInfo
    {
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("name")]
        public string name { get; set; }
    }

    public class BreedInfo
    {
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("name")]
        public string name { get; set; }
        [JsonPropertyName("slug")]
        public string slug { get; set; }
    }
}
