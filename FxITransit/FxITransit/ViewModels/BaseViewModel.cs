using FxITransit.Helpers;
using FxITransit.Models;
using FxITransit.Services;
using FxITransit.Services.NextBus;
using Plugin.Geolocator;
using Plugin.TextToSpeech;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FxITransit.ViewModels
{
    public  class BaseViewModel : ObservableObject
    {
        /// <summary>
        /// Get the azure service instance
        /// </summary>
        public ITransitService TransitService { get; private set; }
        public TrackingHelper TrackingHelper { get; private set; }

        public SettingsHelper Settings { get; private set; }

        Stop _closestStop;
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


        public BaseViewModel()
        {
            TransitService =  NextBusService.Instance;
            TrackingHelper = TrackingHelper.Instance;
            Settings = SettingsHelper.Instance;
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

        public Xamarin.Forms.Point DeviceLocation
        {
            get; set;
        }

        
        

        public  void Speak(string text)
        {
             CrossTextToSpeech.Current.Speak(text);
            
        }

        


    }
}

