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
using UnityEditor.Experimental.GraphView;

namespace Amilious.FunctionGraph.Editor.Serialization {
    
    /// <summary>
    /// This class is used to serialize edges.
    /// </summary>
    [Serializable]
    public class FunctionGraphConnectionSerializedData {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the input node's guid.
        /// </summary>
        public string InputNode { get; set; }
        
        /// <summary>
        /// This property contains the output node's guid.
        /// </summary>
        public string OutputNode { get; set; }
        
        /// <summary>
        /// This property contains the input port index.
        /// </summary>
        public int InputPort { get; set; }
        
        /// <summary>
        /// This property contains the output port index.
        /// </summary>
        public int OutputPort { get; set; }
        
        #endregion
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is the default constructor that will be used by the serialization.
        /// </summary>
        public FunctionGraphConnectionSerializedData() { }

        /// <summary>
        /// This constructor is used to serialize the given edge.
        /// </summary>
        /// <param name="edge">The edge that you want to serialize.</param>
        public FunctionGraphConnectionSerializedData(Edge edge) {
            InputNode = edge.InputNode().guid;
            OutputNode = edge.OutputNode().guid;
            InputPort = edge.InputPort();
            OutputPort = edge.OutputPort();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}