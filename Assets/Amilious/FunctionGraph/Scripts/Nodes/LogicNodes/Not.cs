using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.LogicNodes {
    
    /// <summary>
    /// This node is used to invert a bool.
    /// </summary>
    [FunctionNode("This node is used to invert a bool.")]
    public class Not : LogicNodes {
        
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
            inputPorts.Add(new PortInfo<bool>("value"));
            outputPorts.Add(new PortInfo<bool>("result", GetResult));
        }
        
        /// <summary>
        /// This method is used to calculate the value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The calculated value.</returns>
        private bool GetResult(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue<bool>(0, id, out var value);
            return _lastValue = !value;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}