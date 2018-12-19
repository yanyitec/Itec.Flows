﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public class DefinationException:Exception
    {
        public DefinationException(string message,string path,string flowId,ActivityDefination def) : base(message) {
            this.NodePath = path;
            this.FlowId = flowId;
            this.Defination = def;
        }
        public string NodePath { get; private set; }

        public string FlowId { get; private set; }

        public ActivityDefination Defination { get; private set; }

    }
}
