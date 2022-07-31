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

using System;

namespace Amilious.Core.Attributes {
    
    /// <summary>
    /// This attribute is used to add a project link that will be displayed in the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class EditorLinkAttribute : Attribute {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// The tooltip to use for the link.
        /// </summary>
        public string ToolTip { get; }
        
        /// <summary>
        /// The path of the resource to use as the links icon.
        /// </summary>
        public string IconResourcePath { get; }
        
        /// <summary>
        /// The address to navigate to when the link is clicked.
        /// </summary>
        public string Link { get; }
        
        /// <summary>
        /// The display name for the link.
        /// </summary>
        public string LinkName { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This attribute is used to add a project link that will be displayed in the inspector.
        /// </summary>
        /// <param name="toolTip">The links tooltip.</param>
        /// <param name="iconResourcePath">A path to a resource to use as the links icon.</param>
        /// <param name="link">The address to navigate to when the link is clicked.</param>
        /// <param name="linkName">The name that is displayed as the link.</param>
        public EditorLinkAttribute(string toolTip, string iconResourcePath, string link, 
            string linkName = null) {
            ToolTip = toolTip;
            IconResourcePath = iconResourcePath;
            Link = link;
            LinkName = linkName;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}