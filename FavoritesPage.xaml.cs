using CommunityToolkit.Maui.Views;
using MAUI_Tutorial1_TodoList.Helpers;
using MAUI_Tutorial1_TodoList.ViewModel;
using Microsoft.Maui.Controls;
namespace MAUI_Tutorial1_TodoList.Views
{
    public partial class FavoritesPage : ContentPage
    {
        private readonly FavoritesViewModel _vm;
     
        public FavoritesPage(FavoritesViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _vm = viewModel;
            if (BindingContext is FavoritesViewModel vm)
                vm.OpenChatRequested += (_, __) =>
                    this.ShowPopup(new ChatPopup());
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_vm.Pets.Count == 0)
                await _vm.LoadCommand.ExecuteAsync(null);
        }
    }

}

