using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using Node = UnityEditor.Experimental.GraphView.Node;

namespace Amilious.FunctionGraph.Editor {
    
    public class FunctionNodeView : Node {

        public Action<FunctionNodeView> OnNodeSelected;
        public Action<FunctionNodeView> OnNodeUnselected;
        
        public FunctionNode Node { get; }

        public readonly List<Port> Input = new List<Port>();

        public readonly List<Port> Output = new List<Port>();
        
        public FunctionNodeView(FunctionNode node) {
            Node = node;
            Node.Initialize();
            title = node.name.SplitCamelCase();
            tooltip = node.GetDescription;
            viewDataKey = node.guid;
            style.left = Node.position.x;
            style.top = Node.position.y;
            CreateInputPorts();
            CreateOutputPorts();
            Node.ModifyNodeView(this);
            extensionContainer.style.backgroundColor = ColorUtility.TryParseHtmlString("#001d3d",out var c)?c:Color.black;
            RefreshExpandedState();
        }

        public sealed override string title {
            get { return base.title; }
            set { base.title = value; }
        }

        private readonly Dictionary<Type, Color> _typeColors = new Dictionary<Type, Color>() {
            [typeof(float)] = Color.magenta,
            [typeof(bool)] = Color.cyan,
            [typeof(Vector2)] = Color.yellow,
            [typeof(Vector3)] = Color.red,
            [typeof(int)] = Color.green
        };

        private readonly Dictionary<Type, string> _typeFriendlyName = new Dictionary<Type, string>() {
            [typeof(float)] = "float",
            [typeof(bool)] = "bool",
            [typeof(Vector2)] = "Vector2",
            [typeof(Vector3)] = "Vector3",
            [typeof(int)] = "int"
        };

        private string GetPortToolTip(IPortInfo portInfo) {
            if(portInfo.Tooltip != null) return portInfo.Tooltip;
            if(_typeFriendlyName.TryGetValue(portInfo.Type, out var name)) return name;
            return portInfo.Type.ToString();
        }

        private void CreateInputPorts() {
            foreach(var inputInfo in Node.InputPortInfo) {
                var port = InstantiatePort(
                    inputInfo.Horizontal?Orientation.Horizontal:Orientation.Vertical, 
                    Direction.Input, 
                    inputInfo.AllowMultiple?Port.Capacity.Multi:Port.Capacity.Single, 
                    inputInfo.Type);
                port.portName = inputInfo.Name;
                port.tooltip = GetPortToolTip(inputInfo);
                port.portColor = _typeColors.TryGetValue(inputInfo.Type, out var color) ? color : Color.black;
                port.userData = inputInfo;
                Input.Add(port);
                inputInfo.SetIndex(Input.IndexOf(port));
                inputContainer.Add(port);
            }
        }
        
        private void CreateOutputPorts() {
            foreach(var outputInfo in Node.OutputPortInfo) {
                var port = InstantiatePort(
                    outputInfo.Horizontal?Orientation.Horizontal:Orientation.Vertical, 
                    Direction.Output, 
                    outputInfo.AllowMultiple?Port.Capacity.Multi:Port.Capacity.Single, 
                    outputInfo.Type);
                port.portName = outputInfo.Name;
                port.tooltip = GetPortToolTip(outputInfo);
                port.portColor = _typeColors.TryGetValue(outputInfo.Type, out var color) ? color : Color.black;
                port.userData = outputInfo;
                Output.Add(port);
                outputInfo.SetIndex(Output.IndexOf(port));
                outputContainer.Add(port);
            }
        }

        public override void SetPosition(Rect newPos) {
            base.SetPosition(newPos);
            Node.position.x = newPos.xMin;
            Node.position.y = newPos.yMin;
        }

        public override void OnSelected() {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }

        public override void OnUnselected() {
            base.OnUnselected();
            OnNodeUnselected?.Invoke(this);
        }
    }
    
}