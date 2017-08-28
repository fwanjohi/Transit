using FxITransit.Helpers;
using FxITransit.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;



namespace FxITransit.Services.NextBus
{ 

    public class NextBusService : ITransitService
    {
        public NextBusService()
        {
            _client = GetClient();
        }

        private HttpClient _client;


        public async Task<IEnumerable<Agency>> GetAgencyList()
        {
            var data = await _client.GetStringAsync(EndPoints.AgenciesUrl());
            //var doc = new XDocument();

            XDocument doc = XDoc.LoadXml(data);



            var list = new  List<Agency>();

            foreach (var node in doc.GetDescendantElements("agency"))
            {

                var agency = new Agency();

                agency.Title = node.GetAttribute("title");
                agency.RegionTitle = node.GetAttribute("regionTitle");
                agency.Tag = node.GetAttribute("tag");
                list.Add(agency);
            }

            return list;
        }
        public async Task<IEnumerable<Route>> GetRouteList(string agencyTag)
        {
            var routes = new List<Route>();

            var xml = await _client.GetStringAsync(EndPoints.RoutesUrl(agencyTag));

            var doc = XDoc.LoadXml(xml);

            foreach (var node in doc.GetDescendantElements("route"))
            {
                var route = new Route();
                route.Tag = node.GetAttribute("tag");
                route.Title = node.GetAttribute("title");
                routes.Add(route);
                route.AgencyTag = agencyTag;
            }
            return routes;

        }


        public async Task PopulateRouteDetails(Route route)
        {
            //http://webservices.nextbus.com/service/publicXMLFeed?command=routeConfig&a=sf-muni&r=N
            if (!route.IsConfigured)
            {
                var xml = _client.GetStringAsync(EndPoints.RouteConfigUrl(route.AgencyTag, route.Tag)).Result;

                var doc = XDoc.LoadXml(xml);


                var stops = new List<Stop>();
                foreach (var stopNode in doc.GetDescendantElements("stop"))
                {
                    var stop = new Stop();
                    stop.Lat = stopNode.GetAttribute("lat");
                    stop.Lon = stopNode.GetAttribute("lon");
                    stop.Tag = stopNode.GetAttribute("tag");
                    stop.Title = stopNode.GetAttribute("title");
                    stop.StopId = stopNode.GetAttribute("stopId");
                    stop.RouteTag = route.Tag;
                    stop.AgencyTag = route.AgencyTag;
                    stops.Add(stop);
                }

                //directions
                route.Directions.Clear();
                foreach (var directionNode in doc.GetDescendantElements("direction"))
                {
                    var direction = new Direction();
                    direction.Tag = directionNode.GetAttribute("tag");
                    direction.Title = directionNode.GetAttribute("title");
                    direction.Name = directionNode.GetAttribute("name");
                    direction.UseForUI = directionNode.GetAttribute("useForUI");
                    route.Directions.Add(direction);

                    //get stops
                    direction.Stops.Clear();
                    foreach (var stopIdNode in directionNode.Descendants())
                    {
                        var tag = stopIdNode.GetAttribute("tag");
                        var stop = stops.FirstOrDefault(x => x.Tag == tag);
                        if (stop != null)
                        {
                            direction.Stops.Add(stop);
                        }
                    }

                }

            }
            route.IsConfigured = true;


            //?command=predictions&a=sf-muni&r=2&s=6594&useShortTitles=true
            //http://webservices.nextbus.com/service/publicXMLFeed?command=predictions&a=sf-muni&r=2&s=6594&useShortTitles=true

        }

        public async Task GetStopPredictions(Stop stop)
        {
            
            var x = EndPoints.PredictionsUrl(stop.AgencyTag, stop.RouteTag, stop.Tag);

            var xml =  _client.GetStringAsync(EndPoints.PredictionsUrl(stop.AgencyTag, stop.RouteTag, stop.Tag)).Result;

            var doc = XDoc.LoadXml(xml);

            /*
             * <prediction tripTag="7679393" 
             * block="9718" 
             * vehicle="1537" 
             * dirTag="N____O_F00" 
             * isDeparture="false" minutes="20" seconds="1220" epochTime="1503625693370" vehiclesInConsist="2"/>
             */
            List<Prediction> preds = new List<Prediction>();

            
            foreach (var predNode in doc.GetDescendantElements("prediction"))
            {
                var pred = new Prediction();
                pred.Minutes = predNode.GetAttribute("minutes");
                pred.Seconds = predNode.GetAttribute("seconds");
                pred.EpochTime = predNode.GetAttribute("epochTime");
                pred.IsDeparture = predNode.GetAttribute("isDeparture");
                pred.DirTag = predNode.GetAttribute("dirTag");
                pred.LocalTime = ConvertUnixTimeStamp(pred.EpochTime);
                preds.Add(pred);
            }
            stop.Predictions.ReplaceRange(preds);


        }

        public static DateTime? ConvertUnixTimeStamp(string unixTimeStamp)
        {
            var UTC = new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(Convert.ToDouble(unixTimeStamp));
            var date = UTC.ToLocalTime();
            return date;
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(EndPoints.BaseUrl);
            return client;
        }

       
    }

    
   
}
