using UnityEngine.UIElements;

namespace Amilious.FunctionGraph.Nodes.Tests {
    
    /// <summary>
    /// This is the base class for the test nodes.
    /// </summary>
    public abstract class TestNodes : FunctionNode {
        
        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR
        
        private Label _label;

        /// <inheritdocs />
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            _label = new Label("no test executed") { style = { marginLeft = 5, marginRight = 5 } };
            var button = new Button(OnClick) { text = "Test Value" };
            nodeView.extensionContainer.Add(_label);
            nodeView.extensionContainer.Add(button);
        }

        /// <summary>
        /// This method is called when the test button is clicked.
        /// </summary>
        private void OnClick() => TestValue(new CalculationId());
        
        #endif
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private & Protected Methods ////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdocs />
        protected void SetLabel(CalculationId id, string value) {
            #if UNITY_EDITOR
            _label.text = $"Calculation Id: {id.Id}\n";
            _label.text += $"Value: {value}";
            #endif
        }

        /// <summary>
        /// This method is called when the test button is pressed.
        /// </summary>
        /// <param name="id">The test calculation id.</param>
        protected abstract void TestValue(CalculationId id);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}