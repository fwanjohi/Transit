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

        private NextBusService()
        {
            _client = GetClient();



        }

        private HttpClient _client;



        public async Task<IEnumerable<Agency>> GetAgencyList()
        {
            string xml = string.Empty;
            var root = FileSystem.Current.LocalStorage;
            var myFolder = root.CreateFolder("fxitransit");
            IFile myFile = null;
            if (myFolder != null)
            {
                myFile = myFolder.CreateFile("agencies.xml");
                if (myFile != null)
                {
                    UtilsHelper.Instance.Log("Loading agencies from file " + myFile.Path);
                    xml = await myFile.ReadAllTextAsync();
                }
            }
            
            

            XDocument doc;
            if (xml.HasValue())
            {
                
                doc = XDoc.LoadXml(xml);

            }
            else
            {
                try
                {
                    UtilsHelper.Instance.Log("Loading agencies from Internet");
                    var data = await _client.GetStringAsync(EndPoints.AgenciesUrl());
                    //var doc = new XDocument();
                    doc = XDoc.LoadXml(data);

                    await myFile.WriteAllTextAsync(data);
                }
                catch (Exception e)
                {
                    string err = e.Message;
                    throw;
                }

            }




            var list = new List<Agency>();

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


        public async Task<IEnumerable<Route>> GetRouteList(Agency agency)
        {
            var routes = new List<Route>();

            string xml = string.Empty;
            var root = FileSystem.Current.LocalStorage;
            var myFolder = root.CreateFolder("fxitransit");
            IFile myFile = null;
            if (myFolder != null)
            {
                myFile = myFolder.CreateFile($"{agency.Tag}.routes.xml");
                if (myFile != null)
                {
                    xml = await myFile.ReadAllTextAsync();
                }
            }
            if (!xml.HasValue())
            {
                xml = await _client.GetStringAsync(EndPoints.RoutesUrl(agency.Tag));
                myFile.WriteAllTextAsync(xml);
            }

            var doc = XDoc.LoadXml(xml);

            foreach (var node in doc.GetDescendantElements("route"))
            {
                var route = new Route();
                route.Tag = node.GetAttribute("tag");
                route.Title = node.GetAttribute("title");
                route.AgencyTitle = agency.Title;
                routes.Add(route);
                route.AgencyTag = agency.Tag;
            }
            return routes;

        }


        public async Task GetRouteDetailsFromService(Route route, Action callBack)
        {
            //http://webservices.nextbus.com/service/publicXMLFeed?command=routeConfig&a=sf-muni&r=N
            if (!route.IsConfigured)
            {


                string xml = string.Empty;
                var root = FileSystem.Current.LocalStorage;
                var myFolder = root.CreateFolder("fxitransit");
                IFile myFile = null;
                if (myFolder != null)
                {
                    myFile = myFolder.CreateFile($"{route.AgencyTag}.{route.Tag}.stops.xml");
                    if (myFile != null)
                    {
                        xml = await myFile.ReadAllTextAsync();

                    }
                }
                if (!xml.HasValue())
                {
                    xml = _client.GetStringAsync(
                        EndPoints.RouteConfigUrl(route.AgencyTag, route.Tag)).Result;

                    await myFile.WriteAllTextAsync(xml);
                }
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
                        catch { }

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

                            stop.DirectionTitle = direction.Title;
                            direction.Stops.Add(stop);
                            if (stop.Tag != tag)
                            {
                                stop.OtherStops.Add(stop.Postion);
                            }
                        }
                    }

                }

            }
            route.IsConfigured = true;
            callBack?.Invoke();


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
