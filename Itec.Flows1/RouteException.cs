using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public class RouteException:Exception
    {
        public RouteException(string message,string routeValue,ActivityState state, JToken nexts):base(message) {
            this.ActivityState = state;
            this.Nexts = nexts;
            this.RouteValue = routeValue;
        }
        public ActivityState ActivityState { get; private set; }
        public JToken Nexts { get; private set; }

        public string RouteValue { get; private set; }
    }
}
