namespace Amilious.FunctionGraph.Nodes.InputNodes {
    
    /// <summary>
    /// This class is used as a base class for all input nodes
    /// </summary>
    public abstract class InputNodes : FunctionNode {
        
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