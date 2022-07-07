using System;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using Amilious.FunctionGraph.Nodes.Hidden;

namespace Amilious.FunctionGraph {
    
    public abstract class FunctionNode : ScriptableObject {

        [HideInInspector] public string guid;
        [SerializeField] private Vector2 position;
        [SerializeField] public List<Connection> inputConnections = new ();
        [SerializeField] public List<Connection> outputConnections = new ();
        [NonSerialized] private bool _initialized = false;

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

        public virtual bool IsLoop => false;

        public virtual bool IsResultNode => false;

        public virtual bool IsInputNode => false;
        
        public void SetPositionWithoutSaving(Vector2 position) => this.position = position;
        
        public IEnumerable<Connection> GetInputConnections(int port) =>
            inputConnections.Where(con => con.inputPort == port);

        public IEnumerable<Connection> GetOutputConnections(int port) =>
            outputConnections.Where(con => con.outputPort == port);

        protected bool TryGetPortValues<T>(int inputPort, CalculationId calculationId, out List<T> values) {
            values = new List<T>();
            var cons = GetInputConnections(inputPort);
            foreach(var con in cons)
                if(con.outputNode.OutputPortInfo[con.outputPort].TryGetResult(calculationId, out T result))
                    values.Add(result);
            return values.Count > 0;
        }

        protected bool TryGetPortValue<T>(int port, CalculationId calculationId, out T value) {
            value = default;
            if(!TryGetPortValues<T>(port, calculationId, out var values)) return false;
            value = values[0];
            return true;
        }

        
        #if UNITY_EDITOR
        
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

        public bool HasOutputConnectionToNode(FunctionNode node) {
            if(this==node) return true;
            foreach(var connection in outputConnections) {
                if(connection.inputNode==node) return true;
                if(connection.inputNode.HasOutputConnectionToNode(node)) return true;
            }
            return false;
        }

        /*public bool HasInputConnectionToLoop(int port, out FunctionNode loopNode, FunctionNode excludedNode = null) {
            loopNode = null;
            if(IsLoop && this != excludedNode && OutputPortInfo[port].IsLoopPort) {
                loopNode = this;
                return true;
            }
            foreach(var connection in inputConnections) {
                if(connection.outputNode==excludedNode) continue;
                if(connection.outputNode.IsLoop &&
                   connection.outputNode.OutputPortInfo[connection.outputPort].IsLoopPort) {
                    loopNode = connection.outputNode;
                    return true;
                }
                if(connection.outputNode.HasInputConnectionToLoop(connection.outputPort,out loopNode, excludedNode)) return true;
            }
            return false;
        }*/

        public bool IsPartOfLoop(out FunctionNode loop,FunctionNode excludedNode=null) {
            foreach(var input in inputConnections) {
                if(input.outputNode.IsLoop && input.outputNode.OutputPortInfo[input.outputPort].IsLoopPort) {
                    if(input.outputNode == excludedNode) continue;
                    loop = input.outputNode;
                    return true;
                }
            }
            foreach(var output in outputConnections)
                if(output.inputNode.IsPartOfLoop(out loop,excludedNode)) return true;
            loop = null;
            return false;
        }

        public bool HasInputConnectionToNode(FunctionNode node) {
            if(this==node) return true;
            foreach(var connection in inputConnections) {
                if(connection.outputNode==node) return true;
                if(connection.outputNode.HasInputConnectionToNode(node)) return true;
            }
            return false;
        }
        
        public static FunctionNodeAttribute GetAttribute(Type type) {
            if(AttributeCache.TryGetValue(type, out var value)) return value;
            //get the attribute
            var attribute = type.GetCustomAttribute<FunctionNodeAttribute>();
            AttributeCache.Add(type,attribute);
            return attribute;
        }

        public FunctionNodeAttribute GetAttribute() => GetAttribute(GetType());

        public string GetDescription =>GetAttribute()?.Description;

        public bool IsHidden => GetAttribute() is { Hidden: true } || typeof(HiddenNode).IsAssignableFrom(GetType());

        public virtual void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView){}
        
        public void AddOutputConnection(Connection connection) {
            if(connection == null) return;
            outputConnections.Add(connection);
        }

        public void AddInputConnection(Connection connection) {
            if(connection == null) return;
            inputConnections.Add(connection);
        }

        public void RemoveOutputConnection(Connection connection) {
            if(connection == null) return;
            outputConnections.RemoveAll(x => x.Equals(connection));
        }

        public void RemoveInputConnection(Connection connection) {
            if(connection == null) return;
            inputConnections.RemoveAll(x => x.Equals(connection));
        }
        
        #endif

        public bool ContainsInputConnectionFrom(FunctionNode output, int outputPort, int inputPort) {
            return inputConnections.Any(x => x.inputPort == inputPort && x.outputPort == outputPort &&
                                             x.outputNode.guid == output.guid);
        }
        
        public bool ContainsOutConnectionFrom(FunctionNode inputNode, int inputPort, int outputPort) {
            return inputConnections.Any(x => x.inputPort == inputPort && x.outputPort == outputPort &&
                                             x.outputNode.guid == inputNode.guid);
        }

        public void Initialize() {
            if(_initialized) return;
            _initialized = true;
            SetUpPorts(_inputPorts, _outputPorts);
        }

        protected abstract void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts);

        private readonly List<IPortInfo> _inputPorts = new ();

        private readonly List<IPortInfo> _outputPorts = new ();

        public List<IPortInfo> InputPortInfo {
            get {
                Initialize();
                return _inputPorts;
            }
        }

        public List<IPortInfo> OutputPortInfo {
            get {
                Initialize();
                return _outputPorts;
            }
        }

    }
}