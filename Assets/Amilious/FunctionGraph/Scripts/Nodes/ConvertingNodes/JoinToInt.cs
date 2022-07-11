using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    /// <summary>
    /// This node is used to join bool representations of bits into an integer.
    /// </summary>
    [FunctionNode("This node is used to join bool representations of bits into an integer.")]
    public class JoinToInt : ConvertingNodes {

        #region Non-Serialized Feilds //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to store the last calculation id.
        /// </summary>
        private CalculationId _lastId;
        
        /// <summary>
        /// This field is used to store the last calculated value.
        /// </summary>
        private int _lastValue;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<int>("result", GetValue));
            for(var i=0;i<32;i++) inputPorts.Add(new PortInfo<bool>(i.ToString()));
        }

        /// <summary>
        /// This method is used to get the value of this nodes first output port.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The value.</returns>
        private int GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            var val = 0;
            for(var i = 0; i < 31; i++) {
                TryGetPortValue(i, id, out bool bit);
                val = bit ? val | (1 << i) : val & ~(1 << i);
            }
            return _lastValue = val;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
    }
}