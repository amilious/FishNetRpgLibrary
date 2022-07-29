using System;
using UnityEditor;

namespace Amilious.Core.Attributes {
    
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class EditorLinkAttribute : Attribute {
        
        public string ToolTip { get; }
        public string IconResourcePath { get; }
        public string Link { get; }
        public string LinkName { get; }

        public EditorLinkAttribute(string toolTip, string iconResourcePath, string link, 
            string linkName = null) {
            ToolTip = toolTip;
            IconResourcePath = iconResourcePath;
            Link = link;
            LinkName = linkName;
        }
        
    }
    
}