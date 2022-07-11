using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    /// <summary>
    /// This node is used to split a Vector3Int into three ints.
    /// </summary>
    [FunctionNode("This node is used to split a Vector3Int into three ints.")]
    public class SplitVector3Int : ConvertingNodes {

        #region Non-Serialized Feilds //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to store the last calculation id.
        /// </summary>
        private CalculationId _lastId;
        
        /// <summary>
        /// This field is used to store the last calculated value.
        /// </summary>
        private Vector3Int _lastValue;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<Vector3Int>("value"));
            outputPorts.Add( new PortInfo<int>("x",GetX));
            outputPorts.Add( new PortInfo<int>("y",GetY));
            outputPorts.Add( new PortInfo<int>("z",GetZ));
        }

        /// <summary>
        /// This method is used to get the values of this nodes output ports.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The value.</returns>
        private Vector3Int GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out Vector3Int value);
            return _lastValue = value;
        }

        /// <summary>
        /// This method is used to get the value of this nodes third output port.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The value.</returns>
        private int GetZ(CalculationId id) => GetValue(id).z;

        /// <summary>
        /// This method is used to get the value of this nodes second output port.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The value.</returns>
        private int GetY(CalculationId id) => GetValue(id).y;

        /// <summary>
        /// This method is used to get the value of this nodes first output port.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The value.</returns>
        private int GetX(CalculationId id) => GetValue(id).x;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}