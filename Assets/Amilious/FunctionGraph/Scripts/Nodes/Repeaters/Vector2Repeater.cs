using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Repeaters {
    
    /// <summary>
    /// This node is used to to give the Vector2 input value as an output.
    /// </summary>
    [FunctionNode("This node is used to to give the Vector2 input value as an output.")]
    public class Vector2Repeater : RepeaterNodes {
        
        #region Private & Protected Methods ////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<Vector2>(""));
            outputPorts.Add(new PortInfo<Vector2>("",GetValue));
        }
        
        /// <summary>
        /// This method is used to repeat the input port as the output port.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The same value as the input port.</returns>
        private Vector2 GetValue(CalculationId id) {
            TryGetPortValue(0, id, out Vector2 value);
            return value;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}