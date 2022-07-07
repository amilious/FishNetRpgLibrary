using UnityEngine;

namespace Amilious.Core.Extensions {
    public static class Vector3Extensions {

        public static Vector3 Unscale(this Vector3 value, Vector3 appliedScale) {
            return new Vector3(
                value.x / appliedScale.x,
                value.y / appliedScale.y,
                value.z / appliedScale.z);
        }

    }
}