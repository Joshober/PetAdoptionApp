using System;
using System.Text.Json.Serialization;

namespace MAUI_Tutorial1_TodoList.Models
{
    public class AnimalResponse
    {
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("animal")]
        public Animal[] Animals { get; set; } = Array.Empty<Animal>();
    }
}
