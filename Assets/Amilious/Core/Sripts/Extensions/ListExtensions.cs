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
//  Website:        http://www.amilious,com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using System;
using System.Collections.Generic;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add methods to the <see cref="List{T}"/> class.
    /// </summary>
    public static class ListExtensions {

        /// <summary>
        /// This method is used to sort a list of paths/
        /// </summary>
        /// <param name="list">The list.</param>
        public static void SortByPath(this List<string> list) {
            list.Sort((a, b) => {
                var aLevels = a.Split('/');
                var bLevels = b.Split('/');
                for(var i = 0; i < aLevels.Length; i++) {
                    if(i >= bLevels.Length) return 1;
                    var value = string.Compare(aLevels[i], bLevels[i], StringComparison.Ordinal);
                    if(value == 0) continue;
                    if(aLevels.Length != bLevels.Length && (i == aLevels.Length - 1 || i == bLevels.Length - 1))
                        return aLevels.Length < bLevels.Length ? 1 : -1;
                    return value;
                }
                return 0;
            });
        }
        
    }
    
}