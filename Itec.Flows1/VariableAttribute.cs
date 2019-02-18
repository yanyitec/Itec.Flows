using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public class VariableAttribute
    {
        public VariableAttribute() {
            this.Scope = Scopes.Variables;
        }
        public VariableAttribute(string name,Scopes scope = Scopes.Variables) {
            this.Scope = scope;
            this.Name = name;
        }

        public VariableAttribute(Scopes scope,string name = null) {
            this.Scope = scope;
            this.Name = name;
        }

        
        public Scopes Scope { get; private set; }

        public bool Is(Scopes ck) {
            return ck == this.Scope || (((int)ck & (int)this.Scope) >0);
        }

        public string Name { get; private set; }
    }
}
