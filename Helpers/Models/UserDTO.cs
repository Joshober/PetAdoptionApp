using System.Text.Json.Serialization;

namespace MAUI_Tutorial1_TodoList.Models
{
    public class UserDTO
    {

        public int UserID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfilePhoto { get; set; } = "default_profile.png";
        // Filter properties:

        public string FilterAnimalType  { get; set; }
        public string FilterGenderString { get; set; }

        // Expose a List<string> property for your UI code
        [JsonIgnore]
        public List<string> FilterGender
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FilterGenderString))
                    return new List<string>();
                return new List<string> { FilterGenderString };
            }
            set
            {
                // If you need to store multiple values, decide how to handle them:
                // E.g., just store the first in FilterGenderString
                FilterGenderString = value?.FirstOrDefault() ?? string.Empty;
            }
        }
        public List<string> FilterBreed { get; set; } = new List<string>();
        
        public List<string> FilterAge { get; set; } = new List<string>();
        public List<string> FilterSize { get; set; } = new List<string>();
        public List<string> FilterHousehold { get; set; } = new List<string>();
        public List<string> FilterCoatLength { get; set; } = new List<string>();
        public List<string> FilterColor { get; set; } = new List<string>();
        public List<string> FilterDaysOnPetfinder { get; set; } = new List<string>();
        public List<string> FilterShelter { get; set; } = new List<string>();
        public List<string> FilterAttribute { get; set; } = new List<string>();
    }
}
