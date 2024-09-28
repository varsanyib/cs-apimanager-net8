using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIManagerVB
{
    public enum AuthType
    {
        NONE = 0,
        OTHER = 1,
        BEARER = 2
    }
    public enum LogType
    {
        INFO = 0,
        WARNING = 1,
        ERROR = 2
    }
    public enum BodyType
    {
        NONE = 0,
        OTHER = 1,
        JSON = 2,
        URLENCODED = 3
    }
    public enum HttpMethod
    {
        GET = 0,
        POST = 1,
        PUT = 2,
        PATCH = 3,
        DELETE = 4,
    }
}
