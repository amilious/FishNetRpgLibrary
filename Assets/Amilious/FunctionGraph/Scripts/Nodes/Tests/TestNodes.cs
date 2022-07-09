using UnityEngine.UIElements;

namespace Amilious.FunctionGraph.Nodes.Tests {
    public abstract class TestNodes : FunctionNode {
        
        #if UNITY_EDITOR
        
        private Label _label;

        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            _label = new Label("no test executed") { style = { marginLeft = 5, marginRight = 5 } };
            var button = new Button(OnClick) { text = "Test Value" };
            nodeView.extensionContainer.Add(_label);
            nodeView.extensionContainer.Add(button);
        }

        private void OnClick() {
            var testId = new CalculationId();
            _label.text = $"Calculation Id: {testId.Id}\n";
            _label.text += $"Value: {TestValue(testId)}";
        }
        
        #endif

        protected abstract string TestValue(CalculationId id);

    }
}