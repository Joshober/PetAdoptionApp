using Microsoft.Maui.Controls;
using MAUI_Tutorial1_TodoList.ViewModel;
using CommunityToolkit.Maui.Views;
using MAUI_Tutorial1_TodoList.Views; // For LocationPopup and ChatPopup
using MAUI_Tutorial1_TodoList.Helpers; // Assuming GlobalSettings and CurrentFilters are defined here
using System;

namespace MAUI_Tutorial1_TodoList.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly HomeViewModel _vm;

        public HomePage()
        {
            InitializeComponent();
            _vm = App.Current.Services.GetService<HomeViewModel>();
            BindingContext = _vm;
            if (BindingContext is HomeViewModel vm)
                vm.OpenChatRequested += (_, __) => this.ShowPopup(new ChatPopup());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Prompt for location if not already set
            if (string.IsNullOrWhiteSpace(GlobalSettings.PostalCode))
            {
                var popup = new LocationPopup();
                var result = await this.ShowPopupAsync(popup);
                if (result is string zipcode && !string.IsNullOrWhiteSpace(zipcode))
                {
                    GlobalSettings.PostalCode = zipcode;
                }
            }

            if (_vm.Pets.Count == 0)
                await _vm.LoadCommand.ExecuteAsync(null);
            if (_vm.Reccomend.Count == 0)
                await _vm.LoadRecommend.ExecuteAsync(null);
        }
    }
}
