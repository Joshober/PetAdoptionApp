using System.Collections.Generic;
using System.Linq;

namespace MAUI_Tutorial1_TodoList.Helpers
{


    public static class GlobalSettings
    {
        public static string PostalCode { get; set; } = "64082";
        public static int RadiusMiles { get; set; } = 10;
    }
    public static class PetPlaceSettings
    {
        public static string Url { get; set; } = "https://api.petplace.com/animal/";

        // These properties control the POST payload.
        public static string PostalCode { get; set; } = GlobalSettings.PostalCode; // Update if needed.
        public static int RadiusMiles { get; set; } = GlobalSettings.RadiusMiles;
        public static int StartIndex { get; set; } = 100;
        public static string FilterAnimalType { get; set; } = "Dog";
    }
    
}
