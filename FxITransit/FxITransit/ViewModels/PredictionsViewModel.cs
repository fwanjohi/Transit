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

        public PredictionsViewModel(Stop stop)
        {
            AutoRefresh = true;
            Stop = stop;
            Title = $"Predictions : {stop.Title }";
            LoadPredictionsCommand = new Command(async () => await ExecuteLoadPredictionsCommand());
            AutoRefreshPredictionsCommand = new Command(async () => await ExecuteRefreshCommand());
            //_refreshTimer = new Timer(1 * 60 * 1000);
            //_refreshTimer.Elapsed += OnTimedEvent;
            //_refreshTimer.AutoReset = true;

            //_timeTimer = new Timer(1 * 1000);
            //_timeTimer.Elapsed += OnSecondEvent;
            //_timeTimer.AutoReset = true;


        }
        async Task ExecuteRefreshCommand()
        {
            Device.StartTimer(TimeSpan.FromSeconds(30), () =>
            {
                if (!AutoRefresh) return false;
                ExecuteLoadPredictionsCommand();
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
