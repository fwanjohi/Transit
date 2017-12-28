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
using Acr.UserDialogs;

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

    public class PreferencesHelper : ObservableObject
    {
        private static readonly Lazy<PreferencesHelper> _instance = new Lazy<PreferencesHelper>(() => new PreferencesHelper());
        private int _elapsedTime;
        private Preference _preference;
        private bool _viewUpdates = false;
        private ITransitService _transitService;

        public static PreferencesHelper Instance => _instance.Value;
        public ObservableRangeCollection<Stop> _stopsToUpdate;

        public Command<Stop> FavoriteCommand { get; set; }
        private ObservableRangeCollection<Stop> _favoriteStops = new ObservableRangeCollection<Stop>();
        public Command<Route> FavoriteRouteCommand { get; set; }


        public Action<List<Prediction>> OnPredictionsChanged { get; set; }

        private PreferencesHelper()
        {
            _preference = new Preference();
            _stopsToUpdate = new ObservableRangeCollection<Stop>();
            _transitService = NextBusService.Instance;
            _preference = DbHelper.Instance.GetPreference();
            _favoriteStops = new ObservableRangeCollection<Stop>();
            _favoriteStops.ReplaceRange(DbHelper.Instance.GetFavoriteStops());
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

        //public ObservableRangeCollection<Stop> FavoriteStops
        //{
        //    get { return _favoriteStops; }
        //    set
        //    {
        //        _favoriteStops = value;
        //        OnPropertyChanged("FavoriteStops");
        //    }
        //}



        public void Update()
        {
            OnPropertyChanged("FavoriteStops");
            //OnPropertyChanged("FavoriteRoutes");

        }

        public Preference Preference
        {
            get { return _preference; }
            set
            {
                _preference = value;
                OnPropertyChanged("Preference");
               
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
        //public void RemoveFavorite(Stop stop)
        //{
        //    var fav = FavoriteStops.FirstOrDefault(x => x.Tag == stop.Tag);
        //    if (fav != null)
        //    {
        //        stop.IsFavorite = false;
        //        FavoriteStops.Remove(stop);
        //        var inWatch = ViewStopsToUpdate.FirstOrDefault(x => x.StopId == fav.StopId);
        //        if (inWatch != null)
        //        {
        //            ViewStopsToUpdate.Remove(stop);
        //        }
                
                
        //    }
        //}

        //public void AddFavorite(Stop stop)
        //{
        //    Device.BeginInvokeOnMainThread(() =>
        //    {
        //        if (FavoriteStops == null)
        //        {
        //            FavoriteStops = new ObservableRangeCollection<Stop>();
        //        }

        //        var fav = FavoriteStops.FirstOrDefault(x => x.Tag == stop.Tag);
              

        //        if (fav == null)
        //        {
        //            stop.IsFavorite = true;
        //            FavoriteStops.Add(stop);
        //            var inWatch = ViewStopsToUpdate.FirstOrDefault(x => x.StopId == stop.StopId);
        //            if (inWatch != null)
        //            {
        //                ViewStopsToUpdate.Add(stop);
        //            }
        //        }
                
        //    });
        //}
        
        


        public void StartAutoRefresh()
        {
            _viewUpdates = true;

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (this.Preference.AutoRefresh && _viewUpdates)
                {
                    if (_elapsedTime >= this.Preference.RefreshInterval)
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
                return Preference.AutoRefresh && _viewUpdates;  // True = Repeat again, False = Stop the timer
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

       

        public async void SaveSttingsToFile()
        {
            UtilsHelper.Instance.Log("----------saving---------");
            string json = JsonConvert.SerializeObject(Preference);
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



        ~PreferencesHelper()
        {

        }

    }
}
