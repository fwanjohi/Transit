using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxITransit.Models
{
    public class ServiceError
    {
        /*
         * <body>
<Error shouldRetry="true">
Agency server cannot accept client while status is: agency
name = sf-muni,status = UNINITIALIZED, client count = 0, last
even t = 0 seconds ago Could not get route list for agency tag "sf-muni".
Either the route tag is bad or the system is initializing.
</Error>
</body>
         */
        public string ErrorLevel { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsReTry { get; set; }
    }
}
