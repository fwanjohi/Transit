using FxITransit.Helpers;
using FxITransit.Models;
using FxITransit.Services;
using FxITransit.Services.NextBus;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.TextToSpeech;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace FxITransit.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        private Stop _closestStop;
        public ITransitService TransitService { get; private set; }
        public TrackingHelper Tracking { get; private set; }

        public PreferencesHelper Settings { get; private set; }
        public UtilsHelper Utils { get; private set; }
        public DbHelper Db { get; private set; }

        public Command<DbEntity> ChangeFavoriteCommand { get; set; }

        public BaseViewModel()
        {
            TransitService = NextBusService.Instance;
            Tracking = TrackingHelper.Instance;
            Settings = PreferencesHelper.Instance;
            Utils = UtilsHelper.Instance;
            Db = DbHelper.Instance;

            ChangeFavoriteCommand = new Command<DbEntity>((entity =>
            {
                entity.IsFavorite = !entity.IsFavorite;
            }));
        }


        public Stop ClosestStop
        {
            get { return _closestStop; }
            set
            {
                _closestStop = value;
                OnPropertyChanged("ClosestStop");
                try
                {

                    if (_closestStop != null)
                    {
                        if (Settings.Preference.Speak)
                        {
                            Speak("The closest stop is " + _closestStop.TitleDisplay);
                        }
                    }
                }
                catch
                {
                }
                ;

            }
        }





        /// <summary>
        /// Private backing field to hold the title
        /// </summary>
        string title = string.Empty;

        /// <summary>
        /// Public property to set and get the title of the item
        /// </summary>
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

       

        protected void Speak(string text)
        {
            UtilsHelper.Instance.Speak(text);

        }




    }

    public class Hoge : ObservableObject
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }

        private Color _Color;

        public Color Color
        {
            get { return _Color; }
            set { SetProperty(ref _Color, value); }
        }

        private double _Width;

        public double Width
        {
            get { return _Width; }
            set { SetProperty(ref _Width, value); }
        }

        private double _Height;

        public double Height
        {
            get { return _Height; }
            set { SetProperty(ref _Height, value); }
        }
    }
}

