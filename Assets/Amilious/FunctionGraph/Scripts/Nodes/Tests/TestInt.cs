using System.Globalization;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Tests {
    
    /// <summary>
    /// This node is used to test a int value at the given part of your function.
    /// </summary>
    [FunctionNode("This node is used to test a int value at the given part of your function.")]
    public class TestInt : TestNodes {

        #region Non-Serialized Fields //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This last calculation id.
        /// </summary>
        private CalculationId _lastId;
        
        /// <summary>
        /// The last cached value.
        /// </summary>
        private int _lastValue;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private & Protected Methods ////////////////////////////////////////////////////////////////////////////
                                            
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<int>("input"));
            outputPorts.Add( new PortInfo<int>("output", GetInput));
        }

        /// <summary>
        /// This method is used to calculate the value.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The calculated value.</returns>
        private int GetInput(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out int value);
            _lastValue = value;
            #if UNITY_EDITOR
            SetLabel(id,_lastValue.ToString(CultureInfo.InvariantCulture));
            #endif
            return _lastValue;
        }

        /// <inheritdoc />
        protected override void TestValue(CalculationId id) => GetInput(id);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}