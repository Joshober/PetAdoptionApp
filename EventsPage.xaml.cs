using Microsoft.Maui.Controls;
using MAUI_Tutorial1_TodoList.Models;
using System.Linq;

namespace MAUI_Tutorial1_TodoList.Views
{
    public partial class EventsPage : ContentPage
    {
        public EventsPage()
        {
            InitializeComponent();
        }

        private async void OnEventTapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is PetEvent petEvent)
            {
                await Navigation.PushAsync(new EventDetailPage(petEvent));
            }
        }
    }
}
