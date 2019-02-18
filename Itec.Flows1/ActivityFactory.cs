using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public class ActivityFactory
    {
        Func<ActivityState, IActivity> CreateInstance { get;  }

        Action<ActivityState, IActivity> ImportStates { get; }

        Action<ActivityState, IActivity> ExportStates { get; }
    }
}
