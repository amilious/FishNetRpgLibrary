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

namespace Amilious.Core {
    
    public static class MathV {
        
        /// <summary>
        /// This method is used to get the angle in degrees based on
        /// the given position and direction.
        /// </summary>
        /// <param name="position">The position of the object.</param>
        /// <param name="dir">The angle direction.</param>
        /// <returns>The angle in degrees.</returns>
        public static float AnglesFromDirection(Vector3 position, Vector3 dir) {
            var forwardLimitPos = position + dir;
            var srcAngles = Mathf.Rad2Deg * Mathf.Atan2(
                forwardLimitPos.z - position.z, 
                forwardLimitPos.x - position.x);
            return srcAngles;
        }
        
        /// <summary>
        /// This method is used to get the mod result for both x and y.
        /// </summary>
        /// <param name="vector2">The vector2 that you want to get the mod values of.</param>
        /// <param name="mod">The mod value that you want to apply to the vector2.</param>
        /// <returns>The mod results.</returns>
        public static Vector2 Mod(Vector2 vector2, float mod) {
            return new Vector2(vector2.x % mod, vector2.y % mod);
        }

        /// <summary>
        /// This method is used to get the mod result for x, y, and z.
        /// </summary>
        /// <param name="vector3">The vector3 that you want to get the mod values of.</param>
        /// <param name="mod">The mod value that you want to apply to the vector3.</param>
        /// <returns>The mod results.</returns>
        public static Vector3 Mod(Vector3 vector3, float mod) {
            return new Vector3(vector3.x % mod, vector3.y % mod, vector3.z % mod);
        }

        /// <summary>
        /// This method is used to get the mod result for both x and y.
        /// </summary>
        /// <param name="vector2">The vector2 that you want to get the mod values of.</param>
        /// <param name="mod">The mod values that you want to apply to the vector2.</param>
        /// <returns>The mod results.</returns>
        public static Vector2 Mod(Vector2 vector2, Vector2 mod) {
            return new Vector2(vector2.x % mod.x, vector2.y % mod.y);
        }

        /// <summary>
        /// This method is used to get the mod result for x, y, and z.
        /// </summary>
        /// <param name="vector3">The vector3 that you want to get the mod values of.</param>
        /// <param name="mod">The mod value that you want to apply to the vector3.</param>
        /// <returns>The mod results.</returns>
        public static Vector3 Mod(Vector3 vector3, Vector3 mod) {
            return new Vector3(vector3.x % mod.x, vector3.y % mod.y, vector3.z % mod.z);
        }

        /// <summary>
        /// This method is used round the given vector2 values to the nearest integer.
        /// </summary>
        /// <param name="vector2">The vector2 that you want to round.</param>
        /// <returns>The rounded vector2.</returns>
        public static Vector2 Round(Vector2 vector2) {
            return new Vector2(Mathf.Round(vector2.x), Mathf.Round(vector2.y));
        }

        /// <summary>
        /// This method is used round the given vector3 values to the nearest integer.
        /// </summary>
        /// <param name="vector3">The vector3 that you want to round.</param>
        /// <returns>The rounded vector3.</returns>
        public static Vector3 Round(Vector3 vector3) {
            return new Vector3(Mathf.Round(vector3.x), Mathf.Round(vector3.y), Mathf.Round(vector3.z));
        }
        
    }
}