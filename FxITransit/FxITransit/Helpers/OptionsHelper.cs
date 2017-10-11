using Acr.Settings;
using FxITransit.Models;
using Plugin.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FxITransit.Helpers
{
    //public class Container
    //{
    //    public Container()
    //    {
    //        Alerts = new Models.Settings();
    //        Favorites = new FavoriteSettings();
    //        Alerts.Alert = true;
    //        Alerts.AlertInterval = 1;
    //        Alerts.AlertMinsBefore = 5;

    //        Favorites = new FavoriteSettings();
    //    }

    //    public Settings Alerts { get; set; }

    //    public FavoriteSettings Favorites { get; set; }
    //}

    public class OptionsHelper : ObservableObject
    {
        private static readonly Lazy<OptionsHelper> instance = new Lazy<OptionsHelper>(() => new OptionsHelper());

        public static OptionsHelper Instance { get { return instance.Value; } }

        private Alerts _alerts;


        public OptionsHelper()
        {
            Alerts = new Alerts();
            FavoriteCommand = new Command<Stop>(ChangeFavourite);
        }

        public Alerts Alerts
        {
            get { return _alerts; }
            set
            {
                _alerts = value;
                OnPropertyChanged("Alerts");
            }
        }

        public Command<Stop> FavoriteCommand { get; set; }

        public void ChangeFavourite(Stop stop)
        {
            var s = stop;
            Device.BeginInvokeOnMainThread(() =>
            {

                if (Alerts.FavoriteStops == null)
                {
                    Alerts.FavoriteStops = new ObservableRangeCollection<Stop>();

                }

                

                var fav = Alerts.FavoriteStops.FirstOrDefault(x => x.Tag == stop.Tag);

                if (fav == null && stop.IsFavorited)
                {
                    Alerts.FavoriteStops.Add(stop);
                }

                else if (fav != null && !stop.IsFavorited)
                {
                    Alerts.FavoriteStops.Remove(fav);
                }
                Alerts.Update();
                OnPropertyChanged("Alerts");
            });
        }

        public void SendNotification(string message)
        {
            try
            {
                CrossNotifications.Current.Send(new Notification
                {
                    Title = "Alert!",
                    Message = message,
                    Vibrate = true,
                    When = TimeSpan.FromSeconds(2)
                });
            }
            catch (Exception ex)
            {
                UtilsHelper.Instance.Log("Send Notification Failed : " + ex.Message);

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
        ~OptionsHelper()
        {
            Acr.Settings.Settings.Current.UnBind(this);
        }

    }
}
