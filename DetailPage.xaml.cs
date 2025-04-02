using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MAUI_Tutorial1_TodoList.Models;
using MAUI_Tutorial1_TodoList.ViewModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace MAUI_Tutorial1_TodoList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage, IQueryAttributable
    {
        private CancellationTokenSource _cts;

        public DetailPage()
        {
            InitializeComponent();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Animal", out var animalObj) && animalObj is Animal animal)
            {
                // Create the view model with the Animal object.
                var detailViewModel = new DetailViewModel(animal);

                // Check if the query contains the Favorite value.
                if (query.TryGetValue("Favorite", out var favoriteObj) && favoriteObj is bool isFavorited)
                {
                    detailViewModel.IsFavorited = isFavorited;
                }
                BindingContext = detailViewModel;
                // Optionally start image loading (or other asynchronous work)
                _ = LoadImageAsync(CancellationToken.None);
            }
            else
            {
                // If no Animal is provided, navigate back.
                Shell.Current.GoToAsync("//AllPetsPage");
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Create a new CancellationTokenSource for this page session.
            _cts = new CancellationTokenSource();

            //if (BindingContext is DetailViewModel viewModel)
            //{
            //    await viewModel.InitializeAsync();
            //}
            _ = LoadImageAsync(_cts.Token);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Cancel and dispose any pending async operations.
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            // Remove this page from the navigation stack to ensure it cannot be revisited.
            if (Navigation?.NavigationStack.Contains(this) == true)
            {
                Navigation.RemovePage(this);
            }
        }

        private async Task LoadImageAsync(CancellationToken token)
        {
            try
            {
                // Simulate asynchronous work (e.g., image processing or download)
                await Task.Delay(2000, token);
                if (token.IsCancellationRequested)
                    return;
                // Here you can update the UI safely.
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("LoadImageAsync was canceled.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in LoadImageAsync: " + ex.Message);
                // Optionally navigate away if there is an error.
                await Shell.Current.GoToAsync("//AllPetsPage");
            }
        }
    }
}
