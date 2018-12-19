using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public class VariableAttribute
    {
        public VariableAttribute() {
            this.Kind = Scopes.Internal;
        }
        public VariableAttribute(string name,Scopes kind = Scopes.Internal) {
            this.Kind = kind;
            this.Name = name;
        }

        public VariableAttribute(Scopes kind,string name = null) {
            this.Kind = kind;
            this.Name = name;
        }

        
        public Scopes Kind { get; private set; }

        public bool Is(Scopes ck) {
            return ck == this.Kind || (((int)ck & (int)this.Kind)>0);
        }

        public string Name { get; private set; }
    }
}
