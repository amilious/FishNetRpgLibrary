using UnityEngine;

namespace Amilious.Core.Serializables {
    
    /// <summary>
    /// This class is used to serialize a Vector3's values.
    /// </summary>
    [System.Serializable]
    public class SerializableVector3 {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to hold the vector3's x value.
        /// </summary>
        private readonly float _x;
        
        /// <summary>
        /// This field is used to hold the vector3's y value.
        /// </summary>
        private readonly float _y;
        
        /// <summary>
        /// This field is used to hold the vector3'sz value.
        /// </summary>
        private readonly float _z;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get a Vector3 form this SerializedVector3.
        /// </summary>
        public Vector3 Vector3 => new Vector3(_x, _y, _z);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a SerializableVector3 from
        /// the given Vector3.
        /// </summary>
        /// <param name="vector3">The Vector3 that you want to make serializable.</param>
        public SerializableVector3(Vector3 vector3) {
            _x = vector3.x;
            _y = vector3.y;
            _z = vector3.z;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Operator Methods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This operator is used to auto cast a serializable Vector3 to a Vector3.
        /// </summary>
        /// <param name="sVector3">The serializable Vector3.</param>
        /// <returns>A Vector3.</returns>
        public static explicit operator Vector3(SerializableVector3 sVector3) => sVector3.Vector3;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}