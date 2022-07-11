using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Tests {
    
    /// <summary>
    /// This node is used to test a Vector3Int value at the given part of your function.
    /// </summary>
    [FunctionNode("This node is used to test a Vector3Int value at the given part of your function.")]
    public class TestVector3Int : TestNodes {

        #region Non-Serialized Fields //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This last calculation id.
        /// </summary>
        private CalculationId _lastId;
        
        /// <summary>
        /// The last cached value.
        /// </summary>
        private Vector3Int _lastValue;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private & Protected Methods ////////////////////////////////////////////////////////////////////////////
                                            
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<Vector3Int>("input"));
            outputPorts.Add( new PortInfo<Vector3Int>("output", GetInput));
        }

        /// <summary>
        /// This method is used to calculate the value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The calculated value.</returns>
        private Vector3Int GetInput(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out Vector3Int value);
            _lastValue = value;
            #if UNITY_EDITOR
            SetLabel(id,_lastValue.ToString());
            #endif
            return _lastValue;
        }

        /// <inheritdoc />
        protected override void TestValue(CalculationId id) => GetInput(id);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}