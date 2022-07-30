using UnityEngine;

namespace Amilious.Core.Editor {
    
    /// <summary>
    /// This struct is used to hold information on a link.
    /// </summary>
    public struct LinkInfo {
        
        /// <summary>
        /// This property contains the links gui content.
        /// </summary>
        public GUIContent GUIContent { get; set; }
        
        /// <summary>
        /// This property contains the link.
        /// </summary>
        public string Link { get; set; }
        
    }
}