using UnityEngine;

namespace Amilious.Core.Serializables {
    
    /// <summary>
    /// This class is used to serialize a Transform's values.
    /// </summary>
    [System.Serializable]
    public class SerializableTransform {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to hold the transform's position.
        /// </summary>
        private SerializableVector3 _position;
        
        /// <summary>
        /// This field is used to hold the transform's rotation.
        /// </summary>
        private SerializableQuaternion _rotation;
        
        /// <summary>
        /// This field is used to hold the transform's scale.
        /// </summary>
        private SerializableVector3 _scale;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get the position form this SerializedTransform.
        /// </summary>
        public Vector3 Position => _position.Vector3;

        /// <summary>
        /// This property is used to get the rotation form this SerializedTransform.
        /// </summary>
        public Quaternion Rotation => _rotation.Quaternion;

        /// <summary>
        /// This property is used to get the local scale form this SerializedTransform.
        /// </summary>
        public Vector3 LocalScale => _scale.Vector3;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a SerializableTransform from
        /// the given transform.
        /// </summary>
        /// <param name="transform">The transform that you want to make serializable.</param>
        public SerializableTransform(Transform transform) {
            _position = new SerializableVector3(transform.position);
            _rotation = new SerializableQuaternion(transform.rotation);
            _scale = new SerializableVector3(transform.localScale);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to update the given transform with the
        /// SerializableTransforms data.
        /// </summary>
        /// <param name="transform">The transform you want to update.</param>
        /// <param name="applyLocalScale">If true the Serialized local scale will
        /// also be applied to the transform.</param>
        public void UpdateTransform(Transform transform, bool applyLocalScale = false) {
            transform.position = Position;
            transform.rotation = Rotation;
            if(applyLocalScale)transform.localScale = LocalScale;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}