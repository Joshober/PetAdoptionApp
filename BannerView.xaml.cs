using Microsoft.Maui.Controls;
using MAUI_Tutorial1_TodoList.Helpers;

namespace MAUI_Tutorial1_TodoList.Controls
{
    public partial class BannerView : ContentView
    {
        public static readonly BindableProperty ProfileImageSourceProperty =
            BindableProperty.Create(
                nameof(ProfileImageSource),
                typeof(ImageSource),
                typeof(BannerView),
                ImageSource.FromFile("default_profile.png"));

        public ImageSource ProfileImageSource
        {
            get => (ImageSource)GetValue(ProfileImageSourceProperty);
            set => SetValue(ProfileImageSourceProperty, value);
        }

        // New bindable property for the postal code.
        public static readonly BindableProperty PostalCodeProperty =
            BindableProperty.Create(
                nameof(PostalCode),
                typeof(string),
                typeof(BannerView),
                default(string));

        public string PostalCode
        {
            get => (string)GetValue(PostalCodeProperty);
            set => SetValue(PostalCodeProperty, value);
        }

        public BannerView()
        {
            InitializeComponent();
            // Initialize the PostalCode property from a global setting.
            PostalCode = GlobalSettings.PostalCode;
        }

        private void OnProfileImageTapped(object sender, EventArgs e)
        {
            DropdownMenu.IsVisible = !DropdownMenu.IsVisible;
        }
    }
}
