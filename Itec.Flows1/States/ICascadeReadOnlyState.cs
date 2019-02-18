using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public interface ICascadeReadOnlyState:IReadOnlyState
    {
        ICascadeReadOnlyState Super { get; }
        T Overall<T>(string key);
        string OverallString(string key);
        object Overall(string key, Type objectType);

        
    }
}
