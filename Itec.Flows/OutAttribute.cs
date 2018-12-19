using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class OutAttribute : Attribute
    {
        public OutAttribute()
        {
            this.Location = Locations.Parent;
        }
        public OutAttribute(string name, Locations location = Locations.Parent)
        {
            this.Location = location;
            this.Name = name;
        }

        public OutAttribute(Locations location, string name = null)
        {
            this.Location = location;
            this.Name = name;
        }


        public Locations Location { get; private set; }

        public bool Is(Locations ck)
        {
            return ck == this.Location || (((int)ck & (int)this.Location) > 0);
        }

        public string Name { get; private set; }
    }
}
