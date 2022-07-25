using Amilious.Core.Serializables;
using UnityEngine;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extensions to the Vector2Int class.
    /// </summary>
    public static class Vector2IntExtensions {
        /// <summary>
        /// This method is used to convert a Vector2Int into a SerializableVector2Int.
        /// </summary>
        /// <param name="vector2Int">The Vector2 that you want to convert.</param>
        /// <returns>A Serializable version of the given Vector2Int.</returns>
        public static SerializableVector2Int ToSerializable(this Vector2Int vector2Int) {
            return new SerializableVector2Int(vector2Int);
        }
        
        /// <summary>
        /// This method is used to get the values from the <see cref="Vector2"/>
        /// </summary>
        /// <param name="vector2">The <see cref="Vector2"/> that you want to get the values for.</param>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        public static void GetValues(this Vector2 vector2, out float x, out float y) {
            x = vector2.x;
            y = vector2.y;
        }
    }
}