using UnityEngine;

namespace Amilious.Core.Extensions {
    
    public static class ColorExtensions {

        public static string HtmlRGBA(this Color color) => ColorUtility.ToHtmlStringRGBA(color);

        public static string HtmlRGB(this Color color) => ColorUtility.ToHtmlStringRGB(color);

    }
    
}