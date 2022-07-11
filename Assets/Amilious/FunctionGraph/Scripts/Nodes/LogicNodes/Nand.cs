using System.Linq;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.LogicNodes {
    
    /// <summary>
    /// This node is used to check if only one value is false.
    /// </summary>
    [FunctionNode("This node is used to check if only one value is false.")]
    public class Nand : LogicNodes {
        
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
            inputPorts.Add(new PortInfo<bool>("values",true));
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
            TryGetPortValues<bool>(0, id, out var values);
            if(values == null || values.Count == 0) return _lastValue = true;
            return _lastValue = values.Count(x=>!x)==1;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}