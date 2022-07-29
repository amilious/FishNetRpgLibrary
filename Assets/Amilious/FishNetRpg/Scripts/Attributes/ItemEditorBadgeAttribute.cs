using System;

namespace Amilious.FishNetRpg.Attributes {
    
    [AttributeUsage(AttributeTargets.Class)]
    //JetBrains.Annotations
    public class ItemEditorBadgeAttribute : Attribute {
        
        public string IconResourcePath { get; }

        public ItemEditorBadgeAttribute(string iconResourcePath) {
            IconResourcePath = iconResourcePath;
        }
        
    }
}