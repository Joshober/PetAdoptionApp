using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAUI_Tutorial1_TodoList.Helpers
{
    public static class QueryStringHelper
    {
        public static string AddQueryString(string uri, Dictionary<string, string> queryParams)
        {
            if (string.IsNullOrEmpty(uri))
                throw new ArgumentNullException(nameof(uri));

            if (queryParams == null || !queryParams.Any())
                return uri;

            var separator = uri.Contains("?") ? "&" : "?";
            var query = string.Join("&", queryParams.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
            return uri + separator + query;
        }
    }
}
