using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Repeaters {
    
    /// <summary>
    /// This node is used to to give the bool input value as an output.
    /// </summary>
    [FunctionNode("This node is used to to give the bool input value as an output.")]
    public class BoolRepeater : RepeaterNodes {
        
        #region Private & Protected Methods ////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<bool>(""));
            outputPorts.Add(new PortInfo<bool>("",GetValue));
        }
        
        /// <summary>
        /// This method is used to repeat the input port as the output port.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The same value as the input port.</returns>
        private bool GetValue(CalculationId id) {
            TryGetPortValue(0, id, out bool value);
            return value;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
    }
}