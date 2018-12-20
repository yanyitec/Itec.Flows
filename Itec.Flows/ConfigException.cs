using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public class ConfigException:Exception
    {
        public ConfigException(string message,string path,string flowId,Config def) : base(message) {
            this.NodePath = path;
            this.FlowId = flowId;
            this.Defination = def;
        }
        public string NodePath { get; private set; }

        public string FlowId { get; private set; }

        public Config Defination { get; private set; }

    }
}
