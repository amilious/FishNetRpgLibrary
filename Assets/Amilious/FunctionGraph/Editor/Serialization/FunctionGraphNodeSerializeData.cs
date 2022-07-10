using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Amilious.FunctionGraph.Nodes.Hidden;

namespace Amilious.FunctionGraph.Editor.Serialization {
    
    /// <summary>
    /// This class is used to serialize node data for copy and paste.
    /// </summary>
    [Serializable]
    public class FunctionGraphNodeSerializeData {

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the type of the node.
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// This property contains the copied nodes guid.
        /// </summary>
        public string Guid { get; set; }
        
        /// <summary>
        /// This property contains the nodes x position.
        /// </summary>
        public float XPos { get; set; }
        
        /// <summary>
        /// This property contains the nodes y position.
        /// </summary>
        public float YPos { get; set; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is a default constructor that will be used by the serialization.
        /// </summary>
        public FunctionGraphNodeSerializeData() { }

        /// <summary>
        /// This constructor is used to 
        /// </summary>
        /// <param name="nodeView">The node view that you want to serialize.</param>
        /// <param name="offset">The view's current offset.</param>
        /// <param name="scale">The view's current scale.</param>
        public FunctionGraphNodeSerializeData(FunctionNodeView nodeView, Vector2 offset, Vector2 scale) {
            Type = nodeView.Node.GetType().Name;
            Guid = nodeView.Node.guid;
            var start = new Vector2(nodeView.Node.Position.x, nodeView.Node.Position.y);
            start += offset;
            start += scale;
            XPos = start.x;
            YPos = start.y;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the type of node.
        /// </summary>
        /// <returns>The type of the node.</returns>
        public Type GetCastedType() {
            var type = TypeCache.GetTypesDerivedFrom<FunctionNode>()
                .Where(t => !t.IsAbstract&&!typeof(HiddenNode).IsAssignableFrom(t))
                .FirstOrDefault(x=>x.Name==Type);
            return type;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}