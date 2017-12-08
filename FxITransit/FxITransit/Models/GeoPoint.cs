﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FxITransit.Models
{
    public class GeoPoint : DbEntity
    {
        public double Lat { get; set; }
        public double Lon { get; set; }


        public static implicit operator Point(GeoPoint gp)
        {
            return new Point { X = gp.Lat, Y = gp.Lon };
        }

        public static implicit operator Stop(GeoPoint gp)
        {
            return new Stop { Lat = gp.Lat, Lon = gp.Lon };
        }

        
    }

    
}
