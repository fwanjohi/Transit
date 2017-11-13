using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FxITransit.Models;
using PCLStorage;
using SQLite;



namespace FxITransit.Helpers
{
    public class DbHelper
    {
        private SQLiteConnection _db;

        public SQLiteConnection Db
        {
            get => _db;
        }

        public DbHelper()
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
        }

        public void RefreshDatabase()
        {
            _db.DropTable<Agency>();
            _db.DropTable<Route>();
            _db.DropTable<Direction>();
            _db.DropTable<Stop>();
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

        internal async Task<List<Agency>>  GetAgencyListAsync()
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
