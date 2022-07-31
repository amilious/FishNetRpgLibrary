/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using UnityEngine;
using Amilious.Core.Serializables;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extensions to the Vector3Int class.
    /// </summary>
    public static class Vector3IntExtensions {
        
        /// <summary>
        /// This method is used to convert a Vector3Int into a SerializableVector3Int.
        /// </summary>
        /// <param name="vector3Int">The Vector3Int that you want to convert.</param>
        /// <returns>A Serializable version of the given Vector3Int.</returns>
        public static SerializableVector3Int ToSerializable(this Vector3Int vector3Int) {
            return new SerializableVector3Int(vector3Int);
        }
        
        /// <summary>
        /// This method is used to swap the x and y values of a Vector3Int.
        /// </summary>
        /// <param name="vector3Int">The Vector3Int that you want to swap the values for.</param>
        /// <param name="swap">If true the values will be swapped.</param>
        /// <returns>The resulting Vector3Int.</returns>
        public static Vector3Int SwapXY(this Vector3Int vector3Int, bool swap = true) {
            return !swap ? vector3Int : new Vector3Int(vector3Int.y, vector3Int.x, vector3Int.z);
        }
        
        /// <summary>
        /// This method is used to swap the x and z values of a Vector3Int.
        /// </summary>
        /// <param name="vector3Int">The Vector3Int that you want to swap the values for.</param>
        /// <param name="swap">If true the values will be swapped.</param>
        /// <returns>The resulting Vector3Int.</returns>
        public static Vector3Int SwapXZ(this Vector3Int vector3Int, bool swap = true) {
            return !swap ? vector3Int : new Vector3Int(vector3Int.z, vector3Int.y, vector3Int.x);
        }
        
        /// <summary>
        /// This method is used to swap the y and z values of a Vector3Int.
        /// </summary>
        /// <param name="vector3Int">The Vector3Int that you want to swap the values for.</param>
        /// <param name="swap">If true the values will be swapped.</param>
        /// <returns>The resulting Vector3Int.</returns>
        public static Vector3Int SwapYZ(this Vector3Int vector3Int, bool swap = true) {
            return !swap ? vector3Int : new Vector3Int(vector3Int.x, vector3Int.z, vector3Int.y);
        }
        
        /// <summary>
        /// This method is used to get the values from the <see cref="Vector3Int"/>
        /// </summary>
        /// <param name="vector3Int">The <see cref="Vector3Int"/> that you want to get the values for.</param>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <param name="z">The z value.</param>
        public static void GetValues(this Vector3Int vector3Int, out int x, out int y, out int z) {
            x = vector3Int.x;
            y = vector3Int.y;
            z = vector3Int.z;
        }
        
        /// <summary>
        /// This method is used to get the x and y values from the <see cref="Vector3Int"/>
        /// </summary>
        /// <param name="vector3Int">The <see cref="Vector3Int"/> that you want to get the values from.</param>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        public static void GetXYValues(this Vector3Int vector3Int, out int x, out int y) {
            x = vector3Int.x;
            y = vector3Int.y;
        }

        /// <summary>
        /// This method is used to get the x and z values from the <see cref="Vector3Int"/>
        /// </summary>
        /// <param name="vector3Int">The <see cref="Vector3Int"/> that you want to get the values from.</param>
        /// <param name="x">The x value.</param>
        /// <param name="z">The z value.</param>
        public static void GetXZValues(this Vector3Int vector3Int, out int x, out int z) {
            x = vector3Int.x;
            z = vector3Int.z;
        }

        /// <summary>
        /// This method is used to get the y and z values from the <see cref="Vector3Int"/>
        /// </summary>
        /// <param name="vector3Int">The <see cref="Vector3Int"/> that you want to get the values from.</param>
        /// <param name="y">The y value.</param>
        /// <param name="z">The z value.</param>
        public static void GetYZValues(this Vector3Int vector3Int, out int y, out int z) {
            y = vector3Int.y;
            z = vector3Int.z;
        }

        /// <summary>
        /// This method is used to get a <see cref="Vector2Int"/> containing the x position as x and
        /// the z position as y.
        /// </summary>
        /// <param name="vector3Int">The <see cref="Vector3Int"/> that you want to get the x and z positions form.</param>
        /// <returns>The vector's position as x = x and y = z.</returns>
        public static Vector2Int GetXZ(this Vector3Int vector3Int) {
            return new Vector2Int(vector3Int.x, vector3Int.z);
        }

        /// <summary>
        /// This method is used to get a <see cref="Vector2Int"/> containing the y position as x and the
        /// z position as y.
        /// </summary>
        /// <param name="vector3Int">The <see cref="Vector3Int"/> that you want to get the y and z positions from.</param>
        /// <returns>The vector's position as x = y and y = z.</returns>
        public static Vector2Int GetYZ(this Vector3Int vector3Int) {
            return new Vector2Int(vector3Int.y, vector3Int.z);
        }
        
    }
}