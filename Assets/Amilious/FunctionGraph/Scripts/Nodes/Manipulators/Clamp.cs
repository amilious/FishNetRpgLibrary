using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Manipulators {
    
    /// <summary>
    /// This node is used to clamp the given input between the given minimum and maximum inclusively.
    /// </summary>
    [FunctionNode("This node is used to clamp the given input between the given minimum and maximum inclusively.")]
    public class Clamp : ManipulatorNodes {
        
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
            inputPorts.Add(new PortInfo<float>("value"));
            inputPorts.Add(new PortInfo<float>("minimum"));
            inputPorts.Add(new PortInfo<float>("maximum"));
            outputPorts.Add( new PortInfo<float>("result", GetValue));
        }

        /// <summary>
        /// This method is used to calculate the value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The calculated value.</returns>
        private float GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out float value);
            TryGetPortValue(0, id, out float min);
            TryGetPortValue(0, id, out float max);
            return _lastValue = Mathf.Clamp(value, min, max);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}