using System.Collections.Generic;
using System.Linq;
namespace MAUI_Tutorial1_TodoList.Helpers
{
    public static class GlobalFilterSettings
    {
        public static FilterOptions CurrentFilters { get; set; } = new FilterOptions();
        public static string PostalCode { get; set; } = "64082";
        public static int RadiusMiles { get; set; } = 10;
        public static int StartIndex { get; set; } = 1;
        public static string? FilterAnimalType { get; internal set; }
    }


}