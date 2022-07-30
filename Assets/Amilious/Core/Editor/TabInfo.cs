using Amilious.Core.Attributes;
using UnityEditor;

namespace Amilious.Core.Editor {
    
    /// <summary>
    /// This struct is used to hold information about a tab item.
    /// </summary>
    public struct TabInfo {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the name of the tab group.
        /// </summary>
        public string TabGroup { get; set; }
        
        /// <summary>
        /// This property contains the name of the tab.
        /// </summary>
        public string TabName { get; set; }
        
        /// <summary>
        /// This property contains the serialized property that should be added to the tab.
        /// </summary>
        public SerializedProperty Property { get; set; }
        
        /// <summary>
        /// This property contains the draw order for the property on the tab.
        /// </summary>
        public int Order { get; set; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructor ////////////////////////////////////////////////////////////////////////////////////////////

        public TabInfo(AmiliousTabAttribute attribute, SerializedProperty property) {
            TabGroup = attribute.TabGroup;
            TabName = attribute.TabName;
            Order = attribute.Order;
            Property = property;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}