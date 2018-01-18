using Acr.Settings;
using Acr.UserDialogs;
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
            
            var result = CrossNotifications.Current.RequestPermission().Result;
            
            SetMainPage();
        }


        ~App()
        {
            PreferencesHelper.Instance.Preference.AutoRefresh = false;
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

                    new NavigationPage(new AddressSearchPage())
                    {
                        Title = "Dest",
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

        }
        protected override void OnStart()
        {
            base.OnStart();

        }

        
    }
}
