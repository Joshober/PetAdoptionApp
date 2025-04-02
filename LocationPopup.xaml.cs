using CommunityToolkit.Maui.Views;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;

namespace MAUI_Tutorial1_TodoList.Views
{
    public partial class LocationPopup : Popup
    {
        public LocationPopup()
        {
            InitializeComponent();
        }

        private void OnOkClicked(object sender, EventArgs e)
        {
            // Return the text entered in the Entry as the result.
            Close(ZipEntry.Text);
        }
    }
}
