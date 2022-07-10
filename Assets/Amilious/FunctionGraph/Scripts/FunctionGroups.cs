using System;
using System.Collections.Generic;

namespace Amilious.FunctionGraph {
    
    /// <summary>
    /// This class is used to store information about node groups.
    /// </summary>
    [Serializable]
    public class FunctionGroup {

        #region Public Fields //////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This id or guid of the group.
        /// </summary>
        public string id;
        
        /// <summary>
        /// The title of the group.
        /// </summary>
        public string title;
        
        /// <summary>
        /// A list of all the node guids that belong to this group.
        /// </summary>
        public List<string> nodeIds = new ();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}