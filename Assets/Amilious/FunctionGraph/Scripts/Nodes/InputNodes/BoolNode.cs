using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.InputNodes {
    
    [FunctionNode("This node is used to represent a bool value.")]
    public class BoolNode : InputNodes {
        
        [SerializeField, HideInInspector] private int value;
        
        public static readonly List<string> Options = new List<string> {"False", "True" };
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<bool>("",GetValue));
        }

        private bool GetValue(CalculationId arg) => value==1;

        #if UNITY_EDITOR

        private DropdownField _field;
        
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            _field = new DropdownField(Options, value) { style = { top = -3 } };
            _field.RegisterValueChangedCallback(OnChanged);
            nodeView.inputContainer.Add(_field);
        }

        private void OnChanged(ChangeEvent<string> evt) => value = Options.IndexOf(evt.newValue);

        private void OnValidate() {
            if(_field==null) return;
            _field.value = Options[value];
        }

        #endif
    }
}