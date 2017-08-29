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
        public Command LoadPredictionsCommand { get; private set; }
        public Command AutoRefreshPredictionsCommand { get; private set; }

        public Stop Stop { get; private set; }
        public bool AutoRefresh { get; internal set; }

        public int RefreshInterval  {get; set;}

        private int _elapsedTime;

        public PredictionsViewModel(Stop stop)
        {
            _elapsedTime = 0;
            AutoRefresh = true;
            RefreshInterval = 30;
            Stop = stop;
            Title = $"Predictions : {stop.Title }";
            LoadPredictionsCommand = new Command(async () => await ExecuteLoadPredictionsCommand());
            AutoRefreshPredictionsCommand = new Command(async () => await ExecuteRefreshCommand());
            


        }
        async Task ExecuteRefreshCommand()
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
                        pred.LocalTime = Utils.ConvertUnixTimeStamp(pred.EpochTime);
                    }
                }

                _elapsedTime++;
                return AutoRefresh; // True = Repeat again, False = Stop the timer
            });
        }

        async Task ExecuteLoadPredictionsCommand()
        {

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                
                await DataStore.GetStopPredictions(Stop);

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


    }
}
