using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI_Tutorial1_TodoList.Helpers;
using MAUI_Tutorial1_TodoList.Models;
using Microsoft.Maui.Devices.Sensors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAUI_Tutorial1_TodoList.ViewModel
{
    public partial class EventsViewModel : ObservableObject
    {
        public EventsViewModel()
        {
            Events = new ObservableCollection<PetEvent>();
            // Initialise the command.
            LoadCommand = new AsyncRelayCommand(LoadEventsAsync);
            // Optionally call it on construction.
            _ = LoadEventsAsync();
        }

        [ObservableProperty]
        private ObservableCollection<PetEvent> events;

        [ObservableProperty]
        private bool isLoading;

        public IAsyncRelayCommand LoadCommand { get; }

        private async Task LoadEventsAsync()
        {
            IsLoading = true;
            Events.Clear();

            // Retrieve postal code from global settings.
            string postalCode = GlobalSettings.PostalCode;
            Debug.WriteLine($"Initial postal code: {postalCode}");

            // If postal code is missing or "Unknown", attempt to get it from device location.
            if (string.IsNullOrWhiteSpace(postalCode) || postalCode.Equals("Unknown", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));
                    if (location != null)
                    {
                        var placemarks = await Geocoding.GetPlacemarksAsync(location);
                        var placemark = placemarks?.FirstOrDefault();
                        postalCode = placemark?.PostalCode;
                        Debug.WriteLine($"Retrieved postal code from geolocation: {postalCode}");
                        // Update global settings for future use.
                        if (!string.IsNullOrWhiteSpace(postalCode))
                        {
                            GlobalSettings.PostalCode = postalCode;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Location error: {ex.Message}");
                }
            }

            // Build the API URL.
            var apiKey = Environment.GetEnvironmentVariable("SERP_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Debug.WriteLine("SERP_API_KEY is missing.");
            }
            else
            {
                Debug.WriteLine($"Loaded SERP_API_KEY? {(!string.IsNullOrWhiteSpace(apiKey))}");
            }
            var url = $"https://10.0.2.2:7291/api/Users/GetEvents/{postalCode}";
            Debug.WriteLine($"Events API URL: {url}");

            try
            {
         
                var handler = new HttpClientHandler
                {
                    // WARNING: In production, do not ignore certificate errors.
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };
                using HttpClient httpClient = new HttpClient(handler);
                var response = await httpClient.GetStringAsync(url);
                Debug.WriteLine("SERP API response received.");
                using JsonDocument document = JsonDocument.Parse(response);
                var eventsList = new List<PetEvent>();

                if (document.RootElement.TryGetProperty("events_results", out JsonElement eventsElement))
                {
                    foreach (var eventJson in eventsElement.EnumerateArray())
                    {
                        string title = eventJson.GetProperty("title").GetString();

                        // Process date information.
                        string when = "";
                        DateTime startDate = DateTime.MaxValue; // fallback if parsing fails

                        if (eventJson.TryGetProperty("date", out JsonElement dateElement))
                        {
                            if (dateElement.TryGetProperty("when", out JsonElement whenElement))
                                when = whenElement.GetString();
                            else if (dateElement.TryGetProperty("start_date", out JsonElement startDateElement))
                                when = startDateElement.GetString();

                            // Attempt to parse the "start_date" (e.g., "Mar 26")
                            if (dateElement.TryGetProperty("start_date", out JsonElement startDateElement2))
                            {
                                string startDateStr = startDateElement2.GetString();
                                if (!string.IsNullOrEmpty(startDateStr))
                                {
                                    // Using "MMM d" format. Adjust format if necessary.
                                    if (DateTime.TryParseExact(startDateStr, "MMM d", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                                    {
                                        // Set the start date to the current year with parsed month and day.
                                        startDate = new DateTime(DateTime.Now.Year, parsedDate.Month, parsedDate.Day);
                                    }
                                }
                            }
                        }

                        // Combine address array into a single string.
                        string address = "";
                        if (eventJson.TryGetProperty("address", out JsonElement addressElement) &&
                            addressElement.ValueKind == JsonValueKind.Array)
                        {
                            var parts = new List<string>();
                            foreach (var part in addressElement.EnumerateArray())
                            {
                                if (part.ValueKind == JsonValueKind.String)
                                    parts.Add(part.GetString());
                            }
                            address = string.Join(", ", parts);
                        }

                        // Get description, link, and image URL (prefer "image" over "thumbnail").
                        string description = eventJson.TryGetProperty("description", out JsonElement descElem)
                                                    ? descElem.GetString()
                                                    : string.Empty;
                        string link = eventJson.TryGetProperty("link", out JsonElement linkElem)
                                                    ? linkElem.GetString()
                                                    : string.Empty;
                        string imageUrl = "";
                        if (eventJson.TryGetProperty("image", out JsonElement imageElement))
                        {
                            imageUrl = imageElement.GetString();
                        }
                        else if (eventJson.TryGetProperty("thumbnail", out JsonElement thumbElem))
                        {
                            imageUrl = thumbElem.GetString();
                        }

                        // Only add event if not a duplicate based on link.
                        if (!eventsList.Any(e => e.Link == link))
                        {
                            var petEvent = new PetEvent
                            {
                                Title = title,
                                When = when,
                                Address = address,
                                Description = description,
                                Link = link,
                                ImageUrl = imageUrl,
                                StartDate = startDate
                            };

                            eventsList.Add(petEvent);
                        }
                    }

                    // Order events by StartDate and add to the ObservableCollection.
                    foreach (var petEvent in eventsList.OrderBy(e => e.StartDate))
                    {
                        Events.Add(petEvent);
                    }
                }
                else
                {
                    Events.Add(new PetEvent { Title = "No upcoming pet events found." });
                }
            }
            catch (Exception ex)
            {
                Events.Add(new PetEvent { Title = $"Error loading events: {ex.Message}" });
                Debug.WriteLine($"Error loading events: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
