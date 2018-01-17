using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxITransit.Messaging
{
    public class StartLoadingRoutesTaskMessage { }

    public class StopLoadingRoutesTaskMessage { }

    public class LoadRouteProgressMessage
    {
        public string Message { get; set; }
    }
}
