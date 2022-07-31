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
using Amilious.Core.Serializables;

namespace Amilious.Core.Extensions {
    
     /// <summary>
    /// This class is used to add methods to the <see cref="Vector2"/> struct.
    /// </summary>
    public static class Vector2Extensions {
        
        /// <summary>
        /// This method is used to convert a Vector2 into a SerializableVector2.
        /// </summary>
        /// <param name="vector2">The Vector2 that you want to convert.</param>
        /// <returns>A Serializable version of the given Vector2.</returns>
        public static SerializableVector2 ToSerializable(this Vector2 vector2) {
            return new SerializableVector2(vector2);
        }
        
        /// <summary>
        /// This method is used to get the values from the <see cref="Vector2Int"/>
        /// </summary>
        /// <param name="vector2Int">The <see cref="Vector2Int"/> that you want to get the values for.</param>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        public static void GetValues(this Vector2Int vector2Int, out int x, out int y) {
            x = vector2Int.x;
            y = vector2Int.y;
        }
        
    }
}