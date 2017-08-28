using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static string RouteConfigUrl(string agencyTag, string routeTag)
        {
            return $"?command=routeConfig&a={agencyTag}&r={routeTag}";

        }

        public static string PredictionsUrl(string agencyTag, string routeTag, string stopTag)
        {
            return $"?command=predictions&a={agencyTag}&r={routeTag}&s={stopTag}&useShortTitles=true";
        }

    }
}
