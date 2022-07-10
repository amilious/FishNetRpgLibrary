using UnityEngine.UIElements;

namespace Amilious.FunctionGraph.Editor {
    
    /// <summary>
    /// This class is used as a spit view in the editor.
    /// </summary>
    public class SplitView : TwoPaneSplitView {
        
        #region UXML Factory ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This allows the split view to be visible for the UI Builder
        /// </summary>
        public new class UxmlFactory : UxmlFactory<SplitView, UxmlTraits> { }
        
        #endregion
        
    }
    
    
}