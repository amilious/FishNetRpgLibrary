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

namespace Amilious.FunctionGraph {
    
    /// <summary>
    /// This class is used to store information about a connection between two nodes.
    /// </summary>
    [Serializable]
    public class Connection {

        #region Public Fields //////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// The node on the input side of the connection.
        /// </summary>
        public FunctionNode inputNode;
        
        /// <summary>
        /// The node on the output side of the connection.
        /// </summary>
        public FunctionNode outputNode;
        
        /// <summary>
        /// The input node's connected input port.
        /// </summary>
        public int inputPort;
        
        /// <summary>
        /// The output node's connected output port.
        /// </summary>
        public int outputPort;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a new connection instance.
        /// </summary>
        /// <param name="input">The input node.</param>
        /// <param name="inputPort">The input port number.</param>
        /// <param name="output">The output node.</param>
        /// <param name="outputPort">The output port number.</param>
        public Connection(FunctionNode input, int inputPort, FunctionNode output, int outputPort) {
            inputNode = input;
            outputNode = output;
            this.inputPort = inputPort;
            this.outputPort = outputPort;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to check if a connection is equal to another object.
        /// </summary>
        /// <param name="obj">The other object.</param>
        /// <returns>True if the connection is equal to provided object, otherwise false.</returns>
        public override bool Equals(object obj) {
            if(obj is not Connection connection) return false;
            return Equals(inputNode, connection.inputNode) &&
                   Equals(outputNode, connection.outputNode) && 
                   inputPort == connection.inputPort && 
                   outputPort == connection.outputPort;
        }

        /// <summary>
        /// This method is used to get the hash code for this connection.
        /// </summary>
        /// <returns>The hash code for this connections.</returns>
        public override int GetHashCode() {
            // ReSharper disable NonReadonlyMemberInGetHashCode
            return HashCode.Combine(inputNode, outputNode, inputPort, outputPort);
            // ReSharper enable NonReadonlyMemberInGetHashCode
        }

        /// <summary>
        /// This method is used to check if a connection is equal to another.
        /// </summary>
        /// <param name="other">The other connection.</param>
        /// <returns>True if the connection is equal to provided connection, otherwise false.</returns>
        protected bool Equals(Connection other) {
            return Equals(inputNode, other.inputNode) &&
                   Equals(outputNode, other.outputNode) && 
                   inputPort == other.inputPort && 
                   outputPort == other.outputPort;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}