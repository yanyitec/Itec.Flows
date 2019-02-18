﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public interface IReadOnlyState:System.Dynamic.IDynamicMetaObjectProvider,IEnumerable<KeyValuePair<string,object>>
    {
        bool HasChanges { get; }
        
        string this[string key] { get;  }

        T Get<T>(string key);
        string GetString(string key);
        object Get(string key, Type objectType);

        string ToJSON();
    }
}
