using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Amilious.FunctionGraph {
    
    /// <summary>
    /// This class is used to store information about the graph.
    /// </summary>
    public class FunctionGraphData : ScriptableObject {

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField] public Vector3 position = Vector3.zero;
        [SerializeField] public Vector3 scale = Vector3.one;
        [SerializeField] public List<FunctionGroup> groups = new List<FunctionGroup>();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is sued to get the group for the given id.
        /// </summary>
        /// <param name="id">The guid of the group.</param>
        /// <returns>The group with the give id.</returns>
        public FunctionGroup GroupFromId(string id) {
            return groups.FirstOrDefault(x => x.id == id);
        }

        /// <summary>
        /// This method is used to remove the group that has the given id.
        /// </summary>
        /// <param name="id">The id of the group that you want to remove.</param>
        /// <returns>True if the node existed and was removed, otherwise false.</returns>
        public bool RemoveGroup(string id) {
            return groups.RemoveAll(x => x.id == id) > 0;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}