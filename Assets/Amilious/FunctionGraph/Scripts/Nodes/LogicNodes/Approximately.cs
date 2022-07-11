using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.LogicNodes {
    
    /// <summary>
    /// This node is used to check if the given values are approximately equal.
    /// </summary>
    [FunctionNode("This node is used to check if the given values are approximately equal.")]
    public class Approximately : LogicNodes {
        
        #region Non-Serialized Fields //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This last calculation id.
        /// </summary>
        private CalculationId _lastId;
        
        /// <summary>
        /// The last cached value.
        /// </summary>
        private bool _lastValue;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Protected & Private Methods ////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("value 1"));
            inputPorts.Add(new PortInfo<float>("value 2"));
            outputPorts.Add(new PortInfo<bool>("result",GetResult));
        }
        
        /// <summary>
        /// This method is used to calculate the value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The calculated value.</returns>
        private bool GetResult(CalculationId id) {
            if(id == _lastId) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out float value1);
            TryGetPortValue(1, id, out float value2);
            return _lastValue = Mathf.Approximately(value1, value2);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
    }
}