using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using FxITransit.Models;
using PCLStorage;
using SQLite;
using Xamarin.Forms;
using XLabs;


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

            _db.CreateTable<Agency>();
            _db.CreateTable<Route>();
            _db.CreateTable<Direction>();
            _db.CreateTable<Stop>();

            _db.CreateTable<Preference>();

        }

        public List<StopLite> SearchStopsNearAddress(double lat, double lon, double distanceInMiles, string from)
        {
            TableMapping m = new TableMapping(typeof(StopLite));

            //var stops = _db.Query<Stop>("select * from Stop").ToList()
            var lites = _db.Query<StopLite>("select distinct Tag, Title, Lat, Lon, AgencyTitle from Stop").ToList()
            .Where(
                k => TrackingHelper.Instance.CalculateDistance(
                         k.Lat, k.Lon, lat, lon) <= distanceInMiles).ToList();

            foreach (var x in lites)
            {
                x.Distance = TrackingHelper.Instance.CalculateDistance(x.Lat, x.Lon, lat, lon);
                var dist = x.Distance.ToString("0.##0");
                x.DistanceDisplay = $"{x.AgencyTitle} - {dist} from {from}";

            }
            lites = lites.OrderBy(x => x.Distance).ToList();
            return lites;
        }

        

        //get you a list of stops that 
        public List<StopLite> SearchStopsNearMeToADestination(StopLite dest)
        {
            var my = TrackingHelper.Instance.LastPosition;
            var ret = new List<StopLite>();
            ////select distinct bus routes that pass there

            List<IdItem> routesDest = _db.Query<IdItem>("select distinct ParentId as Id from Stop where lat=? and Lon =?", dest.Lat, dest.Lon).ToList();

            var stopsNearMe = SearchStopsNearAddress(my.Latitude, my.Longitude, 0.5, "Here");

            //uniq dests;
            var destRoutes = routesDest.Select(x => x.Id).Distinct().ToList();
            var realStops = _db.Query<Stop>("select * from stop").Where(x => destRoutes.Contains(x.ParentId)).ToList();

            foreach (var stop in stopsNearMe)
            {
                var stopNearMe = realStops.FirstOrDefault(r => r.Lat == stop.Lat && r.Lon == stop.Lon);

                if (stopNearMe != null)
                {
                    stopNearMe.Distance = TrackingHelper.Instance.CalculateDistance(stopNearMe.Lat, stopNearMe.Lon, my.Latitude, my.Longitude);
                    ret.Add(stopNearMe);
                }
            }

            foreach (var lite in ret)
            {
                lite.IsStart = true;
            }
            
            //var destNearMe = stopsNearMe

            return ret;
        }

        public void RefreshDatabase()
        {
            _db.DropTable<Agency>();
            _db.DropTable<Route>();
            _db.DropTable<Direction>();
            _db.DropTable<Stop>();

            _db.CreateTable<Agency>();
            _db.CreateTable<Route>();
            _db.CreateTable<Direction>();
            _db.CreateTable<Stop>();
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

        }

        public Stop GetStopByTag(string tag)
        {
            var my = TrackingHelper.Instance.LastPosition;
            var stop= _db.Query<Stop>("Select * from Stop where Tag=?", tag).FirstOrDefault();
           if (stop!=null)
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
            return _db.Query<Route>("Select * from Route where ParentId=?", agency.Id);
        }

        internal async Task<List<Agency>> GetAgencyListAsync()
        {
            return _db.Query<Agency>("Select * from Agency");
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

                        //only consider it done if there are stops
                        isConfigured = true;
                        stopsCount++;
                    }
                }
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
