using UnityEngine;

namespace Amilious.Core.Serializables {
    
    /// <summary>
    /// This class is used to serialize a Vector3Int's values.
    /// </summary>
    [System.Serializable]
    public class SerializableVector3Int {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to hold the vector3Int's x value.
        /// </summary>
        private readonly int _x;
        
        /// <summary>
        /// This field is used to hold the vector3Int's y value.
        /// </summary>
        private readonly int _y;
        
        /// <summary>
        /// This field is used to hold the vector3Int's z value.
        /// </summary>
        private readonly int _z;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get a Vector3Int form this SerializedVector3Int.
        /// </summary>
        public Vector3Int Vector3Int => new Vector3Int(_x, _y, _z);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a SerializableVector3Int from
        /// the given Vector3Int.
        /// </summary>
        /// <param name="vector3">The Vector3Int that you want to make serializable.</param>
        public SerializableVector3Int(Vector3Int vector3) {
            _x = vector3.x;
            _y = vector3.y;
            _z = vector3.z;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Operator Methods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This operator is used to auto cast a serializable Vector3Int to a Vector3Int.
        /// </summary>
        /// <param name="sVector3Int">The serializable Vector3Int.</param>
        /// <returns>A Vector3Int.</returns>
        public static explicit operator Vector3Int(SerializableVector3Int sVector3Int) => sVector3Int.Vector3Int;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
    }
}