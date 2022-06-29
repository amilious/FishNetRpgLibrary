using System;

namespace FishNetRpgLibrary.Statistics {
    
    [AttributeUsage(AttributeTargets.Field)]
    public class StatAttribute : Attribute {

        public string Name { get; }
        
        public StatAttribute(string name) {
            Name = name;
        }
        
    }
}