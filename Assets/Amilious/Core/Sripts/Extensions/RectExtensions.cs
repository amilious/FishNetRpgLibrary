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

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add methods to the <see cref="Rect"/> struct.
    /// </summary>
    public static class RectExtensions {
        
        /// <summary>
        /// This method is used to get a <see cref="Vector2"/> from the <see cref="Rect"/>'s min values.
        /// </summary>
        /// <param name="rect">The <see cref="Rect"/> of which the minimum position is needed.</param>
        /// <returns>The minimum position.</returns>
        public static Vector2 MinPosition(this Rect rect) => new Vector2(rect.xMin, rect.yMin);

        
        /// <summary>
        /// This method is used to get a <see cref="Vector2"/> from the <see cref="Rect"/>'s max values.
        /// </summary>
        /// <param name="rect">The <see cref="Rect"/> of which the maximum position is needed.</param>
        /// <returns>The maximum position.</returns>
        public static Vector2 MaxPosition(this Rect rect) => new Vector2(rect.xMax, rect.yMax);


    }
}