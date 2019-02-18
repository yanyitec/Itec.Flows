using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Itec.Flows
{
    public class CascadeReadOnlyState:ReadOnlyState, ICascadeReadOnlyState
    {
        public CascadeReadOnlyState(CascadeReadOnlyState super) : base()
        {
            this.Super = super;
        }
        
        public CascadeReadOnlyState(JObject data = null, CascadeReadOnlyState super=null) : base(data)
        {
            this.Super = super;
        }

        public CascadeReadOnlyState(string json, CascadeReadOnlyState super=null) : base(json)
        {
            this.Super = super;
        }

        public ICascadeReadOnlyState Super { get; private set; }

        public T Overall<T>(string key)
        {
            var token = Internals[key];
            if (token == null)
            {
                return (this.Super != null) ? this.Super.Overall<T>(key) : default(T);
            }
            if (token.Type == JTokenType.Undefined || token.Type == JTokenType.Null) return default(T);
            return token.ToObject<T>();
        }

        public object Overall(string key, Type objectType)
        {
            var token = Internals[key];
            if (token == null)
            {
                return (this.Super != null) ? this.Super.Overall(key,objectType) : null;
            }
            if (token.Type == JTokenType.Undefined || token.Type == JTokenType.Null) return null;
            return token.ToObject(objectType);
        }

        public string OverallString(string key)
        {
            var token = Internals[key];
            if (token == null ) {
                return (this.Super != null) ? this.Super.OverallString(key):null;
            }
            if (token.Type == JTokenType.Undefined || token.Type == JTokenType.Null) return null;
            return token.ToString();
        }

        public JObject MergeTo(JObject target=null) {
            if (target == null)
            {
                target = (this.Internals.DeepClone() as JObject) as JObject;
            }
            else {
                foreach (var pair in this.Internals) {
                    if (!target.ContainsKey(pair.Key)) target.Add(pair.Key,pair.Value);
                }
            }
            if (this.Super != null) (this.Super as CascadeReadOnlyState).MergeTo(target);
            return target;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this.Overall(binder.Name,binder.ReturnType);
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            var index = indexes[0];
            if (index == null) { result = null; return false; }
            result = this.Overall(index.ToString(), binder.ReturnType);
            return true;
        }
    }
}
