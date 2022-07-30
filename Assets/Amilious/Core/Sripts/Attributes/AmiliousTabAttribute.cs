using System;

namespace Amilious.Core.Attributes {
    
    /// <summary>
    /// This attribute is used to add an item to a tab group.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class AmiliousTabAttribute : Attribute {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the name of the tab group that the property should belong to.
        /// </summary>
        public string TabGroup { get; }
        
        /// <summary>
        /// This property contains the name of the tab that the property should belong to.
        /// </summary>
        public string TabName { get; }
        
        /// <summary>
        /// This property contains the order of the item within the tab.
        /// </summary>
        public int Order { get; }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This attribute is used to add an item to the default tab group.
        /// </summary>
        /// <param name="tabName">The name of the tab that the property belongs to.</param>
        /// <param name="order">The property order for this property on the tab.</param>
        /// <seealso cref="AmiliousTabAttribute(string,string,int)"/>
        public AmiliousTabAttribute(string tabName, int order = 0) {
            TabGroup = string.Empty;
            TabName = tabName ?? string.Empty;
            Order = order;
        }

        /// <summary>
        /// This attribute is used to add an item to the given tab group and tab.
        /// </summary>
        /// <param name="tabGroup">The name of the tab group that the property belongs to.</param>
        /// <param name="tabName">The name of the tab that the property belongs to.</param>
        /// <param name="order">The property order for this property on the tab.</param>
        /// <seealso cref="AmiliousTabAttribute(string,int)"/>
        public AmiliousTabAttribute(string tabGroup, string tabName, int order = 0) {
            TabGroup = tabGroup ?? string.Empty;
            TabName = tabName ?? string.Empty;
            Order = order;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}