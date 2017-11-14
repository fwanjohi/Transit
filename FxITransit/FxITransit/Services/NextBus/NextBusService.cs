using Acr.UserDialogs;
using FxITransit.Helpers;
using FxITransit.Models;
using PCLStorage;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace FxITransit.Services.NextBus
{

    //public sealed class Singleton
    //{
    //    private static readonly Lazy<Singleton> instance = new Lazy<Singleton>(() => new Singleton());
    //    private Singleton() { }
    //    public static Singleton Instance { get { return instance.Value; } }
    //}

    public class NextBusService : ITransitService
    {

        private static readonly Lazy<NextBusService> instance = new Lazy<NextBusService>(() => new NextBusService());

        public static NextBusService Instance { get { return instance.Value; } }

        public Position LastPosition { get; private set; }
        private DbHelper _dbHelper;
        private NextBusService()
        {
            _client = GetClient();
            _dbHelper = DbHelper.Instance;


        }

        private HttpClient _client;

        public void RefreshDatabase()
        {
            _dbHelper.RefreshDatabase();

        }

        public async Task<IEnumerable<Agency>> GetAgencyList()
        {
            var list = await _dbHelper.GetAgencyListAsync();

            if (list.Count == 0)
            {
                UserDialogs.Instance.ShowLoading("Loading routes from net...", MaskType.Black);
                await Task.Delay(TimeSpan.FromMilliseconds(10));
                var data = await _client.GetStringAsync(EndPoints.AgenciesUrl());
                var doc = XDoc.LoadXml(data);
                foreach (var node in doc.GetDescendantElements("agency"))
                {
                    var agency = new Agency();
                    agency.Title = node.GetAttribute("title");
                    agency.RegionTitle = node.GetAttribute("regionTitle");
                    agency.Tag = node.GetAttribute("tag");
                    agency.Id = agency.Tag;
                    agency.ParentId = "NextBusSetvice";
                    _dbHelper.SaveAgency(agency);
                    list.Add(agency);
                }
            }
            UserDialogs.Instance.HideLoading();

            return list;
        }


        public async Task<IEnumerable<Route>> GetRouteList(Agency agency, bool showDialogs = true)
        {
            var routes = new List<Route>();

            //check if db has routes already
            routes = await _dbHelper.GetRoutesListAsync(agency);
            if (routes.Count == 0)
            {
                if (showDialogs)
                {
                    UserDialogs.Instance.ShowLoading("Loading routes from net...", MaskType.Black);
                }

                var xml = await _client.GetStringAsync(EndPoints.RoutesUrl(agency.Tag));
                var doc = XDoc.LoadXml(xml);

                foreach (var node in doc.GetDescendantElements("route"))
                {
                    var route = new Route();
                    route.Tag = node.GetAttribute("tag");
                    route.Id = $"{agency.Id}.{route.Tag}";
                    route.ParentId = agency.Id;
                    route.Title = node.GetAttribute("title");
                    route.AgencyTitle = agency.Title;
                    routes.Add(route);
                    route.AgencyTag = agency.Tag;
                    _dbHelper.SaveRoute(route);
                    //GetRouteDetails(route);
                }
                if (showDialogs)
                {
                    UserDialogs.Instance.HideLoading();
                }
            }

            return routes;
        }


        public async Task GetRouteDetails(Route route)
        {
            //Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.ShowLoading("Configuring Details from DB...", MaskType.Black));
            //UserDialogs.Instance.ShowLoading($"Configuring Details from DB...", MaskType.Black);

            var count = await _dbHelper.ConfigureRoute(route);
            //UserDialogs.Instance.HideLoading();

            if (!route.IsConfigured)
            {
                UserDialogs.Instance.ShowLoading($"Configuring Details from Service", MaskType.Black);
                await Task.Delay(TimeSpan.FromMilliseconds(1));

                //http://webservices.nextbus.com/service/publicXMLFeed?command=routeConfig&a=sf-muni&r=N

                string xml = _client.GetStringAsync(
                    EndPoints.RouteConfigUrl(route.AgencyTag, route.Tag)).Result;

                var url = EndPoints.RouteConfigUrl(route.AgencyTag, route.Tag);

                var doc = XDoc.LoadXml(xml);


                var stops = new List<Stop>();
                foreach (var stopNode in doc.GetDescendantElements("stop"))
                {
                    var stop = new Stop();
                    stop.Lat = 0;
                    stop.Lon = 0;
                    var lat = stopNode.GetAttribute("lat");
                    var lon = stopNode.GetAttribute("lon");
                    try
                    {
                        try
                        {
                            stop.Lat = Convert.ToDouble(stopNode.GetAttribute("lat"));
                            stop.Lon = Convert.ToDouble(stopNode.GetAttribute("lon"));
                        }
                        catch
                        {
                        }

                        stop.Tag = stopNode.GetAttribute("tag");
                        stop.Title = stopNode.GetAttribute("title");
                        stop.StopId = stopNode.GetAttribute("stopId");

                        stop.RouteTag = route.Tag;
                        stop.AgencyTag = route.AgencyTag;
                        stop.RouteTitle = route.Title;
                        stop.AgencyTitle = route.AgencyTitle;
                        stops.Add(stop);

                    }
                    catch (Exception ex)
                    {
                        var ss = ex.Message;
                    }


                }


                //foreach (XElement directionNode in dirN
                //{

                //}

                //directions
                route.Directions.Clear();
                foreach (XElement directionNode in doc.GetDescendantElements("direction"))
                {
                    var direction = new Direction();

                    direction.Tag = directionNode.GetAttribute("tag");
                    direction.Id = $"{route.Id}.{direction.Tag}";
                    direction.ParentId = route.Id;
                    direction.Title = directionNode.GetAttribute("title");
                    direction.Name = directionNode.GetAttribute("name");
                    direction.UseForUI = directionNode.GetAttribute("useForUI");
                    direction.RouteTag = route.Tag;
                    

                    route.Directions.Add(direction);

                    //get stops
                    direction.Stops.Clear();
                    foreach (var stopIdNode in directionNode.Descendants())
                    {
                        var tag = stopIdNode.GetAttribute("tag");
                        var stop = stops.FirstOrDefault(x => x.Tag == tag);
                        if (stop != null)
                        {
                            stop.Id = $"{direction.Id}.{stop.Tag}";
                            stop.ParentId = direction.Id;
                            stop.DirectionTitle = direction.Title;
                            stop.AgencyTitle = route.AgencyTitle;
                            direction.Stops.Add(stop);
                            stop.DirectionTag = direction.Tag;
                            if (stop.Tag != tag)
                            {
                                stop.OtherStops.Add(stop.Postion);
                            }
                            _dbHelper.SaveStop(stop);
                        }
                    }
                    //var pathXml = "<path>";
                    //foreach (var pathNode in directionNode.Descendants().Where(x => x.Name == "point"))
                    //{
                    //    pathXml += pathNode.ToString();
                    //}
                    //pathXml += "</path>";
                    //direction.PathData = pathXml;

                    _dbHelper.SaveDirection(direction);

                }
                UserDialogs.Instance.HideLoading();
            }

            route.IsConfigured = true;

            //?command=predictions&a=sf-muni&r=2&s=6594&useShortTitles=true
            //http://webservices.nextbus.com/service/publicXMLFeed?command=predictions&a=sf-muni&r=2&s=6594&useShortTitles=true

        }

        public void GetPredictionsFromService(IList<Stop> stops)
        {
            foreach (var stop in stops)
            {

                var x = EndPoints.PredictionsUrl(stop.AgencyTag, stop.RouteTag, stop.Tag);

                var xml = _client.GetStringAsync(EndPoints.PredictionsUrl(stop.AgencyTag, stop.RouteTag, stop.Tag)).Result;

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
                    pred.Vehicle = predNode.GetAttribute("vehicle");
                    pred.LocalTime = UtilsHelper.Instance.ConvertUnixTimeStamp(pred.EpochTime);
                    preds.Add(pred);
                }
                stop.Predictions.ReplaceRange(preds.OrderBy(t => Convert.ToDouble(t.EpochTime)));
                stop.Prediction1 = preds.Count >= 1 ? preds[0] : null;
                stop.Prediction2 = preds.Count >= 2 ? preds[1] : null;
                stop.Prediction3 = preds.Count >= 3 ? preds[2] : null;

            }

        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(EndPoints.BaseUrl);
            return client;
        }


    }



}
