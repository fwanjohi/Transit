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


namespace FxITransit.ViewModels
{
    public  class BaseViewModel : ObservableObject
    {
        private Stop _closestStop;
        public ITransitService TransitService { get; private set; }
        public TrackingHelper Tracking { get; private set; }

        public SettingsHelper Settings { get; private set; }
        public UtilsHelper Utils { get; private set; }

        public BaseViewModel()
        {
            TransitService = NextBusService.Instance;
            Tracking = TrackingHelper.Instance;
            Settings = SettingsHelper.Instance;
            Utils = UtilsHelper.Instance;

        }

        
        public Stop ClosestStop
        {
            get
            {
                return _closestStop;
            }
            set
            {
                _closestStop = value;
                OnPropertyChanged("ClosestStop");
                try
                {
                    
                    if (_closestStop != null)
                    {
                        Speak("The closest stop is " + _closestStop.TitleDisplay);
                    }
                }
                catch { };

            }
        }


       

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
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

        protected Position DeviceLocation
        {
            get => Tracking.LastPosition;
        }
        
        protected  void Speak(string text)
        {
            UtilsHelper.Instance.Speak(text);
            
        }

        


    }
}

