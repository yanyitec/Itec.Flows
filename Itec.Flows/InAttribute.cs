using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    [AttributeUsage(AttributeTargets.Field| AttributeTargets.Property)]
    public class InAttribute:Attribute
    {
        public InAttribute()
        {
            this.Location = Locations.Local;
        }
        public InAttribute(string name, Locations location = Locations.Local)
        {
            this.Location = location;
            this.Name = name;
        }

        public InAttribute(Locations location, string name = null)
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
