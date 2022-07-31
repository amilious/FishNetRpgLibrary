////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                    //
//    _____            .__ .__   .__                             _________  __              .___.__                   //
//   /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /   _____/_/  |_  __ __   __| _/|__|  ____   ______   //
//  /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     \_____  \ \   __\|  |  \ / __ | |  | /  _ \ /  ___/   //
// /    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \      /        \ |  |  |  |  // /_/ | |  |(  <_> )\___ \    //
// \____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    /_______  / |__|  |____/ \____ | |__| \____//____  >   //
//         \/       \/                                  \/             \/                    \/                 \/    //
//                                                                                                                    //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious,comUnity          Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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