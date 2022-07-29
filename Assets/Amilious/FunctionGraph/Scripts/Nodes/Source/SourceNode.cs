using UnityEngine;
using System.Collections.Generic;
using Amilious.Core.Attributes;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Source {
    
    // ReSharper disable ParameterHidesMember
    
    [FunctionNode("This node is the input node for the function",true,false)]
    public abstract class SourceNode<T> : FunctionNode {
        
        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, DynamicLabel(nameof(label))] private T defaultValue;
        [SerializeField,HideInInspector] private string label;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Non-Serialized Fields //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// The value of the first output port.
        /// </summary>
        private T _value;
        
        /// <summary>
        /// This field is used to keep track of whether or not the value has been set.
        /// </summary>
        private bool _setValue;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool IsInputNode => true;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR

        /// <inheritdoc />
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.titleContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#3a86ff", out var c) ? c : Color.blue;
        }

        #endif
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Override Methdods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<T>(label,GetValue));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to set the source's value.
        /// </summary>
        /// <param name="value">The value of the first port.</param>
        public void SetValue(T value) {
            _setValue = true;
            _value = value;
        }

        /// <summary>
        /// This method is used to set the source's label.
        /// </summary>
        /// <param name="label">The label for the first port.</param>
        public void SetLabel(string label) => this.label = label;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the value of the port.
        /// </summary>
        /// <param name="id">The calculation.</param>
        /// <returns>The value of the port.</returns>
        private T GetValue(CalculationId id) => !_setValue?defaultValue:_value;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
    [FunctionNode("This node is the input node for the function",true,false)]
    public abstract class SourceNode<T1,T2> : FunctionNode {
        
        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////

        [SerializeField, DynamicLabel(nameof(label1))] private T1 defaultValue1;
        [SerializeField, DynamicLabel(nameof(label2))] private T2 defaultValue2;
        [SerializeField,HideInInspector] private string label1 = "value 1";
        [SerializeField,HideInInspector] private string label2 = "value 2";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Non-Serialized Fields //////////////////////////////////////////////////////////////////////////////////
                
        /// <summary>
        /// The value of the first output port.
        /// </summary>
        private T1 _value1;
        
        /// <summary>
        /// The value of the second output port.
        /// </summary>
        private T2 _value2;
        
        /// <summary>
        /// This field is used to keep track of whether or not the values have been set.
        /// </summary>
        private bool _setValues;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool IsInputNode => true;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR

        /// <inheritdoc />
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.titleContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#3a86ff", out var c) ? c : Color.blue;
        }

        #endif
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Override Methdods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<T1>(label1,GetValue1));
            outputPorts.Add(new PortInfo<T2>(label2,GetValue2));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to set the source's values.
        /// </summary>
        /// <param name="value1">The value of the first port.</param>
        /// <param name="value2">The value of the second port.</param>
        public void SetValues(T1 value1, T2 value2) {
            _value1 = value1;
            _value2 = value2;
            _setValues = true;
        }

        /// <summary>
        /// This method is used to set the source's labels.
        /// </summary>
        /// <param name="label1">The label for the first port.</param>
        /// <param name="label2">The label for the second port.</param>
        public void SetLabels(string label1, string label2) {
            this.label1 = label1;
            this.label2 = label2;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the value of the first port.
        /// </summary>
        /// <param name="id">The calculation.</param>
        /// <returns>The value of the first port.</returns>
        private T1 GetValue1(CalculationId id) => !_setValues?defaultValue1:_value1;

        /// <summary>
        /// This method is used to get the value of the second port.
        /// </summary>
        /// <param name="id">The calculation.</param>
        /// <returns>The value of the second port.</returns>
        private T2 GetValue2(CalculationId id) => !_setValues?defaultValue2:_value2;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
    [FunctionNode("This node is the input node for the function",true,false)]
    public abstract class SourceNode<T1,T2,T3> : FunctionNode {

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////

        [SerializeField, DynamicLabel(nameof(label1))] private T1 defaultValue1;
        [SerializeField, DynamicLabel(nameof(label2))] private T2 defaultValue2;
        [SerializeField, DynamicLabel(nameof(label3))] private T3 defaultValue3;
        [SerializeField,HideInInspector] private string label1 = "value 1";
        [SerializeField,HideInInspector] private string label2 = "value 2";
        [SerializeField,HideInInspector] private string label3 = "value 3";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
        #region Non-Serialized Fields //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// The value of the first output port.
        /// </summary>
        private T1 _value1;
        
        /// <summary>
        /// The value of the second output port.
        /// </summary>
        private T2 _value2;
        
        /// <summary>
        /// The value of the third output port.
        /// </summary>
        private T3 _value3;
        
        /// <summary>
        /// This field is used to keep track of whether or not the values have been set.
        /// </summary>
        private bool _setValues;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool IsInputNode => true;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR

        /// <inheritdoc />
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.titleContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#3a86ff", out var c) ? c : Color.blue;
        }

        #endif
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Override Methdods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<T1>(label1,GetValue1));
            outputPorts.Add(new PortInfo<T2>(label2,GetValue2));
            outputPorts.Add(new PortInfo<T3>(label3,GetValue3));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to set the source's values.
        /// </summary>
        /// <param name="value1">The value of the first port.</param>
        /// <param name="value2">The value of the second port.</param>
        /// <param name="value3">The value of the third port.</param>
        public void SetValues(T1 value1, T2 value2, T3 value3) {
            _value1 = value1;
            _value2 = value2;
            _value3 = value3;
            _setValues = true;
        }

        /// <summary>
        /// This method is used to set the source's labels.
        /// </summary>
        /// <param name="label1">The label for the first port.</param>
        /// <param name="label2">The label for the second port.</param>
        /// <param name="label3">The label for the third port.</param>
        public void SetLabels(string label1, string label2, string label3) {
            this.label1 = label1;
            this.label2 = label2;
            this.label3 = label3;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the value of the first port.
        /// </summary>
        /// <param name="id">The calculation.</param>
        /// <returns>The value of the first port.</returns>
        private T1 GetValue1(CalculationId id) => !_setValues?defaultValue1:_value1;

        /// <summary>
        /// This method is used to get the value of the second port.
        /// </summary>
        /// <param name="id">The calculation.</param>
        /// <returns>The value of the second port.</returns>
        private T2 GetValue2(CalculationId id) => !_setValues?defaultValue2:_value2;

        /// <summary>
        /// This method is used to get the value of the third port.
        /// </summary>
        /// <param name="id">The calculation.</param>
        /// <returns>The value of the third port.</returns>
        private T3 GetValue3(CalculationId id) =>!_setValues?defaultValue3:_value3;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
    [FunctionNode("This node is the input node for the function",true,false)]
    public abstract class SourceNode<T1,T2,T3,T4> : FunctionNode {

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////

        [SerializeField, DynamicLabel(nameof(label1))] private T1 defaultValue1;
        [SerializeField, DynamicLabel(nameof(label2))] private T2 defaultValue2;
        [SerializeField, DynamicLabel(nameof(label3))] private T3 defaultValue3;
        [SerializeField, DynamicLabel(nameof(label4))] private T4 defaultValue4;
        [SerializeField,HideInInspector] private string label1 = "value 1";
        [SerializeField,HideInInspector] private string label2 = "value 2";
        [SerializeField,HideInInspector] private string label3 = "value 3";
        [SerializeField,HideInInspector] private string label4 = "value 4";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Non-Serialized Fields //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// The value of the first out put port.
        /// </summary>
        private T1 _value1;
        
        /// <summary>
        /// The value of the second out put port.
        /// </summary>
        private T2 _value2;
        
        /// <summary>
        /// The value of the third out put port.
        /// </summary>
        private T3 _value3;
        
        /// <summary>
        /// The value of the fourth out put port.
        /// </summary>
        private T4 _value4;

        /// <summary>
        /// This field is used to keep track of whether or not the values have been set.
        /// </summary>
        private bool _setValues;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool IsInputNode => true;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR

        /// <inheritdoc />
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.titleContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#3a86ff", out var c) ? c : Color.blue;
        }

        #endif
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Override Methdods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<T1>(label1,GetValue1));
            outputPorts.Add(new PortInfo<T2>(label2,GetValue2));
            outputPorts.Add(new PortInfo<T3>(label3,GetValue3));
            outputPorts.Add(new PortInfo<T4>(label4,GetValue4));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to set the source's values.
        /// </summary>
        /// <param name="value1">The value of the first port.</param>
        /// <param name="value2">The value of the second port.</param>
        /// <param name="value3">The value of the third port.</param>
        /// <param name="value4">The value of the fourth port.</param>
        public void SetValues(T1 value1, T2 value2, T3 value3, T4 value4) {
            _value1 = value1;
            _value2 = value2;
            _value3 = value3;
            _value4 = value4;
            _setValues = true;
        }

        /// <summary>
        /// This method is used to set the source's labels.
        /// </summary>
        /// <param name="label1">The label for the first port.</param>
        /// <param name="label2">The label for the second port.</param>
        /// <param name="label3">The label for the third port.</param>
        /// <param name="label4">The label for the fourth port.</param>
        public void SetLabels(string label1, string label2, string label3, string label4) {
            this.label1 = label1;
            this.label2 = label2;
            this.label3 = label3;
            this.label4 = label4;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the value of the first port.
        /// </summary>
        /// <param name="id">The calculation.</param>
        /// <returns>The value of the first port.</returns>
        private T1 GetValue1(CalculationId id) => !_setValues?defaultValue1:_value1;

        /// <summary>
        /// This method is used to get the value of the second port.
        /// </summary>
        /// <param name="id">The calculation.</param>
        /// <returns>The value of the second port.</returns>
        private T2 GetValue2(CalculationId id) => !_setValues?defaultValue2:_value2;
        
        /// <summary>
        /// This method is used to get the value of the third port.
        /// </summary>
        /// <param name="id">The calculation.</param>
        /// <returns>The value of the third port.</returns>
        private T3 GetValue3(CalculationId id) => !_setValues?defaultValue3:_value3;
        
        /// <summary>
        /// This method is used to get the value of the fourth port.
        /// </summary>
        /// <param name="id">The calculation.</param>
        /// <returns>The value of the fourth port.</returns>
        private T4 GetValue4(CalculationId id) => !_setValues?defaultValue4:_value4;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
       
    }
    
    [FunctionNode("This node is the input node for the function",true,false)]
    public abstract class SourceNode<T1,T2,T3,T4,T5> : FunctionNode {

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////

        [SerializeField, DynamicLabel(nameof(label1))] private T1 defaultValue1;
        [SerializeField, DynamicLabel(nameof(label2))] private T2 defaultValue2;
        [SerializeField, DynamicLabel(nameof(label3))] private T3 defaultValue3;
        [SerializeField, DynamicLabel(nameof(label4))] private T4 defaultValue4;
        [SerializeField, DynamicLabel(nameof(label5))] private T5 defaultValue5;
        [SerializeField,HideInInspector] private string label1 = "value 1";
        [SerializeField,HideInInspector] private string label2 = "value 2";
        [SerializeField,HideInInspector] private string label3 = "value 3";
        [SerializeField,HideInInspector] private string label4 = "value 4";
        [SerializeField,HideInInspector] private string label5 = "value 5";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Non-Serialized Fields //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// The value of the first out put port.
        /// </summary>
        private T1 _value1;
        
        /// <summary>
        /// The value of the second out put port.
        /// </summary>
        private T2 _value2;
        
        /// <summary>
        /// The value of the third out put port.
        /// </summary>
        private T3 _value3;
        
        /// <summary>
        /// The value of the fourth out put port.
        /// </summary>
        private T4 _value4;
        
        /// <summary>
        /// The value of the fifth out put port.
        /// </summary>
        private T5 _value5;
        
        /// <summary>
        /// This field is used to keep track of whether or not the values have been set.
        /// </summary>
        private bool _setValues;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool IsInputNode => true;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR

        /// <inheritdoc />
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.titleContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#3a86ff", out var c) ? c : Color.blue;
        }

        #endif
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Override Methdods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<T1>(label1,GetValue1));
            outputPorts.Add(new PortInfo<T2>(label2,GetValue2));
            outputPorts.Add(new PortInfo<T3>(label3,GetValue3));
            outputPorts.Add(new PortInfo<T4>(label4,GetValue4));
            outputPorts.Add(new PortInfo<T5>(label5,GetValue5));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to set the source's values.
        /// </summary>
        /// <param name="value1">The value of the first port.</param>
        /// <param name="value2">The value of the second port.</param>
        /// <param name="value3">The value of the third port.</param>
        /// <param name="value4">The value of the fourth port.</param>
        /// <param name="value5">The value of the fifth port.</param>
        public void SetValues(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) {
            _value1 = value1;
            _value2 = value2;
            _value3 = value3;
            _value4 = value4;
            _value5 = value5;
            _setValues = true;
        }

        /// <summary>
        /// This method is used to set the source's labels.
        /// </summary>
        /// <param name="label1">The label for the first port.</param>
        /// <param name="label2">The label for the second port.</param>
        /// <param name="label3">The label for the third port.</param>
        /// <param name="label4">The label for the fourth port.</param>
        /// <param name="label5">The label for the fifth port.</param>
        public void SetLabels(string label1, string label2, string label3, string label4, string label5) {
            this.label1 = label1;
            this.label2 = label2;
            this.label3 = label3;
            this.label4 = label4;
            this.label5 = label5;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the value of the first port.
        /// </summary>
        /// <param name="id">The calculation.</param>
        /// <returns>The value of the first port.</returns>
        private T1 GetValue1(CalculationId id) => !_setValues?defaultValue1:_value1;

        /// <summary>
        /// This method is used to get the value of the second port.
        /// </summary>
        /// <param name="id">The calculation.</param>
        /// <returns>The value of the second port.</returns>
        private T2 GetValue2(CalculationId id) => !_setValues?defaultValue2:_value2;
        
        /// <summary>
        /// This method is used to get the value of the third port.
        /// </summary>
        /// <param name="id">The calculation.</param>
        /// <returns>The value of the third port.</returns>
        private T3 GetValue3(CalculationId id) => !_setValues?defaultValue3:_value3;
        
        /// <summary>
        /// This method is used to get the value of the fourth port.
        /// </summary>
        /// <param name="id">The calculation.</param>
        /// <returns>The value of the fourth port.</returns>
        private T4 GetValue4(CalculationId id) => !_setValues?defaultValue4:_value4;
        
        /// <summary>
        /// This method is used to get the value of the fifth port.
        /// </summary>
        /// <param name="id">The calculation.</param>
        /// <returns>The value of the fifth port.</returns>
        private T5 GetValue5(CalculationId id) => !_setValues?defaultValue5:_value5;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}