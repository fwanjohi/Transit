using Acr.Settings;
using FxITransit.Models;
using Plugin.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using FxITransit.Services;
using FxITransit.Services.NextBus;
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

    public class StopOptionsHelper : ObservableObject
    {
        private static readonly Lazy<StopOptionsHelper> _instance = new Lazy<StopOptionsHelper>(() => new StopOptionsHelper());
        private int _elapsedTime;
        private MySettings _mySettings;
        private bool _viewUpdates = false;
        private ITransitService _transitService;

        public static StopOptionsHelper Instance => _instance.Value;
        public ObservableRangeCollection<Stop> _stopsToUpdate;
        public Command<Stop> FavoriteCommand { get; set; }
        public Command<Route> FavoriteRouteCommand { get; set; }
        

        public Action<List<Prediction>> OnPredictionsChanged { get; set; }

        private StopOptionsHelper()
        {
            MySettings = new MySettings();
            _stopsToUpdate = new ObservableRangeCollection<Stop>();
            _transitService = NextBusService.Instance;
        }

        public ObservableRangeCollection<Stop> ViewStopsToUpdate
        {
            get => _stopsToUpdate;
            set
            {
                _stopsToUpdate = value;
                OnPropertyChanged("ViewStopsToUpdate");
            }
        }

        public MySettings MySettings
        {
            get { return _mySettings; }
            set
            {
                _mySettings = value;
                OnPropertyChanged("MySettings");
            }
        }
        public ITransitService TransitService
        {
            get { return _transitService; }
            set
            {
                _transitService = value;
                OnPropertyChanged("TransitService");
            }
        }

       



        public async Task ChangeFavouriteStop(Stop stop)
        {
            stop.IsFavorited = !stop.IsFavorited;

            Device.BeginInvokeOnMainThread(() =>
            {

                if (MySettings.FavoriteStops == null)
                {
                    MySettings.FavoriteStops = new ObservableRangeCollection<Stop>();

                }
                var fav = MySettings.FavoriteStops.FirstOrDefault(x => x.Tag == stop.Tag);
                var index = MySettings.FavoriteStops.IndexOf(fav);
                if (fav == null && stop.IsFavorited)
                {
                    MySettings.FavoriteStops.Add(stop);
                }

                else if (fav != null && !stop.IsFavorited)
                {
                    try
                    {
                        MySettings.FavoriteStops.RemoveAt(index);
                        var inWatch = StopOptionsHelper.Instance.ViewStopsToUpdate.FirstOrDefault(x => x.StopId == fav.StopId);
                        if (inWatch != null)
                        {
                            StopOptionsHelper.Instance.ViewStopsToUpdate.Remove(stop);
                        }
                    }
                    catch (Exception e)
                    {
                    }

                }
                MySettings.Update();
                OnPropertyChanged("MySettings");
            });
        }

        
        public void  StartAutoRefresh()
        {
            _viewUpdates = true;

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (this.MySettings.AutoRefresh && _viewUpdates)
                {
                    if (_elapsedTime >= this.MySettings.RefreshInterval)
                    {

                        LoadPredictions();
                        _elapsedTime = 0;
                    }
                    else
                    {
                        UpdatePredictionsDisplay();
                    }
                }

                _elapsedTime++;
                return MySettings.AutoRefresh && _viewUpdates;  // True = Repeat again, False = Stop the timer
            });
        }

        public void StopAutoRefresh()
        {
            _viewUpdates = true;
        }


        public void  LoadPredictions()
        {
            try
            {
                TransitService.GetPredictionsFromService(ViewStopsToUpdate);
                if (OnPredictionsChanged != null)
                {
                    foreach (var stop in ViewStopsToUpdate)
                    {
                        OnPredictionsChanged.Invoke(stop.Predictions.ToList());
                    }
                    
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load Predictions.",
                    Cancel = "OK"
                }, "message");
            }

        }

        public void UpdatePredictionsDisplay()
        {
            foreach (var stop in this.ViewStopsToUpdate)
            {
                stop.UpdateDiaplay();
                
            }

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
        ~StopOptionsHelper()
        {
            Acr.Settings.Settings.Current.UnBind(this);
        }

    }
}
