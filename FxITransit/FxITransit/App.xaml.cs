using Acr.Settings;
using FxITransit.Helpers;
using FxITransit.Models;
using FxITransit.Views;
using Plugin.Notifications;
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
            
            OptionsHelper.Instance.Alerts = Settings.Current.Bind<MySettings>();// persisted bidirectionally with 
            if (OptionsHelper.Instance.Alerts.AlertMinsBefore == 0)
            {
                OptionsHelper.Instance.Alerts.AlertMinsBefore = 5;
                OptionsHelper.Instance.Alerts.AlertInterval = 1;
                OptionsHelper.Instance.Alerts.Speak = true;
                OptionsHelper.Instance.Alerts.Alert = true;


            }
            OptionsHelper.Instance.Alerts.AutoRefresh = true;

            if (OptionsHelper.Instance.Alerts.FavoriteStops == null)
            {
                OptionsHelper.Instance.Alerts.FavoriteStops=  new ObservableRangeCollection<Stop>();
            }
            var result =  CrossNotifications.Current.RequestPermission().Result;
            

            SetMainPage();
        }

         ~App()
        {
            OptionsHelper.Instance.Alerts.AutoRefresh = false;
        }


        public static void SetMainPage()
        {
            Current.MainPage = new MainLaunchPage()
            {
                Children =
                {
                    
                    new NavigationPage(new AgencyListView())
                    {
                        Title = "Browse",
                        Icon = Device.OnPlatform("tab_feed.png",null,null),
                        
                        
                    },

                    new NavigationPage(new FavouritesPage())
                    {
                        Title = "Favorites",
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
