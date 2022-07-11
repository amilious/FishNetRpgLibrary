using System.Collections;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    /// <summary>
    /// This node is used to spit an integer into bool representations of it's bits.
    /// </summary>
    [FunctionNode("This node is used to spit an integer into bool representations of it's bits.")]
    public class SplitInt : ConvertingNodes {
        
        #region Non-Serialized Feilds //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to store the last calculation id.
        /// </summary>
        private CalculationId _lastId;
        
        /// <summary>
        /// This field is used to store the last calculated value.
        /// </summary>
        private readonly bool[] _lastValue = new bool[32];
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<int>("value"));
            for(var i =0;i<32;i++) {
                var index = i;
                outputPorts.Add(new PortInfo<bool>(i.ToString(),id=>GetValue(id,index)));
            }
        }

        /// <summary>
        /// This method is used to get the values of this nodes output ports.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <param name="index">The index of the port.</param>
        /// <returns>The value.</returns>
        private bool GetValue(CalculationId id, int index) {
            if(index is < 0 or >= 32) return false;
            if(_lastId == id) return _lastValue[index];
            //calculate bools
            TryGetPortValue(0, id, out int value);
            var bits = new BitArray(new[] { value });
            bits.CopyTo(_lastValue, 0);
            return _lastValue[index];
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}