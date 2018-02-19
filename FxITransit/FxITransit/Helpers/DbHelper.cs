using FxITransit.Models;
using PCLStorage;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace FxITransit.Helpers
{
    public class DbHelper
    {
        private SQLiteConnection _db;

        private static readonly Lazy<DbHelper> _instance = new Lazy<DbHelper>(() => new DbHelper());

        public static DbHelper Instance { get { return _instance.Value; } }

        private static readonly string _DbPath;

        private DbHelper()
        {
            string xml = string.Empty;
            var root = FileSystem.Current.LocalStorage;
            var myFolder = root.CreateFolderAsync("fxitransit", CreationCollisionOption.OpenIfExists).Result;
            var dbPath = myFolder.Path + "\\fxTransit.db2";

            //if (ex == ExistenceCheckResult.FileExists)
            //{
            //    myFolder.DeleteAsync();
            //}

            _db = new SQLiteConnection(dbPath);
            CreateTables();
        }
        public void CreateTables()
        {
            _db.CreateTable<Agency>();
            _db.CreateTable<Route>();
            _db.CreateTable<Direction>();
            _db.CreateTable<Stop>();

            _db.CreateTable<Preference>();
        }

        public List<Stop> SearchStopsNearAddress(double lat, double lon, double distanceInMiles, string from)
        {
            TableMapping m = new TableMapping(typeof(Stop));

            //var stops = _db.Query<Stop>("select * from Stop").ToList()
            var lites = _db.Query<Stop>("select distinct * from Stop").ToList()
            .Where(
                k => TrackingHelper.Instance.CalculateDistance(
                         k.Lat, k.Lon, lat, lon) <= distanceInMiles).ToList();

            foreach (var x in lites)
            {
                x.Distance = TrackingHelper.Instance.CalculateDistance(x.Lat, x.Lon, lat, lon);
                var dist = x.Distance.ToString("0.##0");
                x.DistanceAwayDisplay = $"{x.AgencyTitle} - {dist} from {from}";

            }
            lites = lites.OrderBy(y=> y.ParentId).ThenBy(x => x.Distance).ToList();
            return lites;
        }

        private PossibleRoute FindCommonDestinations(Stop sourceStop, List<Stop> sharedStops )
        {
            var sourceToDest = new PossibleRoute(sourceStop);
            //only stops going same direction
            var destStops = sharedStops.Where(x => x.ParentId == sourceStop.ParentId).OrderBy(y=>y.Order).ToList();

            //only add stops going that way
            foreach (var destStop in destStops)
            {
                //check for only stops going to destination
                if (destStop.Order >= sourceStop.Order)
                {
                    //stop.Distance = TrackingHelper.Instance.CalculateDistance(sourceStop.Lat, my.Longitude, stop.Lat, stop.Lon);
                    //var dist = stop.Distance.ToString("0.##0");

                    //stop.DistanceAwayDisplay = $"{stop.AgencyTitle} - {dist} away, {dest.Order - stop.Order} stops";
                    sourceToDest.StopsToNear.Add(destStop);
                }
            }

            var closest = sourceToDest.StopsToNear.OrderBy(x => x.Distance).FirstOrDefault();
            sourceToDest.StopAt = closest;


            return sourceToDest;
        }
        public async Task<Destination> SearchDestinationRoutesTo( Stop from, Stop dest, string start, string end)
        {
            double lat; ;
            double lon;

            if (from != null)
            {
                lat = from.Lat;
                lon = from.Lon;
            }
            else
            {
                var my = await TrackingHelper.Instance.GetMyLocation();
                lat = my.Latitude;
                lon = my.Longitude;
            }


            var reverseAdd = await TrackingHelper.Instance.GetAddressReverseGeocode(lat, lon);

            //my = new Plugin.Geolocator.Abstractions.Position();
            //my.Latitude = 37.7859;
            //my.Longitude = 122.41708645;

//            Latitude = Latitude = 37.78884335
//Longitude = -122.41708645

//            //stops near me
            var stopsNearMe = SearchStopsNearAddress(lat, lon, 0.5, start);
            var stopsNearDest = SearchStopsNearAddress(dest.Lat, dest.Lon, 0.5, end);

            //var destRouteIds = new List<string> { dest.ParentId };
            var destRouteIds = stopsNearDest.Select(x => x.ParentId).Distinct().ToList();

            //now figure out what stops new me are common
            var myRouteIds = stopsNearMe.Select(x => x.ParentId).Distinct().ToList();

            var commonRouteIds = myRouteIds.Intersect(destRouteIds).ToList();
            var curDistance = 0.1;

            //if no common routes found; keep increasing the distance
            //while (commonRouteIds.Count == 0 && curDistance <= 1.0)
            //{
            //    curDistance = curDistance + 0.1;
            //    var nearest = stopsNearMe.Where(x => x.Distance <= curDistance).ToList();
            //    myRouteIds = nearest.Select(x => x.ParentId).Distinct().ToList();
            //    commonRouteIds = myRouteIds.Intersect(destRouteIds).ToList();
            //}

            var sharedStopsNearMe = stopsNearMe.Where(m => commonRouteIds.Contains(m.ParentId)).ToList();
            //var sharedStopsNearDest = stopsNearDest.Where(m => commonRouteIds.Contains(m.ParentId)).ToList();

            var ret = new List<Stop>();

            var finalDestination = new Destination();
            finalDestination.DestinationTitle = end;
            
            foreach (var routeId in commonRouteIds)
            {
                //select closest route from you and only use that route as start point
                var closestStopPerRoute = stopsNearMe.Where(x => x.ParentId == routeId).OrderBy(y => y.Distance).FirstOrDefault();
                if (closestStopPerRoute != null)
                {

                    var shared = FindCommonDestinations(closestStopPerRoute, stopsNearDest);
                    if (shared.StopsToNear.Count != 0)
                    {
                        shared.DestinationTitle = end;
                        finalDestination.PossibleRoutes.Add(shared);
                    }
                }
            }

            return finalDestination;
        }

        public void RefreshDatabase()
        {
            _db.DropTable<Agency>();
            _db.DropTable<Route>();
            _db.DropTable<Direction>();
            _db.DropTable<Stop>();
            _db.CreateTable<GeoPoint>();

            CreateTables();
        }


        public void SaveEntity<T>(T entity) where T : DbEntity
        {
            if (entity is Agency)
            {
                if (_db.Find<Agency>(entity.Id) == null)
                {
                    _db.Insert(entity);
                }
                else
                {
                    _db.Update(entity);
                }
            }

            if (entity is Route)
            {
                if (_db.Find<Route>(entity.Id) == null)
                {
                    _db.Insert(entity);
                }
                else
                {
                    _db.Update(entity);
                }
            }

            if (entity is Stop)
            {
                if (_db.Find<Stop>(entity.Id) == null)
                {
                    _db.Insert(entity);
                }
                else
                {
                    _db.Update(entity);
                }
            }

            if (entity is Direction)
            {
                if (_db.Find<Direction>(entity.Id) == null)
                {
                    _db.Insert(entity);
                }
                else
                {
                    _db.Update(entity);
                }
            }

            if (entity is GeoPoint)
            {
                if (_db.Find<GeoPoint>(entity.Id) == null)
                {
                    _db.Insert(entity);
                }
                else
                {
                    _db.Update(entity);
                }
            }

        }

        public Stop GetStopByTag(string tag)
        {
            var my = TrackingHelper.Instance.LastPosition;
            var stop = _db.Query<Stop>("Select * from Stop where Tag=?", tag).FirstOrDefault();
            if (stop != null)
            {
                stop.Distance = TrackingHelper.Instance.CalculateDistance(stop.Lat, stop.Lon, my.Latitude, my.Longitude);

            }
            return stop;

        }

        public List<Stop> GetFavoriteStops()
        {
            return _db.Query<Stop>("Select * from Stop where IsFavorite=?", true);
        }
        public Preference GetPreference()
        {
            var preference = _db.Find<Preference>("user");
            if (preference == null)
            {
                preference = new Preference
                {
                    AutoRefresh = true,
                    AlertInterval = 1,
                    Vibrate = true,
                    AlertMinsBefore = 3
                };
            }

            return preference;

        }


        public void SavePrerefence(Preference preference)
        {
            var saved = _db.Find<Preference>(preference.Id);
            if (saved == null)
            {
                _db.Insert(preference);
            }
            else
            {
                _db.Update(preference);
            }
        }

        public void SaveAgency(Agency agency)
        {
            var saved = _db.Find<Agency>(agency.Id);

            if (saved == null)
            {
                _db.Insert(agency);
            }
            foreach (var route in agency.Routes)
            {
                if (route.IsConfigured)
                    SaveRoute(route);
            }
        }

        public void SaveRoute(Route route)
        {
            var saved = _db.Find<Route>(route.Id);
            if (saved == null)
            {
                _db.Insert(route);
            }
            else
            {
                _db.Update(route);
            }

            foreach (var dir in route.Directions)
            {
                foreach (var stop in dir.Stops)
                {
                    SaveStop(stop);
                }
            }
        }

        public void SaveDirection(Direction dir)
        {
            var saved = _db.Find<Direction>(dir.Id);
            if (saved == null)
            {
                _db.Insert(dir);
            }

            foreach (var stop in dir.Stops)
            {
                SaveStop(stop);
            }
        }

        public void SaveStop(Stop stop)
        {
            var saved = _db.Find<Stop>(stop.Id);
            if (saved == null)
            {
                _db.Insert(stop);
            }
        }
        internal async Task<List<Route>> GetRoutesListAsync(Agency agency)
        {
            var routes = _db.Query<Route>("Select * from Route where ParentId=?", agency.Id).ToList();
            foreach (var route in routes)
            {
                await ConfigureRoute(route);
            }
            return routes;
        }

        //private void PopulateRoutePath(Route route)
        //{
        //    var path = _db.Query<Stop>("select * from stop where route")

        //    //if (!string.IsNullOrEmpty(route.PathData))
        //    //{
        //    //    var doc = XDoc.LoadXml(route.PathData);
        //    //    foreach (var pathNode in doc.Descendants().Where(x => x.Name == "point"))
        //    //    {
        //    //        route.Path.Add(
        //    //            new GeoPoint
        //    //            {
        //    //                ParentId = route.Tag,
        //    //                Lat = Convert.ToDouble(pathNode.GetAttribute("lat")),
        //    //                Lon = Convert.ToDouble(pathNode.GetAttribute("lon"))
        //    //            });
        //    //    }
        //    //}
        //}

        internal async Task<List<Agency>> GetAgencyListAsync()
        {
            return _db.Query<Agency>("Select * from Agency");
        }

        internal List<Route> GetUnconfiguredRoutes()
        {
            var sql = "select * from route  where isconfigured = ?";
            //unconfigured
            var items = _db.Query<Route>(sql, 0);

            return items;

        }


        internal Task<int> ConfigureRoute(Route route)
        {
            bool isConfigured = false;
            int stopsCount = 0;
            // load directions
            try
            {
                var dirs = _db.Query<Direction>("Select * from Direction where ParentId=?", route.Id);
                if (dirs.Count != 0)
                {

                    foreach (var dir in dirs)
                    {

                        var dirStops = _db.Query<Stop>("Select * from Stop where parentId=?", dir.Id);
                        dir.Stops.ReplaceRange(dirStops);
                        var routeStops = dirStops.Select(x => new GeoPoint { Lat = x.Lat, Lon = x.Lon });

                        //only consider it done if there are stops
                        isConfigured = true;
                        stopsCount++;
                    }

                }
                route.Stops.ReplaceRange(_db.Query<Stop>("Select * from Stop where RouteTag=?", route.Tag));

                route.Directions.ReplaceRange(dirs);

                route.IsConfigured = isConfigured;
            }
            catch (Exception e)
            {
                throw;
            }

            return Task.FromResult(stopsCount);
        }
    }
}
