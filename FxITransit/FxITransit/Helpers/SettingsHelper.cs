using Acr.Settings;
using FxITransit.Models;
using Plugin.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxITransit.Helpers
{
    public class SettingsHelper : ObservableObject
    {
        private static readonly Lazy<SettingsHelper> instance = new Lazy<SettingsHelper>(() => new SettingsHelper());

        public static SettingsHelper Instance { get { return instance.Value; } }


        public SettingsHelper()
        {
            //Settings.Current.Bind(this);
            //if (!Settings.Current.Contains("settings"))
            //{

            //    //Settings.Current.SetValue("settings", this);
            //    //Settings.Current.Bind<AlertSetttings>();


            Alerts = new AlertSettings();
            Favorites = new FavoriteSettings();
            Alerts.Alert = true;
            Alerts.AlertInterval = 1;
            Alerts.AlertMinsBefore = 5;

            //}
            //else
            //{
            //    var items = Settings.Current.Get< SettingsHelper>("settings");
            //}
        }

        public AlertSettings Alerts { get; set; }

        public FavoriteSettings Favorites { get; set; }


        public void SendNotification(string message)
        {
            try
            {
                CrossNotifications.Current.Send(new Notification
                {
                    Title = "Alert!",
                    Message = message,
                    Vibrate = true,
                    When = TimeSpan.FromSeconds(10)
                });
            }
            catch (Exception ex)
            {
                UtilsHelper.Instance.Log(ex.Message);

                try
                {
                    UtilsHelper.Instance.Speak(ex.Message);
                }
                catch (Exception ex2)
                {
                    UtilsHelper.Instance.Log(ex2.Message);
                }
            }
        }
        ~SettingsHelper()
        {
            Settings.Current.UnBind(this);
        }
    }
}
