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

namespace Amilious.Core.Attributes {
    
    /// <summary>
    /// This attribute is used to add an item to a tab group.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class AmiliousTabAttribute : Attribute {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the name of the tab group that the property should belong to.
        /// </summary>
        public string TabGroup { get; }
        
        /// <summary>
        /// This property contains the name of the tab that the property should belong to.
        /// </summary>
        public string TabName { get; }
        
        /// <summary>
        /// This property contains the order of the item within the tab.
        /// </summary>
        public int Order { get; }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This attribute is used to add an item to the default tab group.
        /// </summary>
        /// <param name="tabName">The name of the tab that the property belongs to.</param>
        /// <param name="order">The property order for this property on the tab.</param>
        /// <seealso cref="AmiliousTabAttribute(string,string,int)"/>
        public AmiliousTabAttribute(string tabName, int order = 0) {
            TabGroup = string.Empty;
            TabName = tabName ?? string.Empty;
            Order = order;
        }

        /// <summary>
        /// This attribute is used to add an item to the given tab group and tab.
        /// </summary>
        /// <param name="tabGroup">The name of the tab group that the property belongs to.</param>
        /// <param name="tabName">The name of the tab that the property belongs to.</param>
        /// <param name="order">The property order for this property on the tab.</param>
        /// <seealso cref="AmiliousTabAttribute(string,int)"/>
        public AmiliousTabAttribute(string tabGroup, string tabName, int order = 0) {
            TabGroup = tabGroup ?? string.Empty;
            TabName = tabName ?? string.Empty;
            Order = order;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}