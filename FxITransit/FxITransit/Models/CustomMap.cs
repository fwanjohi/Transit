
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Maps;

namespace FxITransit.Models

{
    public class CustomMap : Map
    {
        public List<Position> RouteCoordinates { get; set; }

        public CustomMap()
        {
            RouteCoordinates = new List<Position>();
        }

        public void DrawPath(IEnumerable<GeoPoint> path, IEnumerable<Pin> pins = null, GeoPoint center = null, double distance = 1.0, bool clear = true)
        {
            if (clear) RouteCoordinates.Clear();
            var cor = path.Select(x => new Position(x.Lat, x.Lon));

            RouteCoordinates.AddRange(cor);

            Pins.Clear();

            if (pins != null)
            {
                
                foreach (var p in pins.Where(x=> x != null))
                {
                    Pins.Add(p);
                }
            }

            if (center != null)
            {
                MoveToRegion(MapSpan.FromCenterAndRadius(center, Distance.FromMiles(distance)));
            }
        }

        public void DrawPath(IEnumerable<GeoPoint> path, Pin pin = null , GeoPoint center = null, double distance = 1.0, bool clear = true)
        {
            DrawPath(path, new List<Pin> { pin }, center, distance);
        }
    }
}

