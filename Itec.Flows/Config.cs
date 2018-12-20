using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public class Config
    {
        
        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ActivityType { get; set; }

        public string Code { get; set; }

        public JObject Parameters { get; set; }

        public List<string> Exports { get; set; }

        public JToken Nexts { get; set; }

        public List<Config> Activities { get; set; }

    }
}
