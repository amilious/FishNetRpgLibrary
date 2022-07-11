using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.InputNodes {
    
    /// <summary>
    /// This node is used to represent a bool value.
    /// </summary>
    [FunctionNode("This node is used to represent a bool value.")]
    public class BoolNode : InputNodes {
        
        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        [Serializable] public enum BoolEnum{True,False}
        [SerializeField] private BoolEnum value = BoolEnum.False;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Non-Editor Only Methods ////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<bool>("",_=> value == BoolEnum.True));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR

        /// <summary>
        /// This field is used to display the value on the node.
        /// </summary>
        private UnityEditor.UIElements.EnumField _field;
        
        /// <inheritdoc />
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            _field = new UnityEditor.UIElements.EnumField(value){ style = { top = -3 } };
            _field.RegisterValueChangedCallback(OnChanged);
            nodeView.inputContainer.Add(_field);
        }

        /// <summary>
        /// This method is called when the value is updated from the node.
        /// </summary>
        /// <param name="evt">The change event.</param>
        private void OnChanged(ChangeEvent<Enum> evt) => value = (BoolEnum)evt.newValue;

        /// <summary>
        /// This method is called when any changes are made in the inspector.
        /// </summary>
        private void OnValidate() {
            if(_field==null) return;
            _field.value = value;
        }

        #endif
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
    }
}