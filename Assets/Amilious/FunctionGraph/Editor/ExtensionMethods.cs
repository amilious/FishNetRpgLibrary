using UnityEditor.Experimental.GraphView;

namespace Amilious.FunctionGraph.Editor {
    
    public static class ExtensionMethods {

        public static FunctionNodeView FunctionNodeView(this Port port) => port.node as FunctionNodeView;

        public static FunctionNode FunctionNode(this Port port) => port.FunctionNodeView().Node;

        public static int GetIndex(this Port port) => ((IPortInfo)port.userData).Index;

        public static IPortInfo PortInfo(this Port port) => ((IPortInfo)port.userData);

        public static bool HasConnectionTo(this Port start, Port port) {
            if(start.direction == port.direction) return false;
            var input = start.direction == Direction.Input ? start : port;
            var output = start.direction == Direction.Output ? start : port;
            return input.FunctionNode().ContainsInputConnectionFrom(output.FunctionNode(), 
                output.GetIndex(), input.GetIndex());
        }

        public static FunctionNode InputNode(this Edge edge) => edge.input.FunctionNode();

        public static FunctionNodeView InputNodeView(this Edge edge) => edge.input.FunctionNodeView();

        public static int InputPort(this Edge edge) => edge.input.GetIndex();
        
        public static FunctionNode OutputNode(this Edge edge) => edge.output.FunctionNode();

        public static FunctionNodeView OutputNodeView(this Edge edge) => edge.output.FunctionNodeView();

        public static int OutputPort(this Edge edge) => edge.output.GetIndex();

        public static Connection Connection(this Edge edge) {
            if(edge.userData is Connection connection) return connection;
            var con = new Connection(edge.InputNode(),edge.InputPort(),edge.OutputNode(),edge.OutputPort());
            edge.userData = con;
            return con;
        }

        public static string GetId(this Group group) => group.userData as string;

    }
    
}