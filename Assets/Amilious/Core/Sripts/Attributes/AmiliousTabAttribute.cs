using System;

namespace Amilious.Core.Attributes {
    
    [AttributeUsage((AttributeTargets.Field), Inherited = true)]
    public class AmiliousTabAttribute : Attribute {
        
        public string TabName { get; }
        public int Priority { get; }

        public AmiliousTabAttribute(string tabName, int priority = 0) {
            TabName = tabName;
            Priority = priority;
        }
        
    }
}