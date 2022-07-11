using UnityEngine;

namespace Amilious.Core.Serializables {
    
    /// <summary>
    /// This class is used to serialize a Vector2's values.
    /// </summary>
    [System.Serializable]
    public class SerializableVector2 {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to hold the vector2's x value.
        /// </summary>
        private readonly float _x;
        
        /// <summary>
        /// This field is used to hold the vector2's y value.
        /// </summary>
        private readonly float _y;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get a Vector2 form this SerializedVector2.
        /// </summary>
        public Vector2 Vector2 => new Vector2(_x, _y);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a SerializableVector2 from
        /// the given Vector2.
        /// </summary>
        /// <param name="vector2">The Vector2 that you want to make serializable.</param>
        public SerializableVector2(Vector2 vector2) {
            _x = vector2.x;
            _y = vector2.y;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Operator Methods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This operator is used to auto cast a serializable Vector2 to a Vector2.
        /// </summary>
        /// <param name="sVector2">The serializable Vector2.</param>
        /// <returns>A Vector2.</returns>
        public static explicit operator Vector2(SerializableVector2 sVector2) => sVector2.Vector2;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}