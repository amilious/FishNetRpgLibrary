using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.LogicNodes {
    
    /// <summary>
    /// This node is used to check if the value is greater than or equal to the test.
    /// </summary>
    [FunctionNode("This node is used to check if the value is greater than or equal to the test.")]
    public class GreaterThanOrEqual : LogicNodes {
        
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
            inputPorts.Add(new PortInfo<float>("value"));
            inputPorts.Add(new PortInfo<float>("test"));
            outputPorts.Add(new PortInfo<bool>("result", GetResult));
        }

        /// <summary>
        /// This method is used to calculate the value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The calculated value.</returns>
        private bool GetResult(CalculationId id) {
            if(id == _lastId) return _lastValue;
            _lastId = id;
            if(!TryGetPortValue<float>(0, id, out var value)) return _lastValue = false;
            if(!TryGetPortValue<float>(1, id, out var test)) return _lastValue = true;
            return _lastValue = value >= test;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
    
    }
}