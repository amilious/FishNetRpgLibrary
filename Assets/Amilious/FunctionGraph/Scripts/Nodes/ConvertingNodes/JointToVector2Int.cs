using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    /// <summary>
    /// This node is used to join two int values into a Vector2Int.
    /// </summary>
    [FunctionNode("This node is used to join two int values into a Vector2Int.")]
    public class JointToVector2Int : ConvertingNodes {

        #region Non-Serialized Feilds //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to store the last calculation id.
        /// </summary>
        private CalculationId _lastId;
        
        /// <summary>
        /// This field is used to store the last calculated value.
        /// </summary>
        private Vector2Int _lastValue;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<int>("x"));
            inputPorts.Add(new PortInfo<int>("y"));
            outputPorts.Add(new PortInfo<Vector2Int>("result",GetValue));
        }

        /// <summary>
        /// This method is used to get the value of this nodes first output port.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The value.</returns>
        private Vector2Int GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out int x);
            TryGetPortValue(1, id, out int y);
            return _lastValue = new Vector2Int(x, y);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}