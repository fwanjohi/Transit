using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FxITransit.Helpers;
using FxITransit.Models;

namespace FxITransit.ViewModels
{
    public class StopPredictionBaseViewModel : BaseViewModel
    {
        private DateTime _nextAlert = DateTime.Now;

        public Action<List<Prediction>> OnPredictionsChanged { get;  set; }
        protected void UpdatePredictions(List<Prediction> predictions)
        {
            foreach (var pred in predictions)
            {
                pred.LocalTime = UtilsHelper.Instance.ConvertUnixTimeStamp(pred.EpochTime);
                if (pred.LocalTime.HasValue)
                {
                    int diff = (int)pred.LocalTime.Value.Subtract(DateTime.Now).TotalMinutes;

                    if (diff <= Settings.Preference.AlertMinsBefore)
                    {
                        pred.IsArriving = true;
                        if (PreferencesHelper.Instance.Preference.Alert && _nextAlert <= DateTime.Now)
                        {
                            var vehicle = pred.Vehicle ?? "";

                            var msg = $"Your transit vehicle {vehicle} is arriving in {diff} Minutes";
                            Utils.SendNotification(msg);

                            if (Settings.Preference.Speak)
                            {
                                Speak(msg);
                            }

                            if (Settings.Preference.Vibrate)
                            {

                            }

                            _nextAlert = _nextAlert.AddMinutes(Settings.Preference.AlertInterval);

                        }
                    }
                    else
                    {
                        pred.IsArriving = false;
                    }
                    pred.UpdatePreditionDisplay();
                }
            }
        }
    }

}
