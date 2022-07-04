using System;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Amilious.FunctionGraph.Nodes.InputNodes {
    
    [FunctionNode("This node is used to represent a float value.")]
    public class FloatNode : InputNodes {

        [SerializeField] private float value;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<float>("",GetValue));
        }

        private float GetValue(CalculationId arg) => value;

        #if UNITY_EDITOR

        private UnityEditor.UIElements.FloatField _field;
        
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            _field = new UnityEditor.UIElements.FloatField {
                style = { paddingLeft = 33, paddingTop  = -10, minWidth = 30, top = -15, height = 15},
                value = value
            };
            _field.RegisterValueChangedCallback(OnChanged);
            nodeView.inputContainer.Add(new Label(" Float:"){style = { top = -1}});
            nodeView.inputContainer.Add(_field);
        }
        private void OnChanged(ChangeEvent<float> evt)=> value = evt.newValue;

        private void OnValidate() {
            if(_field==null) return;
            _field.value = value;
        }

        #endif
        
    }
}