using System;

namespace Amilious.FunctionGraph {
    
    [Serializable]
    public class Connection {
        public FunctionNode inputNode;
        public FunctionNode outputNode;
        public int inputPort;
        public int outputPort;
        public string loopGuid = null;

        public Connection(FunctionNode input, int inputPort, FunctionNode output, int outputPort) {
            inputNode = input;
            outputNode = output;
            this.inputPort = inputPort;
            this.outputPort = outputPort;
        }

        public override bool Equals(object obj) {
            if(obj is not Connection connection) return false;
            return Equals(inputNode, connection.inputNode) &&
                   Equals(outputNode, connection.outputNode) && 
                   inputPort == connection.inputPort && 
                   outputPort == connection.outputPort;
        }

        public override int GetHashCode() {
            return HashCode.Combine(inputNode, outputNode, inputPort, outputPort, loopGuid);
        }

        protected bool Equals(Connection other) {
            return Equals(inputNode, other.inputNode) &&
                   Equals(outputNode, other.outputNode) && 
                   inputPort == other.inputPort && 
                   outputPort == other.outputPort;
        }

    }
    
    

}