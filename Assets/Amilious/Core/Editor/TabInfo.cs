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

using UnityEditor;
using Amilious.Core.Attributes;

namespace Amilious.Core.Editor {
    
    /// <summary>
    /// This struct is used to hold information about a tab item.
    /// </summary>
    public struct TabInfo {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the name of the tab group.
        /// </summary>
        public string TabGroup { get; set; }
        
        /// <summary>
        /// This property contains the name of the tab.
        /// </summary>
        public string TabName { get; set; }
        
        /// <summary>
        /// This property contains the serialized property that should be added to the tab.
        /// </summary>
        public SerializedProperty Property { get; set; }
        
        /// <summary>
        /// This property contains the draw order for the property on the tab.
        /// </summary>
        public int Order { get; set; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructor ////////////////////////////////////////////////////////////////////////////////////////////

        public TabInfo(AmiliousTabAttribute attribute, SerializedProperty property) {
            TabGroup = attribute.TabGroup;
            TabName = attribute.TabName;
            Order = attribute.Order;
            Property = property;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}