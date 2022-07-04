using System;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    [FunctionNode("This node is used for converting a float to an integer.")]
    public class FloatToInt : ConvertingNodes {

        [SerializeField, HideInInspector] private int selection = 0;
        
        public static readonly List<string> Options = new List<string> { "Round", "Floor", "Ceil" };
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("float"));
            outputPorts.Add(new PortInfo<int>("int", GetValue));
        }
        
        [NonSerialized] private CalculationId _lastAction;
        [NonSerialized] private int _lastResult;

        private int GetValue(CalculationId calculationId) {
            if(calculationId == _lastAction) return _lastResult;
            _lastAction = calculationId;
            TryGetPortValue(0, calculationId, out float value);
            _lastResult = selection switch {
                0 => Mathf.RoundToInt(value),
                1 => Mathf.FloorToInt(value),
                2 => Mathf.CeilToInt(value),
                _ => (int)value
            };
            return _lastResult;
        }

        #if UNITY_EDITOR
        
        private DropdownField _field;
        
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            _field = new DropdownField(Options, selection);
            _field.RegisterValueChangedCallback(ValueChanged);
            nodeView.extensionContainer.Add(_field);
        }

        private void ValueChanged(ChangeEvent<string> evt) => selection = Options.IndexOf(evt.newValue);

        private void OnValidate() {
            if(_field == null) return;
            if(selection < 0) selection = 0;
            if(selection > 2) selection = 2;
            _field.index = selection;
        }

        #endif
        
    }
}