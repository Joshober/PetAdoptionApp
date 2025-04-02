using Microsoft.Maui.Controls;
using MAUI_Tutorial1_TodoList.Models;
using System;

namespace MAUI_Tutorial1_TodoList.Views
{
    public partial class EventDetailPage : ContentPage
    {
        private readonly PetEvent _petEvent;

        public EventDetailPage(PetEvent petEvent)
        {
            InitializeComponent();
            _petEvent = petEvent;
            BindingContext = _petEvent;
        }

        private async void OnMoreInfoClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_petEvent.Link))
            {
                await Browser.OpenAsync(_petEvent.Link, BrowserLaunchMode.SystemPreferred);
            }
        }
    }
}
