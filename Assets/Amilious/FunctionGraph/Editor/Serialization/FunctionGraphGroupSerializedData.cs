using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace Amilious.FunctionGraph.Editor.Serialization {
    
    /// <summary>
    /// This class is used to serialize groups.
    /// </summary>
    [Serializable]
    public class FunctionGraphGroupSerializedData {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains a list of the guid's of the groups nodes.
        /// </summary>
        public List<string> Members { get; set; }
        
        /// <summary>
        /// This property contains the name of the group.
        /// </summary>
        public string Name { get; set; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is the default constructor that is used by the serialization.
        /// </summary>
        public FunctionGraphGroupSerializedData(){}

        /// <summary>
        /// This constructor is used to serialize the given group.
        /// </summary>
        /// <param name="group">The group that you want to serialize.</param>
        public FunctionGraphGroupSerializedData(Group group) {
            Members = new List<string>();
            Name = group.title;
            foreach(var element in group.containedElements) {
                if(element is not FunctionNodeView node) continue;
                Members.Add(node.Node.guid);
            }
        }
        
        #endregion
        
        
    }
    
}