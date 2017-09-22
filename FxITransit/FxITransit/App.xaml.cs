using Acr.Settings;
using FxITransit.Helpers;
using FxITransit.Models;
using FxITransit.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FxITransit
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            SettingsHelper.Instance.Alerts.AutoRefresh = true;
            SettingsHelper.Instance.Alerts = Settings.Current.Bind<AlertSettings>();// persisted bidirectionally with 

            SetMainPage();
        }

         ~App()
        {
            SettingsHelper.Instance.Alerts.AutoRefresh = false;
        }


        public static void SetMainPage()
        {
            Current.MainPage = new TabbedPage
            {
                Children =
                {
                    new NavigationPage(new AgencyListView())
                    {
                        Title = "Browse",
                        Icon = Device.OnPlatform("tab_feed.png",null,null)
                    },

                    new NavigationPage(new SettingsPage())
                    {
                        Title = "Settings",
                        Icon = Device.OnPlatform("tab_feed.png",null,null)
                    },

                    new NavigationPage(new LogsPage())
                    {
                        Title = "Logs",
                        Icon = Device.OnPlatform("tab_about.png",null,null)
                    },
                    new NavigationPage(new AboutPage())
                    {
                        Title = "About",
                        Icon = Device.OnPlatform("tab_about.png",null,null)
                    },
                }
            };
        }
        protected override void OnStart()
        {
            base.OnStart();

        }
    }
}
