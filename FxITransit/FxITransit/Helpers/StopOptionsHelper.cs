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
using Newtonsoft.Json;
using PCLStorage;

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
                _mySettings.Update();
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

        public async Task ChangeFavouriteStop(Stop stop, bool isFave = true)
        {
            stop.IsFavorited = isFave;

            Device.BeginInvokeOnMainThread(() =>
            {

                if (MySettings.FavoriteStops == null)
                {
                    MySettings.FavoriteStops = new List<Stop>();

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


        public void StartAutoRefresh()
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


        public void LoadPredictions()
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

        public async void SaveSttingsToFile()
        {
            UtilsHelper.Instance.Log("----------saving---------");
            string json = JsonConvert.SerializeObject(MySettings);
            var root = FileSystem.Current.LocalStorage;

            var folderName = "fxitransit";
            IFolder myFolder = null;
            if (await root.CheckExistsAsync(folderName) == ExistenceCheckResult.FolderExists)
            {
                UtilsHelper.Instance.Log($"{folderName} exits");
                myFolder = await root.GetFolderAsync(folderName);
            }
            else
            {
                UtilsHelper.Instance.Log($"{folderName} DOES NOT exit... creating one at {root.Path}, {root.Name}");
            }
            if (myFolder == null)
            {
                UtilsHelper.Instance.Log($"Error creating folder at {root.Path}, {root.Name}");
                return;
            }

            IFile myFile = null;

            var fileName = "settings.json";


            if ( await myFolder.CheckExistsAsync(fileName) == ExistenceCheckResult.FileExists)
            {
                myFile = await myFolder.GetFileAsync(fileName);
                UtilsHelper.Instance.Log($"The File {fileName} exits at, {myFolder.Name}");
            }
            else
            {
                UtilsHelper.Instance.Log($"The File does NOT EXIST at, {myFolder.Name} LET ME CREATE IT");
                myFile = await myFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                if (myFile == null)
                {
                    UtilsHelper.Instance.Log($"Error creating file");
                    return;
                }


            }
            UtilsHelper.Instance.Log(json);
            await myFile.WriteAllTextAsync(json);
            UtilsHelper.Instance.Log("----------DONE SAVING---------");

        }


        public async void LoadSettingsFromFile()
        {
            UtilsHelper.Instance.Log("----------START LOADING---------");
            var fileName = "settings.json";
            var root = FileSystem.Current.LocalStorage;
            var folderName = "fxitransit";
            IFolder myFolder = null;
            if (await root.CheckExistsAsync(folderName) == ExistenceCheckResult.FolderExists )
            {
                UtilsHelper.Instance.Log($"{folderName} exits");
                myFolder = await root.GetFolderAsync(folderName);
            }
            else
            {
                UtilsHelper.Instance.Log($"{folderName} DOES NOT exiSt... at {root.Path}, {root.Name}");
                return;
            }


            IFile myFile = null;

            myFile = await myFolder.GetFileAsync(fileName);

            if (myFile != null)
            {
                UtilsHelper.Instance.Log("Loading settings from file " + myFile.Path);
                var json = await myFile.ReadAllTextAsync();
                if (json.HasValue())
                {
                    UtilsHelper.Instance.Log(json);
                    var settings = JsonConvert.DeserializeObject<MySettings>(json);
                    if (settings != null)
                    {

                        MySettings = settings;
                        UtilsHelper.Instance.Log("loaded the settings... " + settings.FavoriteStops.Count);

                        //MySettings = new MySettings();
                        MySettings.FavoriteStops = new List<Stop>(settings.FavoriteStops);


                    }
                    else
                    {
                        UtilsHelper.Instance.Log("FUCK... could not load settings...");
                    }
                    

                    var items = settings.FavoriteStops.Count;
                }
                UtilsHelper.Instance.Log("----------DONE LOADING---------");
            }
            else
            {
                UtilsHelper.Instance.Log("ERROR GETTING settings from file " + myFile.Path);
            }
        }

        ~StopOptionsHelper()
        {

        }

    }
}
