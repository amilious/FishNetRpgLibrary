using System;

namespace Amilious.FunctionGraph.Attributes {
    
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class FunctionNodeAttribute : Attribute {

        public string Description { get; }
        
        public bool Hidden { get; }
        
        public FunctionNodeAttribute(string description = null, bool hidden = false) {
            Description = description;
            Hidden = hidden;
        }
        
    }
}