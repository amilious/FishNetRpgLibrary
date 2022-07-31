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
    /// This class is used to serialize a quaternion's values.
    /// </summary>
    [System.Serializable]
    public class SerializableQuaternion {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to hold the quaternions w value.
        /// </summary>
        private float _w;
        
        /// <summary>
        /// This field is used to hold the quaternions x value.
        /// </summary>
        private float _x;
        
        /// <summary>
        /// This field is used to hold the quaternions w value.
        /// </summary>
        private float _y;
        
        /// <summary>
        /// This field is used to hold the quaternions w value.
        /// </summary>
        private float _z;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get a Quaternion form this SerializedQuaternion.
        /// </summary>
        public Quaternion Quaternion => new (_x, _y, _z, _w);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a SerializableQuaternion from
        /// the given Quaternion.
        /// </summary>
        /// <param name="quaternion">The Quaternion that you want to make serializable.</param>
        public SerializableQuaternion(Quaternion quaternion) {
            _w = quaternion.w;
            _x = quaternion.x;
            _y = quaternion.y;
            _z = quaternion.z;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Operator Methods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This operator is used to auto cast a serializable quaternion to a quaternion.
        /// </summary>
        /// <param name="sQuaternion">The serializable quaternion.</param>
        /// <returns>A quaternion.</returns>
        public static explicit operator Quaternion(SerializableQuaternion sQuaternion) => sQuaternion.Quaternion;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}