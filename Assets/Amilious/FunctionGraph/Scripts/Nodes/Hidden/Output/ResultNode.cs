using System.Collections.Generic;
using UnityEngine;

namespace Amilious.FunctionGraph.Nodes.Hidden.Output {
    
    public abstract class ResultNode<T> : HiddenNode {
        
        [HideInInspector] public string label = "result";
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<T>(label));
        }

        public override bool IsResultNode => true;


        #if UNITY_EDITOR

        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.titleContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#8338ec", out var c) ? c : 
                    nodeView.titleContainer.style.backgroundColor;
            nodeView.inputContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#264653aa", out var c2) ? c2 : 
                    nodeView.inputContainer.style.backgroundColor;
        }

        #endif

        public T GetResult(CalculationId calculationId) =>
            TryGetPortValue(0, calculationId, out T value) ? value : default;

        public void SetLabel(string label) => this.label = label;

    }
    
    public abstract class ResultNode<T1,T2> : HiddenNode {
        
        [HideInInspector] public string label1 = "result 1";
        [HideInInspector] public string label2 = "result 2";
        
        public override bool IsResultNode => true;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<T1>(label1));
            inputPorts.Add(new PortInfo<T2>(label2));
        }
        
        #if UNITY_EDITOR

        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.titleContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#8338ec", out var c) ? c : 
                    nodeView.titleContainer.style.backgroundColor;
            nodeView.inputContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#264653aa", out var c2) ? c2 : 
                    nodeView.inputContainer.style.backgroundColor;
        }

        #endif
        
        public T1 GetResult1(CalculationId calculationId) =>
            TryGetPortValue(0, calculationId, out T1 value) ? value : default;
        
        public T2 GetResult2(CalculationId calculationId) =>
            TryGetPortValue(1, calculationId, out T2 value) ? value : default;

        public void SetLabel(string label1, string label2) {
            this.label1 = label1;
            this.label2 = label2;
        }
    }
    
    public abstract class ResultNode<T1,T2,T3> : HiddenNode {
        
        [HideInInspector] public string label1 = "result 1";
        [HideInInspector] public string label2 = "result 2";
        [HideInInspector] public string label3 = "result 3";
        
        public override bool IsResultNode => true;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<T1>(label1));
            inputPorts.Add(new PortInfo<T2>(label2));
            inputPorts.Add(new PortInfo<T3>(label3));
        }
        
        #if UNITY_EDITOR

        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.titleContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#8338ec", out var c) ? c : 
                    nodeView.titleContainer.style.backgroundColor;
            nodeView.inputContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#264653aa", out var c2) ? c2 : 
                    nodeView.inputContainer.style.backgroundColor;
        }

        #endif
        
        public T1 GetResult1(CalculationId calculationId) =>
            TryGetPortValue(0, calculationId, out T1 value) ? value : default;
        
        public T2 GetResult2(CalculationId calculationId) =>
            TryGetPortValue(1, calculationId, out T2 value) ? value : default;
        
        public T3 GetResult3(CalculationId calculationId) =>
            TryGetPortValue(1, calculationId, out T3 value) ? value : default;

        public void SetLabel(string label1, string label2, string label3) {
            this.label1 = label1;
            this.label2 = label2;
            this.label3 = label3;
        }
    }
    
    public abstract class ResultNode<T1,T2,T3,T4> : HiddenNode {
        
        [HideInInspector] public string label1 = "result 1";
        [HideInInspector] public string label2 = "result 2";
        [HideInInspector] public string label3 = "result 3";
        [HideInInspector] public string label4 = "result 4";
        
        public override bool IsResultNode => true;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<T1>(label1));
            inputPorts.Add(new PortInfo<T2>(label2));
            inputPorts.Add(new PortInfo<T3>(label3));
            inputPorts.Add(new PortInfo<T4>(label4));
        }
        
        #if UNITY_EDITOR

        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.titleContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#8338ec", out var c) ? c : 
                    nodeView.titleContainer.style.backgroundColor;
            nodeView.inputContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#264653aa", out var c2) ? c2 : 
                    nodeView.inputContainer.style.backgroundColor;
        }

        #endif
        
        public T1 GetResult1(CalculationId calculationId) =>
            TryGetPortValue(0, calculationId, out T1 value) ? value : default;
        
        public T2 GetResult2(CalculationId calculationId) =>
            TryGetPortValue(1, calculationId, out T2 value) ? value : default;
        
        public T3 GetResult3(CalculationId calculationId) =>
            TryGetPortValue(1, calculationId, out T3 value) ? value : default;

        public T4 GetResult4(CalculationId calculationId) =>
            TryGetPortValue(1, calculationId, out T4 value) ? value : default;

        public void SetLabel(string label1, string label2, string label3, string label4) {
            this.label1 = label1;
            this.label2 = label2;
            this.label3 = label3;
            this.label4 = label4;
        }
    }
    
    public abstract class ResultNode<T1,T2,T3,T4,T5> : HiddenNode {
        
        [HideInInspector] public string label1 = "result 1";
        [HideInInspector] public string label2 = "result 2";
        [HideInInspector] public string label3 = "result 3";
        [HideInInspector] public string label4 = "result 4";
        [HideInInspector] public string label5 = "result 5";
        
        public override bool IsResultNode => true;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<T1>(label1));
            inputPorts.Add(new PortInfo<T2>(label2));
            inputPorts.Add(new PortInfo<T3>(label3));
            inputPorts.Add(new PortInfo<T4>(label4));
            inputPorts.Add(new PortInfo<T5>(label5));
        }
        
        
        #if UNITY_EDITOR

        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.titleContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#8338ec", out var c) ? c : 
                    nodeView.titleContainer.style.backgroundColor;
            nodeView.inputContainer.style.backgroundColor =
                ColorUtility.TryParseHtmlString("#264653aa", out var c2) ? c2 : 
                    nodeView.inputContainer.style.backgroundColor;
        }

        #endif
        
        public T1 GetResult1(CalculationId calculationId) =>
            TryGetPortValue(0, calculationId, out T1 value) ? value : default;
        
        public T2 GetResult2(CalculationId calculationId) =>
            TryGetPortValue(1, calculationId, out T2 value) ? value : default;
        
        public T3 GetResult3(CalculationId calculationId) =>
            TryGetPortValue(1, calculationId, out T3 value) ? value : default;

        public T4 GetResult4(CalculationId calculationId) =>
            TryGetPortValue(1, calculationId, out T4 value) ? value : default;
        
        public T5 GetResult5(CalculationId calculationId) =>
            TryGetPortValue(1, calculationId, out T5 value) ? value : default;

        public void SetLabel(string label1, string label2, string label3, string label4, string label5) {
            this.label1 = label1;
            this.label2 = label2;
            this.label3 = label3;
            this.label4 = label4;
            this.label4 = label5;
        }
    }
    
}