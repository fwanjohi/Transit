using FxITransit.Helpers;
using FxITransit.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FxITransit.ViewModels
{
    public class PredictionsViewModel : BaseViewModel
    {
        //private Timer _refreshTimer;
        //private Timer _timeTimer;

        public PredictionsViewModel() : base()
        {
            StopAlerts = new MySettings
            {
                Alert = true,
                AlertInterval = 1,
                AlertMinsBefore = 5

            };
        }
        public Command LoadPredictionsCommand { get; private set; }
        public Command AutoRefreshPredictionsCommand { get; private set; }

        public Command GoogleDirectionsCommand { get; set; }
        public Command ChangeFavoriteCommand { get; set; }

        public Stop Stop { get; private set; }
        public bool AutoRefresh
        {
            get { return Settings.Alerts.AutoRefresh; }
            set { Settings.Alerts.AutoRefresh = value; }
        }

        public MySettings StopAlerts
        {
            get; set;
        }

        public int RefreshInterval { get; set; }

        private int _elapsedTime;
        private DateTime nextAlert;

        public PredictionsViewModel(Stop stop)
        {
            _elapsedTime = 0;
            AutoRefresh = true;
            RefreshInterval = 30;
            nextAlert = DateTime.Now;
            Stop = stop;
            Title = $"Predictions - {stop.TitleDisplay }";
            LoadPredictionsCommand = new Command(async () => await ExecuteLoadPredictionsCommand());
            AutoRefreshPredictionsCommand = new Command(async () => await ExecuteRefreshCommand());
            GoogleDirectionsCommand = new Command(async () => await ExecuteGoogleDirectionCommand(stop));

            ChangeFavoriteCommand = new Command(async () => await ExecuteFavoriteCommand());
        }
        

        private async Task ExecuteFavoriteCommand()
        {
            OptionsHelper.Instance.ChangeFavouriteStop(Stop);

        }


        private async Task ExecuteRefreshCommand()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (!AutoRefresh) return false;
                if (_elapsedTime >= RefreshInterval)
                {

                    ExecuteLoadPredictionsCommand();
                    _elapsedTime = 0;


                }
                else
                {
                    foreach (var pred in Stop.Predictions)
                    {
                        UpdatePrediction(pred, false);

                    }
                }

                _elapsedTime++;
                return AutoRefresh;  // True = Repeat again, False = Stop the timer
            });
        }

        private void UpdatePrediction(Prediction pred, bool alert = false)
        {
            pred.LocalTime = UtilsHelper.Instance.ConvertUnixTimeStamp(pred.EpochTime);
            if (pred.LocalTime.HasValue)
            {
                int diff = (int)pred.LocalTime.Value.Subtract(DateTime.Now).TotalMinutes;

                if (diff <= Settings.Alerts.AlertMinsBefore)
                {
                    pred.IsArriving = true;
                    if (alert && nextAlert <= DateTime.Now)
                    {
                        var vehicle = pred.Vehicle ?? "";

                        var msg = $"Your transit vehicle {vehicle} is arriving in {diff} Minutes at {Stop.TitleDisplay}";
                        Settings.SendNotification(msg);

                        if (Settings.Alerts.Speak)
                        {
                            Speak(msg);
                        }

                        if (Settings.Alerts.Vibrate)
                        {

                        }

                        nextAlert = nextAlert.AddMinutes(Settings.Alerts.AlertInterval);


                    }
                }
                else
                {
                    pred.IsArriving = false;
                }

            }
        }

        private async Task ExecuteLoadPredictionsCommand()
        {

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var stops = new List<Stop> {Stop};
                await TransitService.GetStopPredictions(stops);

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
            finally
            {
                IsBusy = false;
            }
        }


        private async Task ExecuteGoogleDirectionCommand(Stop currentStop)
        {

        }


    }
}
