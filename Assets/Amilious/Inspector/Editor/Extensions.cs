using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Amilious.Inspector.Editor {
    
    public static class Extensions {
        
        /// <summary>
        /// This method is used to get the internal type field of a custom property drawer.
        /// </summary>
        /// <param name="drawer">The drawer attribute that you want to get the drawer for.</param>
        /// <param name="type">The type that the drawer is for.</param>
        /// <returns>True if able to get the type, otherwise false.</returns>
        public static bool TryGetDrawerForType(this CustomPropertyDrawer drawer, out Type type) {
            type = default;
            if(drawer == null) return false;
            var typeField = drawer.GetType().GetField("m_Type",
                BindingFlags.NonPublic | BindingFlags.Instance);
            if(typeField == null) return false;
            type = (Type)typeField.GetValue(drawer);
            return true;
        }
        
        public static void SetBackGroundColor(this GUIStyleState state, Color color) {
            state.background = MakeTexture(2, 2, color);
        }

        
        public static void SetBackgroundColor(GUIStyleState state, string htmlCode) {
            if(!htmlCode.StartsWith("#")) htmlCode = $"#{htmlCode}";
            ColorUtility.TryParseHtmlString(htmlCode, out var color);
            state.SetBackGroundColor(color);
        }
        
        private static Texture2D MakeTexture( int width, int height, Color color ) {
            var pix = new Color[width * height];
            for(var i = 0; i < pix.Length; ++i )pix[ i ] = color;
            var result = new Texture2D( width, height );
            result.SetPixels( pix );
            result.Apply();
            return result;
        }
        
    }
}