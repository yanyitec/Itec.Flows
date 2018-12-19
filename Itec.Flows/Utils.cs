using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public static class Utils
    {
        public static Func<object,string> ToJson;
        public static Func<string,Type, object> FromJson;
    }
}
