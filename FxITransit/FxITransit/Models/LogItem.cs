using FxITransit.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxITransit.Models
{
    public class LogItem : ObservableObject
    {
        public LogItem()
        {
            LogDate = DateTime.Now;
        }

        public LogItem(Exception ex)
        {
            LogDate = DateTime.Now;
            Message = ex.Message;
            Stack = ex.StackTrace;
        }
        public DateTime LogDate { get; set; }
        public string Message { get; set; }
        public string Stack { get; set; }
    }
}
