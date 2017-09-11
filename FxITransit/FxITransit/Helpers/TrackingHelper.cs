﻿using FxITransit.Models;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxITransit.Helpers
{
   
    

    public  class TrackingHelper
    {
        private static readonly Lazy<TrackingHelper> instance = new Lazy<TrackingHelper>(() => new TrackingHelper());

        public static TrackingHelper Instance { get { return instance.Value; } }

        private IGeolocator _locator;
        private TrackingHelper()
        {
            
            InitializeGeoLocator();


        }

        private void InitializeGeoLocator()
        {

            _locator = CrossGeolocator.Current;

            if (_locator.IsGeolocationAvailable && _locator.IsGeolocationEnabled)
            {
                _locator.DesiredAccuracy = 500; //meters

                _locator.PositionChanged += _locator_PositionChanged;
                _locator.PositionError += _locator_PositionError;

                DateTime start = DateTime.Now;
                LastPosition = AsyncHelper.RunSync(() => _locator.GetPositionAsync(TimeSpan.FromSeconds(10), null, false));
                var secs = start.Subtract(DateTime.Now).TotalSeconds;

                _locator.StartListeningAsync(TimeSpan.FromSeconds(10), 10, false);
            }

        }
       
        private  double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        public  double CalculateDistance(Stop location1, Stop location2)
        {
            var loc1 = new Location { Latitude = location1.Lat, Longitude = location1.Lon };
            var loc2 = new Location { Latitude = location2.Lat, Longitude = location2.Lon };
            return CalculateDistance(loc1, loc2);
        }

        public  double ToMiles(double kms)
        {
            return kms * ((double)5 / (double)8);
        }

        public  double CalculateDistance(Location location1, Location location2)
        {
            double circumference = 40000.0; // Earth's circumference at the equator in km
            double distance = 0.0;

            //Calculate radians
            double latitude1Rad = DegreesToRadians(location1.Latitude);
            double longitude1Rad = DegreesToRadians(location1.Longitude);
            double latititude2Rad = DegreesToRadians(location2.Latitude);
            double longitude2Rad = DegreesToRadians(location2.Longitude);

            double logitudeDiff = Math.Abs(longitude1Rad - longitude2Rad);

            if (logitudeDiff > Math.PI)
            {
                logitudeDiff = 2.0 * Math.PI - logitudeDiff;
            }

            double angleCalculation =
                Math.Acos(
                  Math.Sin(latititude2Rad) * Math.Sin(latitude1Rad) +
                  Math.Cos(latititude2Rad) * Math.Cos(latitude1Rad) * Math.Cos(logitudeDiff));

            distance = circumference * angleCalculation / (2.0 * Math.PI);

            return distance;
        }

        public  double CalculateDistance(params Location[] locations)
        {
            double totalDistance = 0.0;

            for (int i = 0; i < locations.Length - 1; i++)
            {
                Location current = locations[i];
                Location next = locations[i + 1];

                totalDistance += CalculateDistance(current, next);
            }

            return totalDistance;
        }

        

        public Stop GetClosestStop(IEnumerable<Stop> stops)
        {

            Stop closestStop = null;
            var lastPos = LastPosition;
            Stop curStop = new Stop { Lat = lastPos.Latitude, Lon = lastPos.Longitude };

            foreach (var stop in stops)
            {

                stop.Distance = ToMiles(CalculateDistance(curStop, stop));
                if (closestStop == null)
                {
                    closestStop = stop;
                }
                else
                {
                    if (closestStop.Distance > stop.Distance)
                    {
                        closestStop = stop;
                    }
                }
            }
            var xPos = new Xamarin.Forms.Maps.Position(closestStop.Lat, closestStop.Lon);
            return closestStop;
            //Map.Pins.Add(new Xamarin.Forms.Maps.Pin { Position = xPos, Address = ClosestStop.Title, Label = ClosestStop.TitleDisplay });
        }

        private void _locator_PositionError(object sender, PositionErrorEventArgs e)
        {
            var X = 1;
        }

        private void _locator_PositionChanged(object sender, PositionEventArgs e)
        {
            LastPosition = e.Position;
        }

        public Position LastPosition { get; private set; }
        public object TransitService { get; private set; }
    }
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
