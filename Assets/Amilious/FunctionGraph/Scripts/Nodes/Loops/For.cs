using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Loops {
    
    [FunctionNode("This node is used to act as a for loop.")]
    public class For : LoopNodes {

        private bool _running;
        private CalculationId _lastId;
        private float _lastValue;
        private float _startValue;
        private int _startIndex;
        private int _lastIndex;
        private int _currentIndex;
        private float _currentValue;

        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("start value"));
            inputPorts.Add(new PortInfo<int>("start index"));
            inputPorts.Add(new PortInfo<int>("last index"));
            inputPorts.Add(new PortInfo<float>("end loop"));
            outputPorts.Add(new PortInfo<float>("result",GetResult));
        }

        private float GetResult(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out _startValue);
            TryGetPortValue(1, id, out _startIndex);
            TryGetPortValue(2, id, out _lastIndex);
            if(_lastIndex < _startIndex) return _lastValue = _startValue;
            _currentIndex = _startIndex;
            //start pulling
            _currentValue = _startValue;
            
            //loop
            while(_currentIndex<=_lastIndex){
                //increment the index
                _currentIndex++;
                // need to generate new id so we do not get cached values
                var pullId = new CalculationId();
                if(!TryGetPortValue(3, pullId, out _currentValue))
                    //if the loop is not complete return the start value
                    return _lastValue = _startValue;
            }
            //end loop
            return _lastValue = _currentValue;
        }
        
        public int GetIndex(CalculationId id) => _currentIndex;

        
        /// This is the start of the loop
        public float GetLoop(CalculationId arg) => _currentValue;
        
        
        
    }
}