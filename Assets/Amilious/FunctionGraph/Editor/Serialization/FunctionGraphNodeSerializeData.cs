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
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Amilious.FunctionGraph.Editor.Serialization {
    
    /// <summary>
    /// This class is used to serialize node data for copy and paste.
    /// </summary>
    [Serializable]
    public class FunctionGraphNodeSerializeData {

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the type of the node.
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// This property contains the copied nodes guid.
        /// </summary>
        public string Guid { get; set; }
        
        /// <summary>
        /// This property contains the nodes x position.
        /// </summary>
        public float XPos { get; set; }
        
        /// <summary>
        /// This property contains the nodes y position.
        /// </summary>
        public float YPos { get; set; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is a default constructor that will be used by the serialization.
        /// </summary>
        public FunctionGraphNodeSerializeData() { }

        /// <summary>
        /// This constructor is used to 
        /// </summary>
        /// <param name="nodeView">The node view that you want to serialize.</param>
        /// <param name="offset">The view's current offset.</param>
        /// <param name="scale">The view's current scale.</param>
        public FunctionGraphNodeSerializeData(FunctionNodeView nodeView, Vector2 offset, Vector2 scale) {
            Type = nodeView.Node.GetType().Name;
            Guid = nodeView.Node.guid;
            var start = new Vector2(nodeView.Node.Position.x, nodeView.Node.Position.y);
            start += offset;
            start += scale;
            XPos = start.x;
            YPos = start.y;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the type of node.
        /// </summary>
        /// <returns>The type of the node.</returns>
        public Type GetCastedType() {
            var type = TypeCache.GetTypesDerivedFrom<FunctionNode>()
                .Where(t => !t.IsAbstract&&!FunctionNode.GetAttribute(t).Hidden)
                .FirstOrDefault(x=>x.Name==Type);
            return type;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}