using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Manipulators {
    
    /// <summary>
    /// This node is used to calculate the shortest difference between the two given angles.
    /// </summary>
    [FunctionNode("This node is used to calculate the shortest difference between the two given angles.")]
    public class DeltaAngle : ManipulatorNodes {
        
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
            inputPorts.Add(new PortInfo<float>("angle(deg) 1"));
            inputPorts.Add(new PortInfo<float>("angle(deg) 2"));
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
            TryGetPortValue(0, id, out float a);
            TryGetPortValue(0, id, out float b);
            return _lastValue = Mathf.DeltaAngle(a, b);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}