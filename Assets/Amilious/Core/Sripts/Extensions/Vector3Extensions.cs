using UnityEngine;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extension methods to a Vector3.
    /// </summary>
    public static class Vector3Extensions {

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

    }
}