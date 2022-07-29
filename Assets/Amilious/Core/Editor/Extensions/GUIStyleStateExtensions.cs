using Amilious.Core.Extensions;
using UnityEngine;

namespace Amilious.Core.Editor.Extensions {
    
    public static class GUIStyleStateExtensions {
        
        public static void SetBackGroundColor(this GUIStyleState state, Color color) {
            state.background = color.MakeTexture(2, 2);
        }

        public static void SetBackgroundColor(this GUIStyleState state, string htmlCode) {
            if(!htmlCode.StartsWith("#")) htmlCode = $"#{htmlCode}";
            ColorUtility.TryParseHtmlString(htmlCode, out var color);
            state.SetBackGroundColor(color);
        }
        
    }
    
}