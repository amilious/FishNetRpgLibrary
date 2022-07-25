using UnityEngine;
using Amilious.Core.Serializables;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extensions to the Quaternion class.
    /// </summary>
    public static class QuaternionExtensions {
        
        /// <summary>
        /// This method is used to convert a Quaternion into a SerializableQuaternion.
        /// </summary>
        /// <param name="quaternion">The Quaternion that you want to convert.</param>
        /// <returns>A Serializable version of the given Quaternion.</returns>
        public static SerializableQuaternion ToSerializable(this Quaternion quaternion) {
            return new SerializableQuaternion(quaternion);
        }
        
    }
}