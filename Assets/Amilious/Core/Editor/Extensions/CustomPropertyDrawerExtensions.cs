using System;
using UnityEditor;
using System.Reflection;

namespace Amilious.Core.Editor.Extensions {
    
    /// <summary>
    /// This class is used to add extension methods to the <see cref="CustomPropertyDrawer"/> class.
    /// </summary>
    public static class CustomPropertyDrawerExtensions {
        
        /// <summary>
        /// This method is used to get the internal type field of a custom property drawer.
        /// </summary>
        /// <param name="drawer">The drawer attribute that you want to get the drawer for.</param>
        /// <param name="type">The type that the drawer is for.</param>
        /// <returns>True if able to get the type, otherwise false.</returns>
        public static bool TryGetDrawersPropertyType(this CustomPropertyDrawer drawer, out Type type) {
            type = default;
            if(drawer == null) return false;
            var typeField = drawer.GetType().GetField("m_Type",
                BindingFlags.NonPublic | BindingFlags.Instance);
            if(typeField == null) return false;
            type = (Type)typeField.GetValue(drawer);
            return true;
        }
        
    }
}