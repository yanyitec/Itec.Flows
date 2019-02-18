using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    [AttributeUsage(AttributeTargets.Field| AttributeTargets.Property)]
    public class ExportAttribute:Attribute
    {
        
        public ExportAttribute(string name=null)
        {
            this.Name = name;
        }

       


        

        public string Name { get; private set; }
    }
}
