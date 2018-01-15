using System;
using System.Reflection.Emit;
using System.Collections.Generic;


namespace PascalDotNet {
    public class VariableScope {
        private Dictionary<string, LocalBuilder> Locals;
        public VariableScope ParentScope;

        public void Add(string name, LocalBuilder local) {
            Locals[name] = local;
        }

        public Boolean Exists(string name) {

            try {
                LocalBuilder lb = Locals[name];
                return true;
            }
            catch {
                return false;
            }
        }

        public LocalBuilder LookUp(string name) {
            try {
                return Locals[name];
            }
            catch {
                return ParentScope.LookUp(name);
            }
        }

        public VariableScope() {
            Locals = new Dictionary<string, LocalBuilder>();
        }

        public VariableScope(VariableScope parent) {
            Locals = new Dictionary<string, LocalBuilder>();
            ParentScope = parent;
        }
    }


}