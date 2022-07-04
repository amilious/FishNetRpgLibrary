using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace Amilious.FunctionGraph.Editor {
    
    public static class ExtensionMethods {

        public static FunctionNodeView FunctionNodeView(this Port port) => port.node as FunctionNodeView;

        public static FunctionNode FunctionNode(this Port port) => port.FunctionNodeView().Node;
        
        public static int PortIndex(this Port port) {
            if(port.node is not FunctionNodeView node) return 0;
            return port.direction == Direction.Input ? node.Input.IndexOf(port) : node.Output.IndexOf(port);
        }

        public static IPortInfo PortInfo(this Port port) {
            if(port.node is not FunctionNodeView node) return null;
            var index = port.direction == Direction.Input ? node.Input.IndexOf(port) : node.Output.IndexOf(port);
            return port.direction == Direction.Input ? node.Node.InputPortInfo[index] : node.Node.OutputPortInfo[index];
        }

        public static bool HasConnectionTo(this Port start, Port port) {
            if(start.direction == port.direction) return false;
            var input = start.direction == Direction.Input ? start : port;
            var output = start.direction == Direction.Output ? start : port;
            return input.FunctionNode().ContainsConnectionFrom(output.FunctionNode(), 
                output.PortIndex(), input.PortIndex());
        }
        
    }
    
}