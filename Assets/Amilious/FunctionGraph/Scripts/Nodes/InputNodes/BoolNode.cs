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