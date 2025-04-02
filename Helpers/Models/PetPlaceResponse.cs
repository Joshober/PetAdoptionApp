using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MAUI_Tutorial1_TodoList.Models
{
    public class PetPlaceResponse
    {
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        // Sometimes the response key is "ppRequired"
        [JsonPropertyName("ppRequired")]
        public List<PetPlaceAnimal> ppRequired { get; set; }

        // Sometimes the response key is "animal"
        [JsonPropertyName("animal")]
        public List<PetPlaceAnimal> animal { get; set; }

        [JsonPropertyName("imageURL")]
        public List<string> ImageURL { get; set; }
    }



    

}
