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