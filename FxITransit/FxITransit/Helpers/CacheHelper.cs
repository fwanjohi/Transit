using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxITransit.Helpers
{
    public class CacheHelper
    {
        private static readonly Lazy<CacheHelper> instance = new Lazy<CacheHelper>(() => new CacheHelper());

        public static CacheHelper Instance { get { return instance.Value; } }

        public string RouteConfig { get; set; }

    }
}
