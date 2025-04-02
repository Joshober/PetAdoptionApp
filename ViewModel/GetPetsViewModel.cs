using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUI_Tutorial1_TodoList.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAUI_Tutorial1_TodoList.ViewModel
{

    public partial class GetPetsViewModel : ObservableObject
    {
        private readonly HttpClient client;
        private readonly JsonSerializerOptions _serializerOptions = new()
        {
            WriteIndented = true
        };
        [ObservableProperty]
        string zipPostal = string.Empty; // Default to empty string
        [ObservableProperty]
        string milesRadius = string.Empty; // Default to empty string
        [ObservableProperty]
        string filterAnimalType = string.Empty; // Default to empty string

        public GetPetsViewModel()
        {

            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [RelayCommand]
        public async Task GetSheltersAsync()
        {
            try
            {
                string apiUrl = "https://api.petplace.com/shelter/search";

                // ✅ Create request body (same as Python)
                var body = new
                {
                    locationInformation = new
                    {
                        zipPostal = "64081",
                        milesRadius = "10"
                    }
                };

                // ✅ Convert object to JSON string
                string jsonBody = JsonSerializer.Serialize(body);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // ✅ Send POST request
                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string rawJson = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Response: {rawJson}");

                    // Deserialize response
                    var data = JsonSerializer.Deserialize<List<Shelter>>(rawJson, _serializerOptions);

                    if (data != null)
                    {
                        Console.WriteLine($"Fetched {data.Count} shelters.");
                    }
                }
                else
                {
                    Console.WriteLine($"Request failed: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching pets: {ex.Message}");
            }
        }
        [RelayCommand]
        public async Task GetZipCodeFromSystemApiAsync()
        {
            try
            {
                // ✅ Step 1: Get Current Location (Latitude & Longitude)
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));
                }

                if (location != null)
                {
                    // ✅ Step 2: Reverse Geocode to Get Zip Code
                    var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                    var placemark = placemarks?.FirstOrDefault();

                    if (placemark != null)
                    {
                        ZipPostal = placemark.PostalCode; // ✅ Set the detected Zip Code
                        Console.WriteLine($"Detected Zip Code: {ZipPostal}");
                    }
                    else
                    {
                        Console.WriteLine("Could not determine postal code.");
                    }
                }
                else
                {
                    Console.WriteLine("Could not determine location.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching zip code: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task GetPetsAsync()
        {

            try
            {
                string apiUrl = "https://api.petplace.com/animal/";

                var body = new
                {
                    locationInformation = new
                    {
                        zipPostal = zipPostal,
                        milesRadius = milesRadius
                    },
                    animalFilters = new
                    {
                        startIndex = 1,
                        filterAnimalType = filterAnimalType
                    }
                };

                string jsonBody = JsonSerializer.Serialize(body);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string rawJson = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Response: {rawJson}");

                    // Deserialize response
                    var jsonDoc = JsonDocument.Parse(rawJson);
                    var animalArray = jsonDoc.RootElement.GetProperty("animal").ToString();

                    var data = JsonSerializer.Deserialize<List<Animal>>(rawJson, _serializerOptions);

                    if (data != null)
                    {
                        Console.WriteLine($"Fetched {data.Count} animals.");
                    }
                }
                else
                {
                    Console.WriteLine($"Request failed: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching pets: {ex.Message}");
            }
        }
    }

}
