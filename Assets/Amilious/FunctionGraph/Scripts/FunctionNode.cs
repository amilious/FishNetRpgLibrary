using System;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph {
    
    public abstract class FunctionNode : ScriptableObject {

        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 position;
        [HideInInspector] public List<Connection> inputConnections = new List<Connection>();

        [NonSerialized] private bool _initialized = false;
        
        public List<Connection> GetInputConnections(int port) =>
            inputConnections.Where(con => con.inputPort == port).ToList();

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

        public static FunctionNodeAttribute GetAttribute(Type type) {
            if(AttributeCache.TryGetValue(type, out var value)) return value;
            //get the attribute
            var attribute = type.GetCustomAttribute<FunctionNodeAttribute>();
            AttributeCache.Add(type,attribute);
            return attribute;
        }

        public FunctionNodeAttribute GetAttribute() => GetAttribute(GetType());

        public string GetDescription  => GetAttribute()?.Description;

        public bool IsHidden => GetAttribute()?.Hidden ?? false;
        
        public virtual void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView){}
        
        public void AddInputConnection(FunctionNode output, int outputPort, int inputPort) {
            if(ContainsConnectionFrom(output, outputPort, inputPort)) return;
            inputConnections.Add(new Connection{inputPort = inputPort, outputNode = output, outputPort = outputPort});
            UnityEditor.AssetDatabase.SaveAssets(); //may need to mark dirty
        }

        public void RemoveInputConnection(FunctionNode output, int outputPort, int inputPort) {
            inputConnections.RemoveAll(x => x.inputPort == inputPort && x.outputPort == outputPort &&
                                            x.outputNode.guid == output.guid);
            UnityEditor.AssetDatabase.SaveAssets();
        }
        
        #endif

        public bool ContainsConnectionFrom(FunctionNode output, int outputPort, int inputPort) {
            return inputConnections.Any(x => x.inputPort == inputPort && x.outputPort == outputPort &&
                                             x.outputNode.guid == output.guid);
        }

        public void Initialize() {
            if(_initialized) return;
            _initialized = true;
            SetUpPorts(_inputPorts, _outputPorts);
        }

        protected abstract void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts);

        private readonly List<IPortInfo> _inputPorts = new List<IPortInfo>();

        private readonly List<IPortInfo> _outputPorts = new List<IPortInfo>();

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