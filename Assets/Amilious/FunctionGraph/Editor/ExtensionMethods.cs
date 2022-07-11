using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace Amilious.FunctionGraph.Editor {
    
    /// <summary>
    /// This class is used to added convenience methods to graph elements.
    /// </summary>
    public static class ExtensionMethods {

        /// <summary>
        /// This method is used to get the <see cref="FunctionNodeView"/> associated with the port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns>The <see cref="FunctionNodeView"/> associated with the port.</returns>
        public static FunctionNodeView FunctionNodeView(this Port port) => port.node as FunctionNodeView;

        /// <summary>
        /// This method is used to get the <see cref="FunctionNode"/> associated with the port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns>The <see cref="FunctionNode"/> associated with the port.</returns>
        public static FunctionNode FunctionNode(this Port port) => port.FunctionNodeView().Node;

        /// <summary>
        /// This method is used to get the port's index.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns>The input index of the port if it is an input port, otherwise the output index.</returns>
        public static int GetIndex(this Port port) => ((IPortInfo)port.userData).Index;

        /// <summary>
        /// This method is used to get the <see cref="IPortInfo"/> for the port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns>The <see cref="IPortInfo"/> associated with the port.</returns>
        public static IPortInfo PortInfo(this Port port) => (IPortInfo)port.userData;

        /// <summary>
        /// This method is used to check if the port is a loop port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns>True if the port is a loop port, otherwise false.</returns>
        public static bool IsLoop(this Port port) => port.PortInfo().IsLoopPort;

        /// <summary>
        /// This method is used to check if the port is of the given type.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <typeparam name="T">The type that you want to check.</typeparam>
        /// <returns>True if the port is of the given type, otherwise false.</returns>
        public static bool IsType<T>(this Port port) => port.portType == typeof(T);

        /// <summary>
        /// This method is used to check if the port is an input port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns>True if the port is an input port.</returns>
        public static bool IsInput(this Port port) => port.direction == Direction.Input;

        /// <summary>
        /// This method is used to check if the port is an output port.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns>True if the port is an output port, otherwise false.</returns>
        public static bool IsOutPut(this Port port) => port.direction == Direction.Output;
        
        /// <summary>
        /// This method is used to check if the port has connections leading to the given port.
        /// </summary>
        /// <param name="start">The port.</param>
        /// <param name="port">The port that you want to see if there is a connection to.</param>
        /// <returns>True if their is a connection leading from the port to the given port.</returns>
        public static bool HasConnectionTo(this Port start, Port port) {
            if(start.direction == port.direction) return false;
            var input = start.direction == Direction.Input ? start : port;
            var output = start.direction == Direction.Output ? start : port;
            return input.FunctionNode().ContainsInputConnectionFrom(output.FunctionNode(), 
                output.GetIndex(), input.GetIndex());
        }

        /// <summary>
        /// This method is used to get the <see cref="FunctionNode"/> that contains the edge's input port.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns>The <see cref="FunctionNode"/> that contains the edge's input port.</returns>
        public static FunctionNode InputNode(this Edge edge) => edge.input.FunctionNode();

        /// <summary>
        /// This method is used to get the <see cref="FunctionNodeView"/> that contains the edge's input port.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns>The <see cref="FunctionNodeView"/> that contains the edge's input port.</returns>
        public static FunctionNodeView InputNodeView(this Edge edge) => edge.input.FunctionNodeView();

        /// <summary>
        /// This method is used to get the index of the edge's input port.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns>The index of the edge's input port.</returns>
        public static int InputPort(this Edge edge) => edge.input.GetIndex();
        
        /// <summary>
        /// This method is used to get the <see cref="FunctionNode"/> that contains the edge's output port.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns>The <see cref="FunctionNode"/> that contains the edge's output port.</returns>
        public static FunctionNode OutputNode(this Edge edge) => edge.output.FunctionNode();

        /// <summary>
        /// This method is used to get the <see cref="FunctionNodeView"/> that contains the edge's output port.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns>The <see cref="FunctionNodeView"/> that contains the edge's output port.</returns>
        public static FunctionNodeView OutputNodeView(this Edge edge) => edge.output.FunctionNodeView();

        /// <summary>
        /// This method is used to get the index of the edge's output port.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns>The index of the edge's output port.</returns>
        public static int OutputPort(this Edge edge) => edge.output.GetIndex();

        /// <summary>
        /// This method is used to get the <see cref="Connection"/> associated with the edge.
        /// </summary>
        /// <param name="edge">The edge.</param>
        /// <returns>The <see cref="Connection"/> associated with the edge,</returns>
        public static Connection Connection(this Edge edge) {
            if(edge.userData is Connection connection) return connection;
            var con = new Connection(edge.InputNode(),edge.InputPort(),edge.OutputNode(),edge.OutputPort());
            edge.userData = con;
            return con;
        }

        /// <summary>
        /// This method is used to get the guid id of the group.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns>The group's guid.</returns>
        public static string GetId(this Group group) => group.userData as string;

        /// <summary>
        /// This method is used to generate a unique key for the transform's data.
        /// </summary>
        /// <param name="viewTransform">The transform.</param>
        /// <returns>A unique key for the transform's data.</returns>
        public static string GetStringKey(this ITransform viewTransform) {
            return viewTransform == null ? string.Empty : 
                $"{viewTransform.position.x}x{viewTransform.position.y}scale{viewTransform.scale}";
        }
        
    }
    
}