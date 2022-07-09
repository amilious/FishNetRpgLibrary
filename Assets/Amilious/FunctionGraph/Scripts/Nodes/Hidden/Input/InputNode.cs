using System.Collections.Generic;
using UnityEngine;

namespace Amilious.FunctionGraph.Nodes.Hidden.Input {
    
    public abstract class InputNode<T> : HiddenNode {
        
        [SerializeField] private T defaultValue;
        
        private T _value;
        private bool _setValue;
        [HideInInspector] public string label;

        public override bool IsInputNode => true;

        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<T>(label,GetValue));
        }

        private T GetValue(CalculationId calculationId) => !_setValue?defaultValue:_value;

        #if UNITY_EDITOR

        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.titleContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#3a86ff", out var c) ? c : Color.blue;
        }

        #endif
        
        public void SetValue(T value) {
            _setValue = true;
            _value = value;
        }

        public void SetLabel(string label) {
            this.label = label;
        }
        
    }
    
    public abstract class InputNode<T1,T2> : HiddenNode {

        [SerializeField] private T1 defaultValue1;
        [SerializeField] private T2 defaultValue2;
        
        private T1 _value1;
        private T2 _value2;
        private bool _setValues;
        [HideInInspector] public string label1 = "value 1";
        [HideInInspector] public string label2 = "value 2";
        
        public override bool IsInputNode => true;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<T1>(label1,GetValue1));
            outputPorts.Add(new PortInfo<T2>(label2,GetValue2));
        }
        
        #if UNITY_EDITOR

        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.titleContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#3a86ff", out var c) ? c : Color.blue;
        }

        #endif

        private T2 GetValue2(CalculationId arg) => !_setValues?defaultValue2:_value2;

        private T1 GetValue1(CalculationId calculationId) => !_setValues?defaultValue1:_value1;

        public void SetValues(T1 value1, T2 value2) {
            _value1 = value1;
            _value2 = value2;
            _setValues = true;
        }

        public void SetLabels(string label1, string label2) {
            this.label1 = label1;
            this.label2 = label2;
        }

    }
    
    public abstract class InputNode<T1,T2,T3> : HiddenNode {

        [SerializeField] private T1 defaultValue1;
        [SerializeField] private T2 defaultValue2;
        [SerializeField] private T3 defaultValue3;
        
        private T1 _value1;
        private T2 _value2;
        private T3 _value3;
        private bool _setValues;
        [HideInInspector] public string label1 = "value 1";
        [HideInInspector] public string label2 = "value 2";
        [HideInInspector] public string label3 = "value 3";
        
        public override bool IsInputNode => true;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<T1>(label1,GetValue1));
            outputPorts.Add(new PortInfo<T2>(label2,GetValue2));
            outputPorts.Add(new PortInfo<T3>(label3,GetValue3));
        }
        
        #if UNITY_EDITOR

        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.titleContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#3a86ff", out var c) ? c : Color.blue;
        }

        #endif

        private T3 GetValue3(CalculationId arg) =>!_setValues?defaultValue3:_value3;

        private T2 GetValue2(CalculationId arg) => !_setValues?defaultValue2:_value2;

        private T1 GetValue1(CalculationId calculationId) => !_setValues?defaultValue1:_value1;

        public void SetValues(T1 value1, T2 value2, T3 value3) {
            _value1 = value1;
            _value2 = value2;
            _value3 = value3;
            _setValues = true;
        }

        public void SetLabels(string label1, string label2, string label3) {
            this.label1 = label1;
            this.label2 = label2;
            this.label3 = label3;
        }

    }
    
    public abstract class InputNode<T1,T2,T3,T4> : HiddenNode {

        [SerializeField] private T1 defaultValue1;
        [SerializeField] private T2 defaultValue2;
        [SerializeField] private T3 defaultValue3;
        [SerializeField] private T4 defaultValue4;
        
        private T1 _value1;
        private T2 _value2;
        private T3 _value3;
        private T4 _value4;
        private bool _setValues;
        [HideInInspector] public string label1 = "value 1";
        [HideInInspector] public string label2 = "value 2";
        [HideInInspector] public string label3 = "value 3";
        [HideInInspector] public string label4 = "value 4";
        
        public override bool IsInputNode => true;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<T1>(label1,GetValue1));
            outputPorts.Add(new PortInfo<T2>(label2,GetValue2));
            outputPorts.Add(new PortInfo<T3>(label3,GetValue3));
            outputPorts.Add(new PortInfo<T4>(label4,GetValue4));
        }

        #if UNITY_EDITOR

        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.titleContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#3a86ff", out var c) ? c : Color.blue;
        }

        #endif
        
        private T4 GetValue4(CalculationId arg) => !_setValues?defaultValue4:_value4;
        
        private T3 GetValue3(CalculationId arg) => !_setValues?defaultValue3:_value3;

        private T2 GetValue2(CalculationId arg) => !_setValues?defaultValue2:_value2;

        private T1 GetValue1(CalculationId calculationId) => !_setValues?defaultValue1:_value1;

        public void SetValues(T1 value1, T2 value2, T3 value3, T4 value4) {
            _value1 = value1;
            _value2 = value2;
            _value3 = value3;
            _value4 = value4;
            _setValues = true;
        }

        public void SetLabels(string label1, string label2, string label3, string label4) {
            this.label1 = label1;
            this.label2 = label2;
            this.label3 = label3;
            this.label4 = label4;
        }

    }
    
    public abstract class InputNode<T1,T2,T3,T4,T5> : HiddenNode {


        [SerializeField] private T1 defaultValue1;
        [SerializeField] private T2 defaultValue2;
        [SerializeField] private T3 defaultValue3;
        [SerializeField] private T4 defaultValue4;
        [SerializeField] private T5 defaultValue5;
        private T1 _value1;
        private T2 _value2;
        private T3 _value3;
        private T4 _value4;
        private T5 _value5;
        private bool _setValues;
        [HideInInspector] public string label1 = "value 1";
        [HideInInspector] public string label2 = "value 2";
        [HideInInspector] public string label3 = "value 3";
        [HideInInspector] public string label4 = "value 4";
        [HideInInspector] public string label5 = "value 5";
        
        public override bool IsInputNode => true;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<T1>(label1,GetValue1));
            outputPorts.Add(new PortInfo<T2>(label2,GetValue2));
            outputPorts.Add(new PortInfo<T3>(label3,GetValue3));
            outputPorts.Add(new PortInfo<T4>(label4,GetValue4));
            outputPorts.Add(new PortInfo<T5>(label5,GetValue5));
        }

        #if UNITY_EDITOR

        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.titleContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#3a86ff", out var c) ? c : Color.blue;
        }

        #endif
        
        private T5 GetValue5(CalculationId arg) => !_setValues?defaultValue5:_value5;
        
        private T4 GetValue4(CalculationId arg) => !_setValues?defaultValue4:_value4;
        
        private T3 GetValue3(CalculationId arg) => !_setValues?defaultValue3:_value3;

        private T2 GetValue2(CalculationId arg) => !_setValues?defaultValue2:_value2;

        private T1 GetValue1(CalculationId calculationId) => !_setValues?defaultValue1:_value1;

        public void SetValues(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) {
            _value1 = value1;
            _value2 = value2;
            _value3 = value3;
            _value4 = value4;
            _value5 = value5;
            _setValues = true;
        }

        public void SetLabels(string label1, string label2, string label3, string label4, string label5) {
            this.label1 = label1;
            this.label2 = label2;
            this.label3 = label3;
            this.label4 = label4;
            this.label5 = label5;
        }

    }
    
}