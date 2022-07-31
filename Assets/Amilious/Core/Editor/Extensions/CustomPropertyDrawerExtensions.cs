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

using System;
using UnityEditor;
using System.Reflection;

namespace Amilious.Core.Editor.Extensions {
    
    /// <summary>
    /// This class is used to add extension methods to the <see cref="CustomPropertyDrawer"/> class.
    /// </summary>
    public static class CustomPropertyDrawerExtensions {
        
        /// <summary>
        /// This method is used to get the internal type field of a custom property drawer.
        /// </summary>
        /// <param name="drawer">The drawer attribute that you want to get the drawer for.</param>
        /// <param name="type">The type that the drawer is for.</param>
        /// <returns>True if able to get the type, otherwise false.</returns>
        public static bool TryGetDrawersPropertyType(this CustomPropertyDrawer drawer, out Type type) {
            type = default;
            if(drawer == null) return false;
            var typeField = drawer.GetType().GetField("m_Type",
                BindingFlags.NonPublic | BindingFlags.Instance);
            if(typeField == null) return false;
            type = (Type)typeField.GetValue(drawer);
            return true;
        }
        
    }
}