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
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Result {
    
    // ReSharper disable ParameterHidesMember
    
    [FunctionNode("This node is the result node for the function",true,false)]
    public abstract class ResultNode<T1> : FunctionNode {
        
        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is the label text for the first result value.
        /// </summary>
        [SerializeField,HideInInspector] private string label1 = "result 1";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool IsResultNode => true;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the first result value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The first result value.</returns>
        public T1 GetResult1(CalculationId id) => TryGetPortValue(0, id, out T1 value) ? value : default;

        /// <summary>
        /// This method is used to set the labels for the result node.
        /// </summary>
        /// <param name="label1">The label for the first value.</param>
        public void SetLabel(string label1) {
            this.label1 = label1;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR

        /// <inheritdoc />
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
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Other Methods //////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<T1>(label1));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
    [FunctionNode("This node is the result node for the function",true,false)]
    public abstract class ResultNode<T1,T2> : FunctionNode {
        
        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is the label text for the first result value.
        /// </summary>
        [SerializeField,HideInInspector] private string label1 = "result 1";
        
        /// <summary>
        /// This is the label text for the second result value.
        /// </summary>
        [SerializeField,HideInInspector] private string label2 = "result 2";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool IsResultNode => true;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the first result value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The first result value.</returns>
        public T1 GetResult1(CalculationId id) => TryGetPortValue(0, id, out T1 value) ? value : default;
        
        /// <summary>
        /// This method is used to get the second result value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The second result value.</returns>
        public T2 GetResult2(CalculationId id) => TryGetPortValue(1, id, out T2 value) ? value : default;

        /// <summary>
        /// This method is used to set the labels for the result node.
        /// </summary>
        /// <param name="label1">The label for the first value.</param>
        /// <param name="label2">The label for the second value.</param>
        public void SetLabel(string label1, string label2) {
            this.label1 = label1;
            this.label2 = label2;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR

        /// <inheritdoc />
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
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Other Methods //////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<T1>(label1));
            inputPorts.Add(new PortInfo<T2>(label2));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
    [FunctionNode("This node is the result node for the function",true,false)]
    public abstract class ResultNode<T1,T2,T3> : FunctionNode {
        
        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is the label text for the first result value.
        /// </summary>
        [SerializeField,HideInInspector] private string label1 = "result 1";
        
        /// <summary>
        /// This is the label text for the second result value.
        /// </summary>
        [SerializeField,HideInInspector] private string label2 = "result 2";
        
        /// <summary>
        /// This is the label text for the third result value.
        /// </summary>
        [SerializeField,HideInInspector] private string label3 = "result 3";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool IsResultNode => true;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the first result value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The first result value.</returns>
        public T1 GetResult1(CalculationId id) => TryGetPortValue(0, id, out T1 value) ? value : default;
        
        /// <summary>
        /// This method is used to get the second result value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The second result value.</returns>
        public T2 GetResult2(CalculationId id) => TryGetPortValue(1, id, out T2 value) ? value : default;
        
        /// <summary>
        /// This method is used to get the third result value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The third result value.</returns>
        public T3 GetResult3(CalculationId id) => TryGetPortValue(2, id, out T3 value) ? value : default;

        /// <summary>
        /// This method is used to set the labels for the result node.
        /// </summary>
        /// <param name="label1">The label for the first value.</param>
        /// <param name="label2">The label for the second value.</param>
        /// <param name="label3">The label for the third value.</param>
        public void SetLabel(string label1, string label2, string label3) {
            this.label1 = label1;
            this.label2 = label2;
            this.label3 = label3;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR

        /// <inheritdoc />
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
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Other Methods //////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<T1>(label1));
            inputPorts.Add(new PortInfo<T2>(label2));
            inputPorts.Add(new PortInfo<T3>(label3));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
    [FunctionNode("This node is the result node for the function",true,false)]
    public abstract class ResultNode<T1,T2,T3,T4> : FunctionNode {
        
        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is the label text for the first result value.
        /// </summary>
        [SerializeField,HideInInspector] private string label1 = "result 1";
        
        /// <summary>
        /// This is the label text for the second result value.
        /// </summary>
        [SerializeField,HideInInspector] private string label2 = "result 2";
        
        /// <summary>
        /// This is the label text for the third result value.
        /// </summary>
        [SerializeField,HideInInspector] private string label3 = "result 3";
        
        /// <summary>
        /// This is the label text for the fourth result value.
        /// </summary>
        [SerializeField,HideInInspector] private string label4 = "result 4";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool IsResultNode => true;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the first result value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The first result value.</returns>
        public T1 GetResult1(CalculationId id) => TryGetPortValue(0, id, out T1 value) ? value : default;
        
        /// <summary>
        /// This method is used to get the second result value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The second result value.</returns>
        public T2 GetResult2(CalculationId id) => TryGetPortValue(1, id, out T2 value) ? value : default;
        
        /// <summary>
        /// This method is used to get the third result value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The third result value.</returns>
        public T3 GetResult3(CalculationId id) => TryGetPortValue(2, id, out T3 value) ? value : default;

        /// <summary>
        /// This method is used to get the fourth result value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The fourth result value.</returns>
        public T4 GetResult4(CalculationId id) => TryGetPortValue(3, id, out T4 value) ? value : default;

        /// <summary>
        /// This method is used to set the labels for the result node.
        /// </summary>
        /// <param name="label1">The label for the first value.</param>
        /// <param name="label2">The label for the second value.</param>
        /// <param name="label3">The label for the third value.</param>
        /// <param name="label4">The label for the fourth value.</param>
        public void SetLabel(string label1, string label2, string label3, string label4) {
            this.label1 = label1;
            this.label2 = label2;
            this.label3 = label3;
            this.label4 = label4;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR

        /// <inheritdoc />
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
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Other Methods //////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<T1>(label1));
            inputPorts.Add(new PortInfo<T2>(label2));
            inputPorts.Add(new PortInfo<T3>(label3));
            inputPorts.Add(new PortInfo<T4>(label4));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
    [FunctionNode("This node is the result node for the function",true,false)]
    public abstract class ResultNode<T1,T2,T3,T4,T5> : FunctionNode {
        
        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is the label text for the first result value.
        /// </summary>
        [SerializeField,HideInInspector] private string label1 = "result 1";
        
        /// <summary>
        /// This is the label text for the second result value.
        /// </summary>
        [SerializeField,HideInInspector] private string label2 = "result 2";
        
        /// <summary>
        /// This is the label text for the third result value.
        /// </summary>
        [SerializeField,HideInInspector] private string label3 = "result 3";
        
        /// <summary>
        /// This is the label text for the fourth result value.
        /// </summary>
        [SerializeField,HideInInspector] private string label4 = "result 4";
        
        /// <summary>
        /// This is the label text for the fifth result value.
        /// </summary>
        [SerializeField,HideInInspector] private string label5 = "result 5";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool IsResultNode => true;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the first result value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The first result value.</returns>
        public T1 GetResult1(CalculationId id) => TryGetPortValue(0, id, out T1 value) ? value : default;
        
        /// <summary>
        /// This method is used to get the second result value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The second result value.</returns>
        public T2 GetResult2(CalculationId id) => TryGetPortValue(1, id, out T2 value) ? value : default;
        
        /// <summary>
        /// This method is used to get the third result value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The third result value.</returns>
        public T3 GetResult3(CalculationId id) => TryGetPortValue(2, id, out T3 value) ? value : default;

        /// <summary>
        /// This method is used to get the fourth result value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The fourth result value.</returns>
        public T4 GetResult4(CalculationId id) => TryGetPortValue(3, id, out T4 value) ? value : default;
        
        /// <summary>
        /// This method is used to get the fifth result value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The fifth result value.</returns>
        public T5 GetResult5(CalculationId id) => TryGetPortValue(4, id, out T5 value) ? value : default;

        /// <summary>
        /// This method is used to set the labels for the result node.
        /// </summary>
        /// <param name="label1">The label for the first value.</param>
        /// <param name="label2">The label for the second value.</param>
        /// <param name="label3">The label for the third value.</param>
        /// <param name="label4">The label for the fourth value.</param>
        /// <param name="label5">The label for the fifth value.</param>
        public void SetLabel(string label1, string label2, string label3, string label4, string label5) {
            this.label1 = label1;
            this.label2 = label2;
            this.label3 = label3;
            this.label4 = label4;
            this.label4 = label5;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR

        /// <inheritdoc />
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
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Other Methods //////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<T1>(label1));
            inputPorts.Add(new PortInfo<T2>(label2));
            inputPorts.Add(new PortInfo<T3>(label3));
            inputPorts.Add(new PortInfo<T4>(label4));
            inputPorts.Add(new PortInfo<T5>(label5));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}