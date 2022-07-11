using System;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph {
    
    public abstract class FunctionNode : ScriptableObject {

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField,HideInInspector] public string guid;
        [SerializeField,HideInInspector] private Vector2 position;
        [SerializeField,HideInInspector] public List<Connection> inputConnections = new ();
        [SerializeField,HideInInspector] public List<Connection> outputConnections = new ();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This list contains the nodes input ports.
        /// </summary>
        private readonly List<IPortInfo> _inputPorts = new ();

        /// <summary>
        /// This list contains the nodes output ports.
        /// </summary>
        private readonly List<IPortInfo> _outputPorts = new ();
        
        /// <summary>
        /// This field is used to keep track of whether the node has been initialized or not.
        /// </summary>
        private bool _initialized = false;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get the node's position.
        /// </summary>
        public Vector2 Position {
            get => position;
            #if UNITY_EDITOR
            set {
                if(position == value) return;
                position = value;
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.SaveAssets();
            }
            #endif
        }

        /// <summary>
        /// This property is used to check if the node is a special loop node.
        /// </summary>
        public virtual bool IsLoop => false;

        /// <summary>
        /// This property is used to check if the node is a result node.
        /// </summary>
        public virtual bool IsResultNode => false;

        /// <summary>
        /// This property is used to check if the node is a function input node.
        /// </summary>
        public virtual bool IsInputNode => false;

        /// <summary>
        /// This property is used to get the function provider that this node belongs to.
        /// </summary>
        public IFunctionProvider FunctionProvider { get; set; }
        
        /// <summary>
        /// This property is used to get the port info for input ports.
        /// </summary>
        public List<IPortInfo> InputPortInfo { get { Initialize(); return _inputPorts; } }

        /// <summary>
        /// This property is used to get the port info for output ports.
        /// </summary>
        public List<IPortInfo> OutputPortInfo { get { Initialize(); return _outputPorts; } }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to set the node's position without saving.
        /// </summary>
        /// <param name="modePosition">The position.</param>
        public void SetPositionWithoutSaving(Vector2 modePosition) => this.position = modePosition;
        
        /// <summary>
        /// This method is used to get the input connections for the given input port.
        /// </summary>
        /// <param name="port">The input port.</param>
        /// <returns>The input connections for the given port.</returns>
        public IEnumerable<Connection> GetInputConnections(int port) =>
            inputConnections.Where(con => con.inputPort == port);

        /// <summary>
        /// This method is used to get the output connections for the given output port.
        /// </summary>
        /// <param name="port">The output port that you want to get the connections for.</param>
        /// <returns>The output connections for the given port.</returns>
        public IEnumerable<Connection> GetOutputConnections(int port) =>
            outputConnections.Where(con => con.outputPort == port);
        
        /// <summary>
        /// This method is used to check if the node has an input connection that leads to the given node/
        /// </summary>
        /// <param name="output">The out put node.</param>
        /// <param name="outputPort">The output node's output port.</param>
        /// <param name="inputPort">This nodes input port.</param>
        /// <returns>True if there is a connection, otherwise false.</returns>
        public bool ContainsInputConnectionFrom(FunctionNode output, int outputPort, int inputPort) {
            return inputConnections.Any(x => x.inputPort == inputPort && x.outputPort == outputPort &&
                                             x.outputNode.guid == output.guid);
        }
        
        /// <summary>
        /// This method is used to check if the node has an output connection that leads to the give node.
        /// </summary>
        /// <param name="inputNode">The input node.</param>
        /// <param name="inputPort">The input node's input port.</param>
        /// <param name="outputPort">This node's output port.</param>
        /// <returns>True if there is a connection, otherwise false.</returns>
        public bool ContainsOutConnectionFrom(FunctionNode inputNode, int inputPort, int outputPort) {
            return inputConnections.Any(x => x.inputPort == inputPort && x.outputPort == outputPort &&
                                             x.outputNode.guid == inputNode.guid);
        }

        /// <summary>
        /// This method is called to initialize the node.
        /// </summary>
        public void Initialize() {
            if(_initialized) return;
            _initialized = true;
            SetUpPorts(_inputPorts, _outputPorts);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to try get all the values for the given input port.
        /// </summary>
        /// <param name="inputPort">The input port that you want to get the value of.</param>
        /// <param name="calculationId">The connection id.</param>
        /// <param name="values">The values of the given port.</param>
        /// <typeparam name="T">The type of value for the port.</typeparam>
        /// <returns>All the values for the given port.</returns>
        protected bool TryGetPortValues<T>(int inputPort, CalculationId calculationId, out List<T> values) {
            values = new List<T>();
            var cons = GetInputConnections(inputPort);
            var type = typeof(T);
            foreach(var con in cons) {
                var outType = con.outputNode.OutputPortInfo[con.outputPort].Type;
                if(outType == typeof(int) && type == typeof(float)) { //cast an int to a float
                    if(con.outputNode.OutputPortInfo[con.outputPort].TryGetResult(calculationId, out int intResult))
                        values.Add((T)Convert.ChangeType(intResult,type));
                } else { //handle matching cases
                    if(con.outputNode.OutputPortInfo[con.outputPort].TryGetResult(calculationId, out T result)) {
                        values.Add(result);
                    }
                }
            }
            return values.Count > 0;
        }

        /// <summary>
        /// This method is used to try get a single value from the given input port.
        /// </summary>
        /// <param name="inputPort">The input port that you want to get the value of.</param>
        /// <param name="calculationId">The connection id.</param>
        /// <param name="value">The value of the given port.</param>
        /// <typeparam name="T">The type of value for the port.</typeparam>
        /// <returns>The first value of the given port.</returns>
        protected bool TryGetPortValue<T>(int inputPort, CalculationId calculationId, out T value) {
            value = default;
            if(!TryGetPortValues<T>(inputPort, calculationId, out var values)) return false;
            value = values[0];
            return true;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Abstract Methods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to set up the node's input and output ports.
        /// </summary>
        /// <param name="inputPorts">Input ports should be added to this list.</param>
        /// <param name="outputPorts">Output ports should be added to this list.</param>
        protected abstract void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR
        
        /// <summary>
        /// This dictionary is used to cached the FunctionNode attributes by the node type.
        /// </summary>
        public static readonly Dictionary<Type, FunctionNodeAttribute> AttributeCache = new();

        /// <summary>
        /// This method is used to check if the node as an output connection that leads to the result node.
        /// </summary>
        /// <param name="excludedNode">A node that should be excluded from the search.</param>
        /// <returns>True if the node has an output connection that leads to the result.</returns>
        public bool HasOutputConnectionToResult(FunctionNode excludedNode = null) {
            if(IsResultNode&&this!=excludedNode) return true;
            foreach(var connection in outputConnections) {
                if(connection.inputNode==excludedNode) continue;
                if(connection.inputNode.IsResultNode) return true;
                if(connection.inputNode.HasOutputConnectionToResult(excludedNode)) return true;
            }
            return false;
        }

        /// <summary>
        /// This method is used to check if this node has an output connection to the given node.
        /// </summary>
        /// <param name="node">The node that you want to check for connections to.</param>
        /// <returns>True if this node has an output connection to the given node.</returns>
        public bool HasOutputConnectionToNode(FunctionNode node) {
            if(this==node) return true;
            foreach(var connection in outputConnections) {
                if(connection.inputNode==node) return true;
                if(connection.inputNode.HasOutputConnectionToNode(node)) return true;
            }
            return false;
        }

        /// <summary>
        /// This method is used to check if this node has an input connection that leads to a loop source.
        /// </summary>
        /// <param name="loop">The loop source.</param>
        /// <param name="excludedNode">An optional node to exclude from the result.</param>
        /// <returns>True if this node has an input connection that leads to a loop source, otherwise false.</returns>
        public bool HasInputConnectionToLoop(out FunctionNode loop,FunctionNode excludedNode=null) {
            foreach(var input in inputConnections) {
                if(input.outputNode.OutputPortInfo[input.outputPort].IsLoopPort) {
                    if(input.outputNode == excludedNode) continue;
                    loop = input.outputNode;
                    return true;
                }
                if(input.outputNode.HasInputConnectionToLoop(out loop, excludedNode)) return true;
            }
            loop = null;
            return false;
        }

        /// <summary>
        /// This method is used to check if the node has an output connection that leads to loop.
        /// </summary>
        /// <param name="loopNode">The loop node.</param>
        /// <param name="excludedNode">An optional node to exclude from the result.</param>
        /// <returns>True if this node has an output connection that leads to a loop node, otherwise false.</returns>
        public bool HasOutputConnectionToLoop(out FunctionNode loopNode, FunctionNode excludedNode = null) {
            foreach(var output in outputConnections) {
                if(output.inputNode.InputPortInfo[output.inputPort].IsLoopPort) {
                    if(output.inputNode == excludedNode) continue;
                    loopNode = output.inputNode;
                    return true;
                }
                if(output.inputNode.HasOutputConnectionToLoop(out loopNode, excludedNode)) return true;
            }
            loopNode = null;
            return false;
        }

        /// <summary>
        /// This method is used to check if this node has an input connection to the given node.
        /// </summary>
        /// <param name="node">The node that you want to check for connections to.</param>
        /// <returns>True if this node has an input connection to the given node.</returns>
        public bool HasInputConnectionToNode(FunctionNode node) {
            if(this==node) return true;
            foreach(var connection in inputConnections) {
                if(connection.outputNode==node) return true;
                if(connection.outputNode.HasInputConnectionToNode(node)) return true;
            }
            return false;
        }
        
        /// <summary>
        /// This method is used to get a nodes attribute by the node's type.
        /// </summary>
        /// <param name="type">The type of the node.</param>
        /// <returns>The function node attribute</returns>
        public static FunctionNodeAttribute GetAttribute(Type type) {
            if(AttributeCache.TryGetValue(type, out var value)) return value;
            //get the attribute
            var attribute = type.GetCustomAttribute<FunctionNodeAttribute>();
            //make sure that there is a default attribute.
            attribute ??= new FunctionNodeAttribute("");
            AttributeCache.Add(type,attribute);
            return attribute;
        }

        /// <summary>
        /// This method is used to get the node's function node attribute.
        /// </summary>
        /// <returns>The attribute.</returns>
        public FunctionNodeAttribute GetAttribute() => GetAttribute(GetType());
        
        /// <summary>
        /// This property is used to get the node's description.
        /// </summary>
        public string Description =>GetAttribute()?.Description;

        /// <summary>
        /// This property is used to check if the node is hidden.
        /// </summary>
        public bool IsHidden => GetAttribute() is { Hidden: true };

        /// <summary>
        /// This property is true if a node can be deleted, otherwise false.
        /// </summary>
        public bool Removable => GetAttribute() is { Removable: true };

        /// <summary>
        /// This method is used to modify the node view. THIS METHOD SHOULD BE WRAPPED AS SUCH:
        /// <code>#if UNITY_EDITOR
        /// public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView){
        ///     //modifications
        /// }
        /// #endif</code>
        /// </summary>
        /// <param name="nodeView"></param>
        public virtual void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView){}
        
        /// <summary>
        /// This method is used to add a connection to the node.
        /// </summary>
        /// <param name="connection">The connection that you want to add.</param>
        public void AddConnection(Connection connection) {
            if(connection == null) return;
            if(connection.inputNode==this)
                inputConnections.Add(connection);
            else outputConnections.Add(connection);
        }

        /// <summary>
        /// This method is used to remove a connection to the node
        /// </summary>
        /// <param name="connection">The connection that you want to remove.</param>
        public void RemoveConnection(Connection connection) {
            if(connection == null) return;
            if(connection.outputNode == this)
                outputConnections.RemoveAll(x => x.Equals(connection));
            else inputConnections.RemoveAll(x => x.Equals(connection));
        }
        
        #endif
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}