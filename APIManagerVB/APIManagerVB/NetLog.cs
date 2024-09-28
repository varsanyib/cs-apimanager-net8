using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIManagerVB
{
    public class NetLog
    {
        public DateTime Time { get; private set; }
        public LogType Type { get; private set; }
        public string Title { get; private set; }
        public string Details { get; private set; } = string.Empty;
        public NetLog(LogType type, string title)
        {
            Type = type;
            Title = title;
            Time = DateTime.Now;
        }
        public NetLog(LogType type, string title, string details) : this(type, title)
        {
            Details = details;
        }
        public NetLog(LogType type, string title, string details, DateTime time) : this(type, title, details)
        {
            Time = time;
        }
    }
}
