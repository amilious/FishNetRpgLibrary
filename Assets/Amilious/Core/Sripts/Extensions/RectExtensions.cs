using UnityEngine;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extension methods to a rect
    /// </summary>
    public static class RectExtensions {
        
        /// <summary>
        /// This method is used to get a <see cref="Vector2"/> from the <see cref="Rect"/>'s min values.
        /// </summary>
        /// <param name="rect">The <see cref="Rect"/> of which the minimum position is needed.</param>
        /// <returns>The minimum position.</returns>
        public static Vector2 MinPosition(this Rect rect) => new Vector2(rect.xMin, rect.yMin);

        
        /// <summary>
        /// This method is used to get a <see cref="Vector2"/> from the <see cref="Rect"/>'s max values.
        /// </summary>
        /// <param name="rect">The <see cref="Rect"/> of which the maximum position is needed.</param>
        /// <returns>The maximum position.</returns>
        public static Vector2 MaxPosition(this Rect rect) => new Vector2(rect.xMax, rect.yMax);


    }
}