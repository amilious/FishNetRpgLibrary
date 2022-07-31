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

namespace Amilious.FunctionGraph.Attributes {

    /// <summary>
    /// This attribute is used to add a description to a node.
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Class, Inherited = true)]
    public class FunctionNodeAttribute : Attribute {

        /// <summary>
        /// This property contains the nodes description.
        /// </summary>
        public string Description { get; }
        
        /// <summary>
        /// This property contains true if the node is hidden from the new node list.
        /// </summary>
        public bool Hidden { get; }
        
        /// <summary>
        /// This property contains true if the node is removable, otherwise false.
        /// </summary>
        public bool Removable { get; }

        /// <summary>
        /// This attribute is used to add a description to a node. 
        /// </summary>
        /// <param name="description">The nodes description.</param>
        /// <param name="hidden">True if the node is hidden, otherwise false.</param>
        /// <param name="removable">This value is used to indicate if a node is removable.</param>
        public FunctionNodeAttribute(string description = null, bool hidden = false, bool removable = true) {
            Description = description;
            Hidden = hidden;
            Removable = removable;
        }
        
    }
}