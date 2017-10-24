using Acr.Settings;
using FxITransit.Helpers;
using FxITransit.Models;
using FxITransit.Views;
using Plugin.Notifications;
using System.Collections.Generic;
using System.Linq;
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

            StopOptionsHelper.Instance.LoadSettingsFromFile();

            //StopOptionsHelper.Instance.MySettings = Settings.Current.Bind<MySettings>();// persisted bidirectionally with 
            if (StopOptionsHelper.Instance.MySettings.AlertMinsBefore == 0)
            {
                StopOptionsHelper.Instance.MySettings.AlertMinsBefore = 5;
                StopOptionsHelper.Instance.MySettings.AlertInterval = 1;
                StopOptionsHelper.Instance.MySettings.Speak = true;
                StopOptionsHelper.Instance.MySettings.Alert = true;


            }
            StopOptionsHelper.Instance.MySettings.AutoRefresh = true;

            if (StopOptionsHelper.Instance.MySettings.FavoriteStops == null)
            {
                StopOptionsHelper.Instance.MySettings.FavoriteStops = new List<Stop>();
            }
            var result = CrossNotifications.Current.RequestPermission().Result;


            SetMainPage();
        }

        ~App()
        {
            StopOptionsHelper.Instance.MySettings.AutoRefresh = false;
        }


        public static void SetMainPage()
        {
            var main = new MainLaunchPage()
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
                        Icon = Device.OnPlatform("fave_on.png",null,null)
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
            Current.MainPage = main;

            UtilsHelper.Instance.Log("AFTER CHECKING SETTINGS, FAVE STOPS = " + StopOptionsHelper.Instance.MySettings.FavoriteStops.Count);

            if (StopOptionsHelper.Instance.MySettings.FavoriteStops.Count > 0)
            {
                var faves = main.Children.FirstOrDefault(x => (x as NavigationPage).Title == "Favorites");

                if (faves != null)
                {
                    main.CurrentPage = faves;
                }
            }

            



        }
        protected override void OnStart()
        {
            base.OnStart();

        }

        
    }
}
