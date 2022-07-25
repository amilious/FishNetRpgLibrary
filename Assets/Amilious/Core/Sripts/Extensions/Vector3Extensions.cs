using UnityEngine;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extension methods to a Vector3.
    /// </summary>
    public static class Vector3Extensions {
        
        public enum AxisPos {Min, Zero, Max}

        /// <summary>
        /// This method is used to remove scaling from a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="value">The <see cref="Vector3"/></param>
        /// <param name="appliedScale">The scale that was applied.</param>
        /// <returns>The <see cref="Vector3"/> with the scale removed.</returns>
        public static Vector3 Unscale(this Vector3 value, Vector3 appliedScale) {
            return new Vector3(
                value.x / appliedScale.x,
                value.y / appliedScale.y,
                value.z / appliedScale.z);
        }

        /// <summary>
        /// This method is used to check if a Vector3 contains all integers.
        /// </summary>
        /// <param name="vector3">The Vector3 that you want to check.</param>
        /// <returns>True if x, y, and z have no fraction or decimal, otherwise false.</returns>
        public static bool IsIntegers(this Vector3 vector3) {
            return vector3.x.IsInteger() && vector3.y.IsInteger() && vector3.z.IsInteger();
        }

        /// <summary>
        /// This method is used to swap the x and y values of a Vector3.
        /// </summary>
        /// <param name="vector3">The Vector3 that you want to swap the values for.</param>
        /// <param name="swap">If true the values will be swapped.</param>
        /// <returns>The resulting Vector3.</returns>
        public static Vector3 SwapXY(this Vector3 vector3, bool swap = true) {
            return !swap ? vector3 : new Vector3(vector3.y, vector3.x, vector3.z);
        }
        
        /// <summary>
        /// This method is used to swap the x and z values of a Vector3.
        /// </summary>
        /// <param name="vector3">The Vector3 that you want to swap the values for.</param>
        /// <param name="swap">If true the values will be swapped.</param>
        /// <returns>The resulting Vector3.</returns>
        public static Vector3 SwapXZ(this Vector3 vector3, bool swap = true) {
            return !swap ? vector3 : new Vector3(vector3.z, vector3.y, vector3.x);
        }
        
        /// <summary>
        /// This method is used to swap the y and z values of a Vector3.
        /// </summary>
        /// <param name="vector3">The Vector3 that you want to swap the values for.</param>
        /// <param name="swap">If true the values will be swapped.</param>
        /// <returns>The resulting Vector3.</returns>
        public static Vector3 SwapYZ(this Vector3 vector3, bool swap = true) {
            return !swap ? vector3 : new Vector3(vector3.x, vector3.z, vector3.y);
        }
        
        /// <summary>
        /// This method is used to position an object withing another object.
        /// </summary>
        /// <param name="parentSize">The size of the parent object.</param>
        /// <param name="size">The size of the child object.</param>
        /// <param name="x">The x axis position type.</param>
        /// <param name="y">The y axis position type.</param>
        /// <param name="z">The z axis position type.</param>
        /// <param name="offset">An optional offset that can be applied after calculating
        /// the position.</param>
        /// <returns>The position of the child object.</returns>
        public static Vector3 PositionWithin(this Vector3 parentSize, Vector3 size, 
            AxisPos x, AxisPos y, AxisPos z, Vector3? offset=null) {
            var result = Vector3.zero;
            var parentHalf = parentSize / 2;
            var halfSize = size / 2f;
            //calculate the x
            result.x = x switch {
                AxisPos.Min => -parentHalf.x+halfSize.x,
                AxisPos.Zero => 0,
                AxisPos.Max => parentHalf.x-halfSize.x,
                _ => 0
            };
            //calculate the y
            result.y = y switch {
                AxisPos.Min => -parentHalf.y+halfSize.y,
                AxisPos.Zero => 0,
                AxisPos.Max => parentHalf.y-halfSize.y,
                _ => 0
            };
            //calculate the z
            result.z = z switch {
                AxisPos.Min => -parentHalf.z+halfSize.z,
                AxisPos.Zero => 0,
                AxisPos.Max => parentHalf.z-halfSize.z,
                _ => 0
            };
            if(offset.HasValue) result += offset.Value;
            return result;
        }

        /// <summary>
        /// This method is used to get the values from the <see cref="Vector3"/>
        /// </summary>
        /// <param name="vector3">The <see cref="Vector3"/> that you want to get the values from.</param>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <param name="z">The z value.</param>
        public static void GetValues(this Vector3 vector3, out float x, out float y, out float z) {
            x = vector3.x;
            y = vector3.y;
            z = vector3.z;
        }

        /// <summary>
        /// This method is used to get the x and y values from the <see cref="Vector3"/>
        /// </summary>
        /// <param name="vector3">The <see cref="Vector3"/> that you want to get the values from.</param>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        public static void GetXYValues(this Vector3 vector3, out float x, out float y) {
            x = vector3.x;
            y = vector3.y;
        }

        /// <summary>
        /// This method is used to get the x and z values from the <see cref="Vector3"/>
        /// </summary>
        /// <param name="vector3">The <see cref="Vector3"/> that you want to get the values from.</param>
        /// <param name="x">The x value.</param>
        /// <param name="z">The z value.</param>
        public static void GetXZValues(this Vector3 vector3, out float x, out float z) {
            x = vector3.x;
            z = vector3.z;
        }

        /// <summary>
        /// This method is used to get the y and z values from the <see cref="Vector3"/>
        /// </summary>
        /// <param name="vector3">The <see cref="Vector3"/> that you want to get the values from.</param>
        /// <param name="y">The y value.</param>
        /// <param name="z">The z value.</param>
        public static void GetYZValues(this Vector3 vector3, out float y, out float z) {
            y = vector3.y;
            z = vector3.z;
        }

        /// <summary>
        /// This method is used to get a <see cref="Vector2"/> containing the x position as x and
        /// the z position as y.
        /// </summary>
        /// <param name="vector3">The <see cref="Vector3"/> that you want to get the x and z positions form.</param>
        /// <returns>The vector's position as x = x and y = z.</returns>
        public static Vector2 GetXZ(this Vector3 vector3) {
            return new Vector2(vector3.x, vector3.z);
        }

        /// <summary>
        /// This method is used to get a <see cref="Vector2"/> containing the y position as x and the
        /// z position as y.
        /// </summary>
        /// <param name="vector3">The <see cref="Vector3"/> that you want to get the y and z positions from.</param>
        /// <returns>The vector's position as x = y and y = z.</returns>
        public static Vector2 GetYZ(this Vector3 vector3) {
            return new Vector2(vector3.y, vector3.z);
        }
        
    }
}