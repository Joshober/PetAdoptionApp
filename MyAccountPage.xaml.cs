using MAUI_Tutorial1_TodoList.ViewModel;
using Microsoft.Maui.Controls;

namespace MAUI_Tutorial1_TodoList.Views
{
    public partial class MyAccountPage : ContentPage
    {
        public MyAccountPage()
        {
            InitializeComponent();
            BindingContext = new MyAccountViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is MyAccountViewModel vm)
            {
                await vm.CheckIfLoggedInAsync();
            }
        }
    }
}
