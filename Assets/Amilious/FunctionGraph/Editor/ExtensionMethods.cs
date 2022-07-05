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
            return input.FunctionNode().ContainsConnectionFrom(output.FunctionNode(), 
                output.GetIndex(), input.GetIndex());
        }

        public static FunctionNode InputNode(this Edge edge) => edge.input.FunctionNode();

        public static FunctionNodeView InputNodeView(this Edge edge) => edge.input.FunctionNodeView();

        public static int InputPortIndex(this Edge edge) => edge.input.GetIndex();
        
        public static FunctionNode OutputNode(this Edge edge) => edge.output.FunctionNode();

        public static FunctionNodeView OutputNodeView(this Edge edge) => edge.output.FunctionNodeView();

        public static int OutputPortIndex(this Edge edge) => edge.input.GetIndex();

        public static bool IsLoop(this Edge edge, out string guid) => (guid = edge.userData as string)!=null;

        public static void SetLoopGuid(this Edge edge, string guid) => edge.userData = guid;

    }
    
}