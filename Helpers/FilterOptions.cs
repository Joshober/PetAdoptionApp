namespace MAUI_Tutorial1_TodoList.Helpers
{
    public class FilterOptions
    {
        public string AnimalType { get; set; } = string.Empty;
        public List<string> Breed { get; set; } = new List<string>();
        public List<string> Gender { get; set; } = new List<string>();
        public List<string> Age { get; set; } = new List<string>();
        public List<string> Size { get; set; } = new List<string>();
        public List<string> Household { get; set; } = new List<string>();
        public List<string> CoatLength { get; set; } = new List<string>();
        public List<string> Color { get; set; } = new List<string>();
        public List<string> DaysOnPetfinder { get; set; } = new List<string>();
        public List<string> Shelter { get; set; } = new List<string>();
        public List<string> Attribute { get; set; } = new List<string>();
    }
}
