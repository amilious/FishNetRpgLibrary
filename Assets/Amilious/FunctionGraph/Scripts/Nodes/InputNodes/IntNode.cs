using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.InputNodes {
    
    [FunctionNode("This node is used to represent an int value.")]
    public class IntNode : InputNodes {
        
        [SerializeField] private int value;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<int>("",GetValue));
        }

        private int GetValue(CalculationId arg) => value;

        #if UNITY_EDITOR

        private UnityEditor.UIElements.IntegerField _field;
        
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            _field = new UnityEditor.UIElements.IntegerField {
                value = value, 
                style = { paddingLeft = 25, paddingTop  = -10,minWidth = 30, top = -15, height = 15}
            };
            _field.RegisterValueChangedCallback(OnChanged);
            nodeView.inputContainer.Add(new Label(" Int:"){style = { top = -1}});
            nodeView.inputContainer.Add(_field);
        }
        private void OnChanged(ChangeEvent<int> evt)=> value = evt.newValue;

        private void OnValidate() {
            if(_field==null) return;
            _field.value = value;
        }

        #endif
        
    }
}