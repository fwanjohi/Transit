using FxITransit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FxITransit.Helpers;

namespace FxITransit.Services.NextBus
{
    public static class EndPoints
    {
        public const string BaseUrl = "http://webservices.nextbus.com/service/publicXMLFeed";
        public static string AgenciesUrl()
        {
            return "?command=agencyList";
        }
        public static string RoutesUrl(string agencyTag)
        {
            return $"?command=routeList&a={agencyTag}";
        }

        public static string RouteConfigUrl(string agencyTag, string routeTag= null)
        {
            var cfg= $"?command=routeConfig&a={agencyTag}&terse";
            //if routeTag is added, then include it to return config for one route only
            if (!string.IsNullOrEmpty(routeTag))
            {
                cfg += $"&r={routeTag}";
            }

            return cfg;

        }

        public static string PredictionsUrl(string agencyTag, string routeTag, string stopTag)
        {
            return $"?command=predictions&a={agencyTag}&r={routeTag}&s={stopTag}&useShortTitles=true";
        }

        public static string StopDirectionsUrl(Stop start, Stop destination)
        {
            // https://maps.googleapis.com/maps/api/directions/json?origin=New+York,+NY&destination=Boston,+MA&waypoints=optimize:true|Providence,+RI|Hartford,+CT&key=AIzaSyC2scZS8w3cAHdAr8iIPJtDRRBQl6b-gwk

            var uri = $"maps.googleapis.com/maps/api/directions/json?origin={start.Lat},{start.Lon}&waypoints=optimize:true|{destination.Lat},{destination.Lon}&key=AIzaSyC2scZS8w3cAHdAr8iIPJtDRRBQl6b-gwk";
            return "https://" +uri;
        }

        public static string StopDirectionsUrl(string start, string destination)
        {
            // https://maps.googleapis.com/maps/api/directions/json?origin=New+York,+NY&destination=Boston,+MA&waypoints=optimize:true|Providence,+RI|Hartford,+CT&key=AIzaSyC2scZS8w3cAHdAr8iIPJtDRRBQl6b-gwk

            var uri = $"maps.googleapis.com/maps/api/directions/json?origin={start}&waypoints=optimize:true|{destination}&key=AIzaSyC2scZS8w3cAHdAr8iIPJtDRRBQl6b-gwk";
            return "https://" + uri;
        }

        public static string GoogleAddressUrl(string address)
        {
            var pos = TrackingHelper.Instance.LastPosition;
            var latLong = $"{pos.Latitude},{pos.Longitude}";

            var apiKey = "AIzaSyC2scZS8w3cAHdAr8iIPJtDRRBQl6b-gwk";
            var encoded = System.Net.WebUtility.UrlEncode(address);
            var radius = TrackingHelper.Instance.MetersFromMiles(10);

            var uri = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={latLong}&radius={radius}&name={encoded}&key={apiKey}";
            

            return uri;
        }

        public static string GoogleReverseGeocodeUrl(double lat, double lon)
        {
            var apiKey = "AIzaSyC2scZS8w3cAHdAr8iIPJtDRRBQl6b-gwk";
            var uri = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={lat},{lon}&key{apiKey}";
            return uri;
        }

    }
}
