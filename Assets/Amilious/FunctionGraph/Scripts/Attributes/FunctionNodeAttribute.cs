using System;

namespace Amilious.FunctionGraph.Attributes {

    /// <summary>
    /// This attribute is used to add a description to a node.
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Class, Inherited = true)]
    public class FunctionNodeAttribute : Attribute {

        /// <summary>
        /// This property contains the nodes description.
        /// </summary>
        public string Description { get; }
        
        /// <summary>
        /// This property contains true if the node is hidden from the new node list.
        /// </summary>
        public bool Hidden { get; }
        
        /// <summary>
        /// This property contains true if the node is removable, otherwise false.
        /// </summary>
        public bool Removable { get; }

        /// <summary>
        /// This attribute is used to add a description to a node. 
        /// </summary>
        /// <param name="description">The nodes description.</param>
        /// <param name="hidden">True if the node is hidden, otherwise false.</param>
        /// <param name="removable">This value is used to indicate if a node is removable.</param>
        public FunctionNodeAttribute(string description = null, bool hidden = false, bool removable = true) {
            Description = description;
            Hidden = hidden;
            Removable = removable;
        }
        
    }
}