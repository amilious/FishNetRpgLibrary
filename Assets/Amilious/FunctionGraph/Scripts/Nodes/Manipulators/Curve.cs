using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Manipulators {
    
    [FunctionNode("This node is used to get a value based in the input using a curve.")]
    public class Curve : ManipulatorNodes {

        [SerializeField] private AnimationCurve curveValue = new AnimationCurve() {
            postWrapMode = WrapMode.Clamp, preWrapMode = WrapMode.Clamp, keys = new [] {
                new Keyframe(0,0), new Keyframe(1,1)
            }
        };

        private CalculationId _lastId;
        private float _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("input"));
            outputPorts.Add(new PortInfo<float>("result",GetValue));
        }

        private float GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out float input);
            return _lastValue = curveValue.Evaluate(input);
        }

        #if UNITY_EDITOR
        
        private UnityEditor.UIElements.CurveField _curve;
        
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            _curve = new UnityEditor.UIElements.CurveField {
                value = curveValue,
                renderMode = UnityEditor.UIElements.CurveField.RenderMode.Texture,
                style = { height = 100, top = -3, borderBottomRightRadius = 12, borderBottomLeftRadius = 12, },
            };
            _curve.RegisterValueChangedCallback(OnChange);
            nodeView.extensionContainer.Add(_curve);
            nodeView.extensionContainer.style.paddingTop = 0;
            nodeView.extensionContainer.style.marginBottom = -7;
            nodeView.extensionContainer.style.marginTop = -4;
            nodeView.extensionContainer.style.marginLeft = -4;
            nodeView.extensionContainer.style.marginRight = -4;
        }

        private void OnChange(ChangeEvent<AnimationCurve> evt) {
            curveValue = evt.newValue;
        }

        public void OnValidate() {
            if(_curve == null) return;
            _curve.value = curveValue;
        }
        
        #endif
        
    }
}