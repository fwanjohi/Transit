using FxITransit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxITransit.Helpers
{
    public class SettingsHelper : ObservableObject
    {
        private static readonly Lazy<SettingsHelper> instance = new Lazy<SettingsHelper>(() => new SettingsHelper());

        public static SettingsHelper Instance { get { return instance.Value; } }


        public SettingsHelper()
        {
            Alerts = new AlertSetttings();
            Favorites = new FavoriteSettings();
        }

        public AlertSetttings Alerts { get; set; }

        public FavoriteSettings Favorites { get; set; }
    }
    }
