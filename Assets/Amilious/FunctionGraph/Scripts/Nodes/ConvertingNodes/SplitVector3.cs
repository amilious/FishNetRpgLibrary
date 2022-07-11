using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    [FunctionNode("This node is used to split a Vector3 into three floats.")]
    public class SplitVector3 : ConvertingNodes {

        #region Non-Serialized Feilds //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to store the last calculation id.
        /// </summary>
        private CalculationId _lastId;
        
        /// <summary>
        /// This field is used to store the last calculated value.
        /// </summary>
        private Vector3 _lastValue;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<Vector3>("Vector3"));
            outputPorts.Add(new PortInfo<float>("x",GetXValue));
            outputPorts.Add(new PortInfo<float>("y",GetYValue));
            outputPorts.Add(new PortInfo<float>("z",GetZValue));
        }

        /// <summary>
        /// This method is used to get the values of this nodes output ports.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The value.</returns>
        private Vector3 GetVector3(CalculationId id) {
            if(_lastId == id) return _lastValue;
            TryGetPortValue(0, id, out _lastValue);
            return _lastValue;
        }

        /// <summary>
        /// This method is used to get the value of this nodes third output port.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The value.</returns>
        private float GetZValue(CalculationId id) => GetVector3(id).z;

        /// <summary>
        /// This method is used to get the value of this nodes second output port.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The value.</returns>
        private float GetYValue(CalculationId id) => GetVector3(id).y;

        /// <summary>
        /// This method is used to get the value of this nodes first output port.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The value.</returns>
        private float GetXValue(CalculationId id) => GetVector3(id).x;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}