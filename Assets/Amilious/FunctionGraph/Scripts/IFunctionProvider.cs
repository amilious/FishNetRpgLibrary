using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Amilious.FunctionGraph {

    /// <summary>
    /// This interface makes a <see cref="ScriptableObject"/> contain a function graph.
    /// <remarks>THIS INTERFACE CAN ONLY BE USED ON SCRIPTABLE OBJECTS.</remarks>
    /// </summary>
    public interface IFunctionProvider {

        
        public List<FunctionNode> Nodes { get => GetNodes(); }

        public List<FunctionNode> GetNodes();
        
        public ScriptableObject FunctionProvider { get; }

        public FunctionGraphData GraphData {
            get => GetGraphData();
            set => SetGraphData(value);
        }

        protected FunctionGraphData GetGraphData();

        protected void SetGraphData(FunctionGraphData graphData);

        protected bool Initialized { get; set; }
        
        public IEnumerable<Connection> GetInputConnections(FunctionNode node, int inputId) {
            return node.GetInputConnections(inputId);
        }
        
        #if UNITY_EDITOR

        public bool TryCreateNode(Type type, out FunctionNode node) {
            node = ScriptableObject.CreateInstance(type) as FunctionNode;
            if(node == null) return false;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            Nodes.Add(node);
            AssetDatabase.AddObjectToAsset(node, FunctionProvider);
            AssetDatabase.SaveAssets();
            return true;
        }

        public void DeleteNode(FunctionNode node) {
            Nodes.Remove(node);
            //remove the connections from the child nodes
            node.outputConnections.ForEach(x=>x.inputNode.RemoveInputConnection(x));
            node.inputConnections.ForEach(x=>x.outputNode.RemoveOutputConnection(x));
            //remove the node's scriptable object.
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        public Connection AddConnection(Connection connection) {
            connection.inputNode.AddInputConnection(connection);
            connection.outputNode.AddOutputConnection(connection);
            return connection;
        }

        public void RemoveConnection(Connection connection) {
            connection.inputNode.RemoveInputConnection(connection);
            connection.outputNode.RemoveOutputConnection(connection);
        }

        public void RemoveConnectionsTo(FunctionNode node) {
            
        }

        public void RemoveConnection(UnityEditor.Experimental.GraphView.Edge edge) {
            
        }

        #endif

        public void Initialize() {
            if(Initialized) return;
            Initialized = true;
            #if UNITY_EDITOR
            EditorUtility.SetDirty(FunctionProvider);
            AssetDatabase.SaveAssets();
            //add graph data
            if(GraphData == null) {
                GraphData = ScriptableObject.CreateInstance<FunctionGraphData>();
                GraphData.name = "GraphData";
                AssetDatabase.AddObjectToAsset(GraphData, FunctionProvider);
                EditorUtility.SetDirty(GraphData);
            }
            var toInitialize = InitializeInputAndOutputs() ?? new List<FunctionNode>();
            foreach(var node in toInitialize) {
                node.guid  = GUID.Generate().ToString();
                Nodes.Add(node);
                AssetDatabase.AddObjectToAsset(node, FunctionProvider);
                EditorUtility.SetDirty(node);
            }
            var provider = FunctionProvider as IFunctionProvider;
            if(toInitialize.Count!=0)AfterInitialization(provider);
            EditorUtility.SetDirty(FunctionProvider);
            AssetDatabase.SaveAssets();
            #endif
        }

        /// <summary>
        /// This method is called after the Input and Output nodes have been initialized.  This method will
        /// only be called if items are returned from the <see cref="InitializeInputAndOutputs"/> method.
        /// </summary>
        /// <param name="provider">A casted version of the function provider.</param>
        public void AfterInitialization(IFunctionProvider provider);

        public List<FunctionNode> InitializeInputAndOutputs();

    }
    
}