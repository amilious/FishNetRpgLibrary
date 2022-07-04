namespace Amilious.FunctionGraph.Nodes.InputNodes {
    public abstract class InputNodes : FunctionNode {
        
        #if UNITY_EDITOR
        
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            nodeView.titleContainer.Clear();
            nodeView.titleButtonContainer.Clear();
            nodeView.titleContainer.style.height = 0;
            nodeView.inputContainer.style.height = 20;
            nodeView.outputContainer.style.height = 20;
        }
        
        #endif
        
    }
}