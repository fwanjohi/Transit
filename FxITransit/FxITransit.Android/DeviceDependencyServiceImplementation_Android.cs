using Android.Speech.Tts;
using Xamarin.Forms;
using FxITransit.Droid;
using System.Collections.Generic;
using FxITransit.Services;
using System;
using System.Threading.Tasks;
using Android.Runtime;
using Android.Locations;
using Android.OS;
using Android.Content;
using FxITransit.Models;

[assembly: Dependency(typeof(DeviceDependencyServiceImplementation_Droid))]
namespace FxITransit.Droid
{
    public class DeviceDependencyServiceImplementation_Droid :
        Java.Lang.Object,
        IDeviceDependencyService,
        TextToSpeech.IOnInitListener
        , ILocationListener
    {
        private LocationManager _locMan;
        private TextToSpeech _speaker;
        private string _toSpeak;
      
        public Task<GeoPoint> GetDeviceCurrentLocationAsync()
        {
            //locMgr = Context.GetSystemService(Context.LocationService) as LocationManager;
           
            var data = Task.Factory.StartNew(() => GetCurrentLocation());
            return data;

            //var point = new Point { L}
        }

        public GeoPoint GetCurrentLocation()
        {
            _locMan = (LocationManager)Forms.Context.GetSystemService(Context.LocationService);

            var loc = _locMan.GetLastKnownLocation(LocationManager.NetworkProvider);

            var pos = new GeoPoint { Lat = loc.Latitude, Lon = loc.Longitude };
            return pos;
        }




        public void Speak(string text)
        {
            var c = Forms.Context;
            _toSpeak = text;
            if (_speaker == null)
            {
                _speaker = new TextToSpeech(c, this);
            }
            else
            {
                var p = new Dictionary<string, string>();
                _speaker.Speak(_toSpeak, QueueMode.Flush, p);
                System.Diagnostics.Debug.WriteLine("spoke " + _toSpeak);
            }
        }




        #region IOnInitListener implementation
        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                System.Diagnostics.Debug.WriteLine("speaker init");
                var p = new Dictionary<string, string>();
                _speaker.Speak(_toSpeak, QueueMode.Flush, p);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("was quiet");
            }
        }

        public void OnProviderDisabled(string provider) { }
        public void OnProviderEnabled(string provider) { }
        public void OnStatusChanged(string provider,
            Availability status, Android.OS.Bundle extras)
        { }
        //---fired whenever there is a change in location---
        public void OnLocationChanged(Android.Locations.Location location)
        {
            if (location != null)
            {
                LocationEventArgs args = new LocationEventArgs();
                args.Lat = location.Latitude;
                args.Lon = location.Longitude;
                OnlocationObtained(this, args);
            };
        }

        
        //---an EventHandler delegate that is called when a location
        // is obtained---

        public event EventHandler<GeoPoint> OnlocationObtained;

        //---custom event accessor that is invoked when client
        // subscribes to the event---
        //event EventHandler<GeoPoint>
        //    IDeviceDependencyService.OnlocationObtained
        //{
        //    add
        //    {
        //        OnlocationObtained += value;
        //    }
        //    remove
        //    {
        //        OnlocationObtained -= value;
        //    }
        //}
        //---method to call to start getting location---
        public void ObtainMyLocation()
        {
            _locMan = (LocationManager)Forms.Context.GetSystemService(Context.LocationService);

            _locMan.RequestLocationUpdates(
                LocationManager.NetworkProvider,
                    0,   //---time in ms---
                    0,   //---distance in metres---
                    this);
        }
        //---stop the location update when the object is set to
        // null---
        ~DeviceDependencyServiceImplementation_Droid()
        {
            _locMan.RemoveUpdates(this);
        }
        #endregion
    }
}