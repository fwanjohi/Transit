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
    public class PredictionsViewModel : StopPredictionBaseViewModel
    {
        
        public PredictionsViewModel(Stop stop)
        {
            OnPredictionsChanged = UpdatePredictions;
            _nextAlert = DateTime.Now;
            Stop = stop;
            Title = $"Predictions - {stop.TitleDisplay }";
            ChangeFavoriteCommand = new Command(async () => await StopOptionsHelper.Instance.ChangeFavouriteStop(Stop));
        }

        public Command GoogleDirectionsCommand { get; set; }
        public Command ChangeFavoriteCommand { get; set; }

        public Stop Stop { get; private set; }

        private DateTime _nextAlert;
        
        
        
        private void UpdatePredictions(List<Prediction> predictions)
        {
            foreach (var pred in predictions)
            {
                pred.LocalTime = UtilsHelper.Instance.ConvertUnixTimeStamp(pred.EpochTime);
                if (pred.LocalTime.HasValue)
                {
                    int diff = (int)pred.LocalTime.Value.Subtract(DateTime.Now).TotalMinutes;

                    if (diff <= Settings.MySettings.AlertMinsBefore)
                    {
                        pred.IsArriving = true;
                        if (StopOptionsHelper.Instance.MySettings.Alert && _nextAlert <= DateTime.Now)
                        {
                            var vehicle = pred.Vehicle ?? "";

                            var msg = $"Your transit vehicle {vehicle} is arriving in {diff} Minutes at {Stop.TitleDisplay}";
                            Settings.SendNotification(msg);

                            if (Settings.MySettings.Speak)
                            {
                                Speak(msg);
                            }

                            if (Settings.MySettings.Vibrate)
                            {

                            }

                            _nextAlert = _nextAlert.AddMinutes(Settings.MySettings.AlertInterval);


                        }
                    }
                    else
                    {
                        pred.IsArriving = false;
                    }

                }
            }
        }
    }
}
