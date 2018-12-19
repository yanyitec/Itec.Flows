using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public class ActivityDefination
    {
        public class Route {
            public string Name { get; set; }
            public string Key { get; set; }

            public string Value { get; set; }
        }
        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ActivityType { get; set; }

        public string Code { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        public List<Route> Routes { get; set; }

        public List<ActivityDefination> Activities { get; set; }

    }
}
