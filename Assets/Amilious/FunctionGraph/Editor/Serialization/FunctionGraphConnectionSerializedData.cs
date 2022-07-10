using System;
using UnityEditor.Experimental.GraphView;

namespace Amilious.FunctionGraph.Editor.Serialization {
    
    /// <summary>
    /// This class is used to serialize edges.
    /// </summary>
    [Serializable]
    public class FunctionGraphConnectionSerializedData {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the input node's guid.
        /// </summary>
        public string InputNode { get; set; }
        
        /// <summary>
        /// This property contains the output node's guid.
        /// </summary>
        public string OutputNode { get; set; }
        
        /// <summary>
        /// This property contains the input port index.
        /// </summary>
        public int InputPort { get; set; }
        
        /// <summary>
        /// This property contains the output port index.
        /// </summary>
        public int OutputPort { get; set; }
        
        #endregion
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is the default constructor that will be used by the serialization.
        /// </summary>
        public FunctionGraphConnectionSerializedData() { }

        /// <summary>
        /// This constructor is used to serialize the given edge.
        /// </summary>
        /// <param name="edge">The edge that you want to serialize.</param>
        public FunctionGraphConnectionSerializedData(Edge edge) {
            InputNode = edge.InputNode().guid;
            OutputNode = edge.OutputNode().guid;
            InputPort = edge.InputPort();
            OutputPort = edge.OutputPort();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}