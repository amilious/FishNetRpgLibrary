using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.InputNodes {
    
    /// <summary>
    /// This node is used to represent a float value.
    /// </summary>
    [FunctionNode("This node is used to represent a float value.")]
    public class FloatNode : InputNodes {

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to store the node's value.
        /// </summary>
        [SerializeField] private float value;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Non-Editor Only Methods ////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<float>("",_=>value));
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR

        /// <summary>
        /// This field is used to display the value on the node.
        /// </summary>
        private UnityEditor.UIElements.FloatField _field;
        
        /// <inheritdoc />
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
        
        /// <summary>
        /// This method is called when the value is updated from the node.
        /// </summary>
        /// <param name="evt">The change event.</param>
        private void OnChanged(ChangeEvent<float> evt)=> value = evt.newValue;

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