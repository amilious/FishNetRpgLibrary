using System;
using UnityEngine;
using Amilious.Core;
using System.Collections.Generic;

namespace Amilious.FunctionGraph {

    /// <summary>
    /// This interface makes a <see cref="ScriptableObject"/> contain a function graph.
    /// <remarks>THIS INTERFACE CAN ONLY BE USED ON AMILIOUS SCRIPTABLE OBJECTS.</remarks>
    /// </summary>
    public interface IFunctionProvider {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This property contains the function's nodes.
        /// </summary>
        public List<FunctionNode> Nodes { get => GetNodes(); }
        
        /// <summary>
        /// This property is used to get the input node for this function.
        /// </summary>
        public FunctionNode GetInputNode { get; }
        
        /// <summary>
        /// This property is used to get the result node for this function.
        /// </summary>
        public FunctionNode GetResultNode { get; }
        
        /// <summary>
        /// This method is used to get the <see cref="AmiliousScriptableObject"/> for the function provider.
        /// </summary>
        public AmiliousScriptableObject FunctionProvider { get; }

        /// <summary>
        /// This property is used to get the functions graph data.
        /// </summary>
        public FunctionGraphData GraphData {
            get => GetGraphData();
            set => SetGraphData(value);
        }

        /// <summary>
        /// This property is used to check if the function has been initialized.
        /// </summary>
        protected bool Initialized { get; set; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Normal Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the function's nodes.
        /// </summary>
        /// <returns>The nodes contained by the function.</returns>
        public List<FunctionNode> GetNodes();

        /// <summary>
        /// This method is used to get the graph data.
        /// </summary>
        /// <returns>The graph data.</returns>
        protected FunctionGraphData GetGraphData();

        /// <summary>
        /// This method is used to set the graph data.
        /// </summary>
        /// <param name="graphData">The graph data.</param>
        protected void SetGraphData(FunctionGraphData graphData);
        
        /// <summary>
        /// This method is used to get the input connections for the given node.
        /// </summary>
        /// <param name="node">The node that you want to get the input connections for.</param>
        /// <param name="inputId">The id of the input port that you want to get the connections for.</param>
        /// <returns>The input connections for the given node at the given port index.</returns>
        public IEnumerable<Connection> GetInputConnections(FunctionNode node, int inputId) {
            return node.GetInputConnections(inputId);
        }

        /// <summary>
        /// This method is called by the editor to initialize 
        /// </summary>
        public void Initialize() {
            if(Initialized) return;
            Initialized = true;
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(FunctionProvider);
            UnityEditor.AssetDatabase.SaveAssets();
            //add graph data
            if(GraphData == null) {
                GraphData = ScriptableObject.CreateInstance<FunctionGraphData>();
                GraphData.name = "GraphData";
                UnityEditor.AssetDatabase.AddObjectToAsset(GraphData, FunctionProvider);
                UnityEditor.EditorUtility.SetDirty(GraphData);
            }
            var toInitialize = InitializeInputAndOutputs() ?? new List<FunctionNode>();
            foreach(var node in toInitialize) {
                node.guid  = UnityEditor.GUID.Generate().ToString();
                Nodes.Add(node);
                UnityEditor.AssetDatabase.AddObjectToAsset(node, FunctionProvider);
                UnityEditor.EditorUtility.SetDirty(node);
            }
            var provider = FunctionProvider as IFunctionProvider;
            if(toInitialize.Count!=0)AfterInitialization(provider);
            UnityEditor.EditorUtility.SetDirty(FunctionProvider);
            UnityEditor.AssetDatabase.SaveAssets();
            #endif
        }

        /// <summary>
        /// This method is called after the Input and Output nodes have been initialized.  This method will
        /// only be called if items are returned from the <see cref="InitializeInputAndOutputs"/> method.
        /// </summary>
        /// <param name="provider">A casted version of the function provider.</param>
        public void AfterInitialization(IFunctionProvider provider);

        /// <summary>
        /// This method is used to initialize input and output ports.
        /// </summary>
        /// <returns></returns>
        public List<FunctionNode> InitializeInputAndOutputs();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR

        /// <summary>
        /// This method is used to try create a node of the given type.
        /// </summary>
        /// <param name="type">The type of node that you want to create.</param>
        /// <param name="node">The created node or null if not created.</param>
        /// <returns>True if able to create a node for the given type, otherwise false.</returns>
        public bool TryCreateNode(Type type, out FunctionNode node) {
            node = ScriptableObject.CreateInstance(type) as FunctionNode;
            if(node == null) return false;
            node.name = type.Name;
            node.guid = UnityEditor.GUID.Generate().ToString();
            Nodes.Add(node);
            UnityEditor.AssetDatabase.AddObjectToAsset(node, FunctionProvider);
            UnityEditor.AssetDatabase.SaveAssets();
            return true;
        }

        /// <summary>
        /// This method is used to delete the given node.
        /// </summary>
        /// <param name="node">The node that you want to delete.</param>
        public void DeleteNode(FunctionNode node) {
            Nodes.Remove(node);
            //remove the connections from the child nodes
            node.outputConnections.ForEach(x=>x.inputNode.RemoveConnection(x));
            node.inputConnections.ForEach(x=>x.outputNode.RemoveConnection(x));
            //remove the node's scriptable object.
            UnityEditor.AssetDatabase.RemoveObjectFromAsset(node);
            UnityEditor.AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// This method is used to add a connection to the function.
        /// </summary>
        /// <param name="connection">The connection that you want to add.</param>
        public void AddConnection(Connection connection) {
            connection.inputNode.AddConnection(connection);
            connection.outputNode.AddConnection(connection);
        }

        /// <summary>
        /// This method is used to remove a connection from the function.
        /// </summary>
        /// <param name="connection">The connection that you want to remove.</param>
        public void RemoveConnection(Connection connection) {
            connection.inputNode.RemoveConnection(connection);
            connection.outputNode.RemoveConnection(connection);
        }

        #endif
        #endregion

    }
    
}