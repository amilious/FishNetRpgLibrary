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
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace Amilious.FunctionGraph.Editor.Serialization {
    
    /// <summary>
    /// This class is used to serialize groups.
    /// </summary>
    [Serializable]
    public class FunctionGraphGroupSerializedData {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains a list of the guid's of the groups nodes.
        /// </summary>
        public List<string> Members { get; set; }
        
        /// <summary>
        /// This property contains the name of the group.
        /// </summary>
        public string Name { get; set; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is the default constructor that is used by the serialization.
        /// </summary>
        public FunctionGraphGroupSerializedData(){}

        /// <summary>
        /// This constructor is used to serialize the given group.
        /// </summary>
        /// <param name="group">The group that you want to serialize.</param>
        public FunctionGraphGroupSerializedData(Group group) {
            Members = new List<string>();
            Name = group.title;
            foreach(var element in group.containedElements) {
                if(element is not FunctionNodeView node) continue;
                Members.Add(node.Node.guid);
            }
        }
        
        #endregion
        
        
    }
    
}