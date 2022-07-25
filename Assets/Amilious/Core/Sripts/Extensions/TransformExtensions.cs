using Amilious.Core.Serializables;
using UnityEngine;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extensions to the Transform class.
    /// </summary>
    public static class TransformExtensions {
        
        /// <summary>
        /// This method is used to get a direction from the given angle.
        /// </summary>
        /// <param name="trans">The transform you want to base the direction
        /// on.</param>
        /// <param name="angleInDegrees">The angle you want to get the direction for.</param>
        /// <returns>The direction of the angle when based on the transform.</returns>
        public static Vector3 DirectionFromAngle(this Transform trans, float angleInDegrees) {
            angleInDegrees += trans.eulerAngles.y;
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0,
                Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        /// <summary>
        /// The method is used to get the direction of a target from the current
        /// transform.
        /// </summary>
        /// <param name="trans">The current transform.</param>
        /// <param name="target">The target you want to get the direction of.</param>
        /// <returns>The direction of the target.</returns>
        public static Vector3 DirectionToTarget(this Transform trans, Transform target) {
            return (target.position - trans.position).normalized;
        }
        
        /// <summary>
        /// This method is used to convert a Transform into a SerializableTransform.
        /// </summary>
        /// <param name="transform">The Transform that you want to convert.</param>
        /// <returns>A Serializable version of the given Transform.</returns>
        public static SerializableTransform ToSerializable(this Transform transform) {
            return new SerializableTransform(transform);
        }

        /// <summary>
        /// This method is used to destroy all of the transforms children.
        /// </summary>
        /// <param name="transform">The transform you want to destroy the children of.</param>
        public static void DestroyChildren(this Transform transform) {
            foreach(Transform child in transform) Object.Destroy(child.gameObject);
        }

        /// <summary>
        /// This method is used to destroy all of the transforms children immediately.
        /// </summary>
        /// <param name="transform">The transform you want to destroy the children of.</param>
        public static void DestroyChildrenImmediate(this Transform transform) {
            foreach(Transform child in transform) Object.DestroyImmediate(child.gameObject);
        }
        
        /// <summary>
        /// This method is used to get the path of the given <see cref="transform"/>
        /// </summary>
        /// <param name="transform">The transform you want to get the path of.</param>
        /// <returns>The path of the given <see cref="transform"/>.</returns>
        public static string GetPath(this Transform transform) {
            if(transform.parent == null) return "/" + transform.name;
            return transform.parent.GetPath() + "/" + transform.name;
        }
        
    }
}