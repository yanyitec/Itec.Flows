using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public interface IActivityState
    {
        string this[string key] { get;set; }
    }
}
