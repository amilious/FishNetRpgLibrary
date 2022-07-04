using System;

namespace Amilious.FunctionGraph {
    
    [Serializable]
    public class Connection {
        public FunctionNode outputNode;
        public int inputPort;
        public int outputPort;
    }
    
}