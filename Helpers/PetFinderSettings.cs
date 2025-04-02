using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace MAUI_Tutorial1_TodoList.Helpers
{
    public static class PetFinderSettings
    {
        // Base URL for Petfinder; note: the URL below is adjusted to include location.
        public static string Url { get; set; } = "https://www.petfinder.com/search/dogs-for-adoption/us/mo/64082/";
        public static string Token { get; set; } = "Lm1GdvdQPbRSr6HTXG6TUhFT7mcbXn6Iy6lUKmmbgVQ";
        public static int Distance { get; set; } = 100;
        public static int StartIndex { get; set; } = 1;
        public static string Type { get; set; } = "dogs";
        public static string Sort { get; set; } = "nearest";
        public static string LocationSlug { get; set; } = "us/mo/64082";

        public static Dictionary<string, string> Headers { get; } = new Dictionary<string, string>
        {
            { "accept", "application/json, text/plain, */*" },
            { "accept-language", "en-US,en;q=0.9" },
            { "cache-control", "no-cache" },
            { "pragma", "no-cache" },
            { "priority", "u=1, i" },
            { "sec-fetch-mode", "cors" },
            { "sec-fetch-site", "same-origin" },
            { "x-requested-with", "XMLHttpRequest" }
        };

        // BuildQueryString constructs a URL with query parameters using both fixed values and filters.
        public static string BuildQueryString()
        {
            var queryParams = new Dictionary<string, string>
            {
                { "page", StartIndex.ToString() },
                { "limit[]", "10" },
                { "status", "adoptable" },
                { "token", Token },
                { "distance[]", Distance.ToString() },
                { "type[]", Type },
                { "sort[]", Sort },
                { "location_slug[]", LocationSlug },
                { "include_transportable", "true" }
            };

            // Use GlobalFilterSettings.CurrentFilters to add filter parameters.
            var filters = GlobalFilterSettings.CurrentFilters;

            // Example: add age values as age[0], age[1], etc.
            if (filters.Age != null && filters.Age.Any())
            {
                for (int i = 0; i < filters.Age.Count; i++)
                    queryParams[$"age[{i}]"] = filters.Age[i];
            }
            // Add attributes if needed.
            if (filters.Attribute != null && filters.Attribute.Any())
            {
                for (int i = 0; i < filters.Attribute.Count; i++)
                    queryParams[$"attribute[{i}]"] = filters.Attribute[i];
            }
            // Add coat_length.
            if (filters.CoatLength != null && filters.CoatLength.Any())
            {
                for (int i = 0; i < filters.CoatLength.Count; i++)
                    queryParams[$"coat_length[{i}]"] = filters.CoatLength[i];
            }
            // Add color.
            if (filters.Color != null && filters.Color.Any())
            {
                for (int i = 0; i < filters.Color.Count; i++)
                    queryParams[$"color[{i}]"] = filters.Color[i];
            }

            // You can add other filters similarly...

            return QueryStringHelper.AddQueryString(Url, queryParams);
        }
    }
}

