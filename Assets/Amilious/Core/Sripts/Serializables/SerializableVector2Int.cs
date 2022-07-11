using UnityEngine;

namespace Amilious.Core.Serializables {
    
    /// <summary>
    /// This class is used to serialize a Vector2Int's values.
    /// </summary>
    [System.Serializable]
    public class SerializableVector2Int {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to hold the vector2Int's x value.
        /// </summary>
        private readonly int _x;
        
        /// <summary>
        /// This field is used to hold the vector2Int's y value.
        /// </summary>
        private readonly int _y;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get a Vector2Int form this SerializedVector2Int.
        /// </summary>
        public Vector2Int Vector2Int => new Vector2Int(_x, _y);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a SerializableVector2Int from
        /// the given Vector2Int.
        /// </summary>
        /// <param name="vector2Int">The Vector2Int that you want to make serializable.</param>
        public SerializableVector2Int(Vector2Int vector2Int) {
            _x = vector2Int.x;
            _y = vector2Int.y;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Operator Methods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This operator is used to auto cast a serializable Vector2Int to a Vector2Int.
        /// </summary>
        /// <param name="sVector2Int">The serializable Vector2Int.</param>
        /// <returns>A Vector2Int.</returns>
        public static explicit operator Vector2Int(SerializableVector2Int sVector2Int) => sVector2Int.Vector2Int;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}