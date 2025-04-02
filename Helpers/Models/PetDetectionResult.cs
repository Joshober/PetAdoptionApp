namespace PetProjectBackend.Models
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class PetDetectionResult
    {
        [JsonPropertyName("age_group")]
        public string AgeGroup { get; set; }

        [JsonPropertyName("breeds")]
        public List<Breed> Breeds { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }
        [JsonPropertyName("link")]
        public string? link { get; set; }
        [JsonPropertyName("title")]
        public string? UserID { get; set; }
        [JsonPropertyName("source")]
        public string? Source { get; set; }

        public class Breed
        {
            [JsonPropertyName("label")]
            public string Label { get; set; }

            [JsonPropertyName("percent")]
            public double Percent { get; set; }
        }
    }

}
