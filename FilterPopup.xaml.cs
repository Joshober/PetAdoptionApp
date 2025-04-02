using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using MAUI_Tutorial1_TodoList.Helpers;
using MAUI_Tutorial1_TodoList.Models;
using MAUI_Tutorial1_TodoList.Services;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;

namespace MAUI_Tutorial1_TodoList.Views
{
    public partial class FilterPopup : Popup
    {
        private UserService _userService;

        public FilterPopup()
        {
            
            InitializeComponent();
            Opened += FilterPopup_Opened;
        }

        private void FilterPopup_Opened(object sender, PopupOpenedEventArgs e)
        {
            // Set the pickers from the current global filter options.
            SetPickerSelectedItem(AnimalTypePicker, GlobalFilterSettings.CurrentFilters.AnimalType);
            SetPickerSelectedItem(BreedPicker, GlobalFilterSettings.CurrentFilters.Breed.FirstOrDefault() ?? string.Empty);
            SetPickerSelectedItem(AgePicker, GlobalFilterSettings.CurrentFilters.Age.FirstOrDefault() ?? string.Empty);
            SetPickerSelectedItem(SizePicker, GlobalFilterSettings.CurrentFilters.Size.FirstOrDefault() ?? string.Empty);
            SetPickerSelectedItem(GenderPicker, GlobalFilterSettings.CurrentFilters.Gender.FirstOrDefault() ?? string.Empty);
            SetPickerSelectedItem(HouseholdPicker, GlobalFilterSettings.CurrentFilters.Household.FirstOrDefault() ?? string.Empty);
            SetPickerSelectedItem(CoatLengthPicker, GlobalFilterSettings.CurrentFilters.CoatLength.FirstOrDefault() ?? string.Empty);
            SetPickerSelectedItem(ColorPicker, GlobalFilterSettings.CurrentFilters.Color.FirstOrDefault() ?? string.Empty);
            SetPickerSelectedItem(DaysPicker, GlobalFilterSettings.CurrentFilters.DaysOnPetfinder.FirstOrDefault() ?? string.Empty);
            SetPickerSelectedItem(ShelterPicker, GlobalFilterSettings.CurrentFilters.Shelter.FirstOrDefault() ?? string.Empty);
        }

        private void SetPickerSelectedItem(Picker picker, string value)
        {
            if (picker?.ItemsSource is IEnumerable<string> items)
            {
                var list = items.ToList();
                int index = string.IsNullOrWhiteSpace(value) ? 0 : list.IndexOf(value);
                picker.SelectedIndex = index >= 0 ? index : 0;
            }
        }

        public event EventHandler<FilterEventArgs> FiltersApplied;

        private async void OnApplyFiltersClicked(object sender, EventArgs e)
        {
            // Create a new FilterOptions object from the pickers.
            var newFilters = new FilterOptions
            {
                AnimalType = (AnimalTypePicker.SelectedItem as string) ?? string.Empty,
                Breed = string.IsNullOrWhiteSpace(BreedPicker.SelectedItem as string)
                            ? new List<string>()
                            : new List<string> { (string)BreedPicker.SelectedItem },
                Age = string.IsNullOrWhiteSpace(AgePicker.SelectedItem as string)
                            ? new List<string>()
                            : new List<string> { (string)AgePicker.SelectedItem },
                Size = string.IsNullOrWhiteSpace(SizePicker.SelectedItem as string)
                            ? new List<string>()
                            : new List<string> { (string)SizePicker.SelectedItem },
                Gender = string.IsNullOrWhiteSpace(GenderPicker.SelectedItem as string)
                            ? new List<string>()
                            : new List<string> { (string)GenderPicker.SelectedItem },
                Household = string.IsNullOrWhiteSpace(HouseholdPicker.SelectedItem as string)
                            ? new List<string>()
                            : new List<string> { (string)HouseholdPicker.SelectedItem },
                CoatLength = string.IsNullOrWhiteSpace(CoatLengthPicker.SelectedItem as string)
                            ? new List<string>()
                            : new List<string> { (string)CoatLengthPicker.SelectedItem },
                Color = string.IsNullOrWhiteSpace(ColorPicker.SelectedItem as string)
                            ? new List<string>()
                            : new List<string> { (string)ColorPicker.SelectedItem },
                DaysOnPetfinder = string.IsNullOrWhiteSpace(DaysPicker.SelectedItem as string)
                            ? new List<string>()
                            : new List<string> { (string)DaysPicker.SelectedItem },
                Shelter = string.IsNullOrWhiteSpace(ShelterPicker.SelectedItem as string)
                            ? new List<string>()
                            : new List<string> { (string)ShelterPicker.SelectedItem },
                Attribute = new List<string>() // Adjust if you have attribute pickers.
            };
            var handler = new HttpClientHandler
            {
                // WARNING: In production, do not ignore certificate errors.
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            // Update global filter settings.
            GlobalFilterSettings.CurrentFilters = newFilters;
            var token = await SecureStorage.GetAsync("auth_token");
            HttpClient httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            _userService = new UserService(httpClient);


            // Retrieve the current user from your global settings.
            var currentUser = CurrentUserSettings.CurrentUser;
            if (string.IsNullOrWhiteSpace(currentUser?.Email))
            {
                Debug.WriteLine("No User Detected");
                FiltersApplied?.Invoke(this, new FilterEventArgs(newFilters));
                Close();
                return;
            }
            if (!string.IsNullOrWhiteSpace(newFilters.AnimalType))
                currentUser.FilterAnimalType = newFilters.AnimalType;

            // Assign list-based filters.
            currentUser.FilterBreed = newFilters.Breed;
            currentUser.FilterGender = newFilters.Gender;
            currentUser.FilterAge = newFilters.Age;
            currentUser.FilterSize = newFilters.Size;
            currentUser.FilterHousehold = newFilters.Household;
            currentUser.FilterCoatLength = newFilters.CoatLength;
            currentUser.FilterColor = newFilters.Color;
            currentUser.FilterDaysOnPetfinder = newFilters.DaysOnPetfinder;
            currentUser.FilterShelter = newFilters.Shelter;
            currentUser.FilterAttribute = newFilters.Attribute;

     

            // Attempt to update the user on the backend.
            bool updateSuccess = await _userService.UpdateUserAsync(currentUser);

            if (!updateSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to update filter options.", "OK");
            }

            else
            {
                // Optionally, show a success message or log success.
            }
            FiltersApplied?.Invoke(this, new FilterEventArgs(newFilters));
            Close();
        }

        private void OnClearFiltersClicked(object sender, EventArgs e)
        {
            BreedPicker.SelectedIndex = 0;
            AgePicker.SelectedIndex = 0;
            SizePicker.SelectedIndex = 0;
            GenderPicker.SelectedIndex = 0;
            HouseholdPicker.SelectedIndex = 0;
            CoatLengthPicker.SelectedIndex = 0;
            ColorPicker.SelectedIndex = 0;
            DaysPicker.SelectedIndex = 0;
            ShelterPicker.SelectedIndex = 0;
            AnimalTypePicker.SelectedIndex = 0;

            GlobalFilterSettings.CurrentFilters = new FilterOptions();
            FiltersApplied?.Invoke(this, new FilterEventArgs(GlobalFilterSettings.CurrentFilters));
            Close();
        }
    }

    public class FilterEventArgs : EventArgs
    {
        public FilterOptions Filters { get; }
        public FilterEventArgs(FilterOptions filters)
        {
            Filters = filters;
        }
    }
}
