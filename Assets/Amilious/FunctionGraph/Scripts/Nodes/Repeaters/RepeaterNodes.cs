namespace Amilious.FunctionGraph.Nodes.Repeaters {
    
    /// <summary>
    /// This is the base class for all repeater nodes.
    /// </summary>
    public abstract class RepeaterNodes : FunctionNode {
        
        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR

        /// <inheritdoc />
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            nodeView.titleContainer.Clear();
            nodeView.titleButtonContainer.Clear();
            nodeView.titleContainer.style.height = 0;
            nodeView.inputContainer.style.height = 20;
            nodeView.outputContainer.style.height = 20;
        }
        
        #endif
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}