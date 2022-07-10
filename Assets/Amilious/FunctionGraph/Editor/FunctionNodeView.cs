using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using Node = UnityEditor.Experimental.GraphView.Node;

namespace Amilious.FunctionGraph.Editor {
    
    /// <summary>
    /// This class is used to display a <see cref="FunctionNode"/> on the <see cref="FunctionGraphView"/>.
    /// </summary>
    public class FunctionNodeView : Node {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
           
        /// <summary>
        /// This list is used to hold the input ports.
        /// </summary>
        private readonly List<Port> _input = new();

        /// <summary>
        /// This list is used to hold the output ports.
        /// </summary>
        private readonly List<Port> _output = new();
        
        /// <summary>
        /// This dictionary is used to look up port colors by type.
        /// </summary>
        private static readonly Dictionary<Type, Color> TypeColors = new (){
            [typeof(float)] = Color.magenta,
            [typeof(bool)] = Color.cyan,
            [typeof(Vector2)] = Color.yellow,
            [typeof(Vector2Int)] = Color.yellow,
            [typeof(Vector3)] = Color.red,
            [typeof(Vector3Int)] = Color.red,
            [typeof(int)] = Color.green
        };

        /// <summary>
        /// This dictionary is used to lookup friendly type names.
        /// </summary>
        private static readonly Dictionary<Type, string> TypeFriendlyName = new () {
            [typeof(float)] = "float",
            [typeof(bool)] = "bool",
            [typeof(Vector2)] = "Vector2",
            [typeof(Vector3)] = "Vector3",
            [typeof(Vector2Int)] = "Vector2Int",
            [typeof(Vector3Int)] = "Vector3Int",
            [typeof(int)] = "int"
        };
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get the <see cref="Node"/> associated with this <see cref="FunctionNodeView"/>.
        /// </summary>
        public FunctionNode Node { get; }

        /// <summary>
        /// This property is used to get the input ports.
        /// </summary>
        public IReadOnlyList<Port> Input => _input;

        /// <summary>
        /// This property is used to get the output ports.
        /// </summary>
        public IReadOnlyList<Port> Output => _output;

        /// <summary>
        /// This property is used to set the node's title text.
        /// </summary>
        public sealed override string title {
            get { return base.title; }
            set { base.title = value; }
        }

        #endregion
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a <see cref="FunctionNodeView"/> for the given <see cref="FunctionNode"/>.
        /// </summary>
        /// <param name="node">The node that you want to display in this <see cref="FunctionNodeView"/>.</param>
        /// <param name="provider">The function provider that this node belongs to.</param>
        public FunctionNodeView(FunctionNode node, IFunctionProvider provider) {
            Node = node;
            Node.FunctionProvider = provider;
            Node.Initialize();
            title = node.name.SplitCamelCase();
            tooltip = node.GetDescription;
            viewDataKey = node.guid;
            style.left = Node.Position.x;
            style.top = Node.Position.y;
            CreateInputPorts();
            CreateOutputPorts();
            Node.ModifyNodeView(this);
            extensionContainer.style.backgroundColor = 
                ColorUtility.TryParseHtmlString("#2D2D2D",out var c)?c:Color.black;
            RefreshExpandedState();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the tooltip for the given <see cref="IPortInfo"/>.
        /// </summary>
        /// <param name="portInfo">The info that you want to get the tooltip for.</param>
        /// <returns>The tooltip for the given <see cref="IPortInfo"/>.</returns>
        private static string GetPortToolTip(IPortInfo portInfo) {
            if(portInfo.Tooltip != null) return portInfo.Tooltip;
            return TypeFriendlyName.TryGetValue(portInfo.Type, out var friendlyName) ? 
                friendlyName : portInfo.Type.ToString();
        }

        /// <summary>
        /// This method is used to create the input ports for the node view.
        /// </summary>
        private void CreateInputPorts() {
            foreach(var inputInfo in Node.InputPortInfo) {
                var port = InstantiatePort(
                    inputInfo.Horizontal?Orientation.Horizontal:Orientation.Vertical, 
                    Direction.Input, 
                    inputInfo.AllowMultiple?Port.Capacity.Multi:Port.Capacity.Single, 
                    inputInfo.Type);
                port.portName = inputInfo.Name;
                port.tooltip = GetPortToolTip(inputInfo);
                port.portColor = TypeColors.TryGetValue(inputInfo.Type, out var color) ? color : Color.black;
                port.userData = inputInfo;
                _input.Add(port);
                inputInfo.SetIndex(_input.IndexOf(port));
                inputContainer.Add(port);
            }
        }
        
        /// <summary>
        /// This method is used to create the output ports for the node view.
        /// </summary>
        private void CreateOutputPorts() {
            foreach(var outputInfo in Node.OutputPortInfo) {
                var port = InstantiatePort(
                    outputInfo.Horizontal?Orientation.Horizontal:Orientation.Vertical, 
                    Direction.Output, 
                    outputInfo.AllowMultiple?Port.Capacity.Multi:Port.Capacity.Single, 
                    outputInfo.Type);
                port.portName = outputInfo.Name;
                port.tooltip = GetPortToolTip(outputInfo);
                port.portColor = TypeColors.TryGetValue(outputInfo.Type, out var color) ? color : Color.black;
                port.userData = outputInfo;
                _output.Add(port);
                outputInfo.SetIndex(_output.IndexOf(port));
                outputContainer.Add(port);
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}