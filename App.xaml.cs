using MAUI_Tutorial1_TodoList.Views;
using Microsoft.Extensions.DependencyInjection;

namespace MAUI_Tutorial1_TodoList
{
    public partial class App : Application
    {
        // Cast Application.Current to your App type for easier access.
        public new static App Current => (App)Application.Current;

        // Expose the DI container.
        public IServiceProvider Services { get; }

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            Services = serviceProvider;
            MainPage = new AppShell();
        }
    }
}
