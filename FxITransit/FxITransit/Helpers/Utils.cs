using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxITransit.Helpers
{
    public static class Utils
    {
        public static DateTime? ConvertUnixTimeStamp(string unixTimeStamp)
        {
            var UTC = new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(Convert.ToDouble(unixTimeStamp));
            var date = UTC.ToLocalTime();
            return date;
        }
    }
}
