using Amilious.Core.Serializables;
using UnityEngine;

namespace Amilious.Core.Extensions {
     /// <summary>
    /// This class is used to add extensions to the Vector2 class.
    /// </summary>
    public static class Vector2Extensions {
        
        /// <summary>
        /// This method is used to convert a Vector2 into a SerializableVector2.
        /// </summary>
        /// <param name="vector2">The Vector2 that you want to convert.</param>
        /// <returns>A Serializable version of the given Vector2.</returns>
        public static SerializableVector2 ToSerializable(this Vector2 vector2) {
            return new SerializableVector2(vector2);
        }
        
        /// <summary>
        /// This method is used to get the values from the <see cref="Vector2Int"/>
        /// </summary>
        /// <param name="vector2Int">The <see cref="Vector2Int"/> that you want to get the values for.</param>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        public static void GetValues(this Vector2Int vector2Int, out int x, out int y) {
            x = vector2Int.x;
            y = vector2Int.y;
        }
        
    }
}