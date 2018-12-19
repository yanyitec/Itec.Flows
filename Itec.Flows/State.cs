using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public class State : IState
    {
        Dictionary<string, string> _Data;
        State _Parent;
        public State() {
            _Data = new Dictionary<string, string>();
        }

        public bool Set(string key, string value) {
            if (_Parent != null && _Parent.Set(key, value)) return false;
            if (this._Data.ContainsKey(key)) this._Data[key] = value;
            else this._Data.Add(key, value);
            return true;
        }
        public string Get(string key) {
            string value = null;
            if (this._Data.TryGetValue(key, out value)) return value;
            if (this._Parent != null) value = this._Parent.Get(key);
            return value;
        }
        public string this[string key] {
            get {
                return this.Get(key);
            }
            set {
                this.Set(key,value);
            }
        }

        

        public ICollection<string> Keys => this._Data.Keys;

        public ICollection<string> Values => this._Data.Values;

        public int Count => this._Data.Count;

        public bool IsReadOnly => false;

        public void Add(string key, string value)
        {
            this._Data.Add(key,value);
        }

        public bool ContainsKey(string key)
        {
            return this._Data.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return this._Data.Remove(key);
        }

        public bool TryGetValue(string key, out string value)
        {
            return this._Data.TryGetValue(key, out value);
        }

        
        public void Clear()
        {
            this._Data.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return (this._Data as IDictionary<string, string>).Contains(item);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            (this._Data as IDictionary<string, string>).CopyTo(array,arrayIndex);
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return (this._Data as IDictionary<string, string>).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return this._Data.GetEnumerator();
        }

        

        public void Add(KeyValuePair<string, string> item)
        {
            (this._Data as IDictionary<string, string>).Add(item);
        }
    }
}
