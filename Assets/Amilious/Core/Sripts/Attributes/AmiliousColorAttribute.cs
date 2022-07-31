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

namespace Amilious.Core.Attributes {
    
    /// <summary>
    /// This attribute is used to indicate that the amilious color drawer should be used.
    /// </summary>
    public class AmiliousColorAttribute : PropertyAttribute {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// If true the color property will allow hdr.
        /// </summary>
        public bool UseHDR { get; }
        
        /// <summary>
        /// If true the color will show the alpha.
        /// </summary>
        public bool ShowAlpha { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This attribute is used to indicate that the amilious color drawer should be used.
        /// </summary>
        /// <param name="useHDR">If true the color will be drawn in hdr, otherwise it will not.</param>
        /// <param name="showAlpha">If true the color drawer will show the alpha channel.</param>
        public AmiliousColorAttribute(bool useHDR = false, bool showAlpha = true) {

            ShowAlpha = showAlpha;
            UseHDR = useHDR;

        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}