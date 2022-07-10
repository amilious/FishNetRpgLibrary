using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    /// <summary>
    /// This node is used for converting a float into an integer.
    /// </summary>
    [FunctionNode("This node is used for converting a float to an integer.")]
    public class FloatToInt : ConvertingNodes {

        #region Serialized Feilds //////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, HideInInspector] private int selection = 0;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
        #region Non-Serialized Feilds //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This list contains the different conversion types.
        /// </summary>
        public static readonly List<string> Options = new (){ "Round", "Floor", "Ceil" };
        
        /// <summary>
        /// This field is used to store the last calculation id.
        /// </summary>
        private CalculationId _lastAction;
        
        /// <summary>
        /// This field is used to store the last calculated value.
        /// </summary>
        private int _lastResult;
        
        /// <summary>
        /// This field is the dropdown used to select the conversion method.
        /// </summary>
        private DropdownField _field;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("float"));
            outputPorts.Add(new PortInfo<int>("int", GetValue));
        }

        /// <summary>
        /// This method is used to get the value of this nodes first output port.
        /// </summary>
        /// <param name="calculationId">The calculation id.</param>
        /// <returns>The value.</returns>
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
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR

        /// <inheritdoc />
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            _field = new DropdownField(Options, selection);
            _field.RegisterValueChangedCallback(ValueChanged);
            nodeView.extensionContainer.Add(_field);
        }

        /// <summary>
        /// This method is called when the type of conversion is changed.
        /// </summary>
        /// <param name="evt">The event.</param>
        private void ValueChanged(ChangeEvent<string> evt) => selection = Options.IndexOf(evt.newValue);

        /// <summary>
        /// This method is called when the node is updated in the inspector.
        /// </summary>
        private void OnValidate() {
            if(_field == null) return;
            if(selection < 0) selection = 0;
            if(selection > 2) selection = 2;
            _field.index = selection;
        }

        #endif
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}