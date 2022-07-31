/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                    //
//    _____            .__ .__   .__                             _________  __              .___.__                   //
//   /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /   _____/_/  |_  __ __   __| _/|__|  ____   ______   //
//  /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     \_____  \ \   __\|  |  \ / __ | |  | /  _ \ /  ___/   //
// /    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \      /        \ |  |  |  |  // /_/ | |  |(  <_> )\___ \    //
// \____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    /_______  / |__|  |____/ \____ | |__| \____//____  >   //
//         \/       \/                                  \/             \/                    \/                 \/    //
//                                                                                                                    //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious,com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Manipulators {
    
    /// <summary>
    /// This node is used to get a value based in the input using a curve.
    /// </summary>
    [FunctionNode("This node is used to get a value based in the input using a curve.")]
    public class Curve : ManipulatorNodes {

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField] private AnimationCurve curveValue = new AnimationCurve() {
            postWrapMode = WrapMode.Clamp, preWrapMode = WrapMode.Clamp, keys = new [] {
                new Keyframe(0,0), new Keyframe(1,1)
            }
        };
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Non-Serialized Fields //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This last calculation id.
        /// </summary>
        private CalculationId _lastId;
        
        /// <summary>
        /// The last cached value.
        /// </summary>
        private float _lastValue;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private & Protected Methods ////////////////////////////////////////////////////////////////////////////
                                            
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("input"));
            outputPorts.Add(new PortInfo<float>("result",GetValue));
        }

        /// <summary>
        /// This method is used to calculate the value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The calculated value.</returns>
        private float GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out float input);
            return _lastValue = curveValue.Evaluate(input);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR
        
        /// <summary>
        /// This field hold the curve that is used in the editor.
        /// </summary>
        private UnityEditor.UIElements.CurveField _curve;
        
        /// <inheritdoc />
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

        /// <summary>
        /// This method is called when the curve is edited.
        /// </summary>
        /// <param name="evt">The event.</param>
        private void OnChange(ChangeEvent<AnimationCurve> evt) => curveValue = evt.newValue;

        /// <summary>
        /// This method is called whenever a value is changed in the inspector.
        /// </summary>
        public void OnValidate() {
            if(_curve == null) return;
            _curve.value = curveValue;
        }
        
        #endif
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}