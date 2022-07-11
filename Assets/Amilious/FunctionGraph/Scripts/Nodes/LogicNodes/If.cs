using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.LogicNodes {
    
    /// <summary>
    /// This node is used output a value based on the given bool.
    /// </summary>
    [FunctionNode("This node is used output a value based on the given bool.")]
    public class If : LogicNodes {
        
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
        
        #region Protected & Private Methods ////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<bool>("bool"));
            inputPorts.Add(new PortInfo<float>("true"));
            inputPorts.Add(new PortInfo<float>("false"));
            outputPorts.Add(new PortInfo<float>("result", GetResult));
        }

        /// <summary>
        /// This method is used to calculate the value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The calculated value.</returns>
        private float GetResult(CalculationId id) {
            if(id == _lastId) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out bool ifValue);
            TryGetPortValue(1, id, out float trueValue);
            TryGetPortValue(2, id, out float falseValue);
            return _lastValue = ifValue?trueValue:falseValue;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}