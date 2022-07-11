using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    /// <summary>
    /// This node is used for converting a integer to a float.
    /// </summary>
    [FunctionNode("This node is used for converting a integer to a float.")]
    public class IntToFloat : ConvertingNodes {
        
        #region Non-Serialized Feilds //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to store the last calculation id.
        /// </summary>
        private CalculationId _lastAction;
        
        /// <summary>
        /// This field is used to store the last calculated value.
        /// </summary>
        private float _lastResult;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<int>("int"));
            outputPorts.Add(new PortInfo<float>("float", GetValue));
        }

        /// <summary>
        /// This method is used to get the value of this nodes first output port.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The value.</returns>
        private float GetValue(CalculationId id) {
            if(id == _lastAction) return _lastResult;
            _lastAction = id;
            TryGetPortValue(0, id, out int value);
            _lastResult = value;
            return _lastResult;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}