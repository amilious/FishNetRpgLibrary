using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Loops {
    
    [FunctionNode("This node is used to act as a for loop.")]
    public class WhileLoop : LoopNodes, ILoopSource {

        private bool _running;
        private CalculationId _lastId;
        private float _lastValue;
        private float _startValue;
        private int _startIndex;
        private int _lastIndex;
        private int _maxLoops;
        private int _currentIndex;
        private float _currentValue;

        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("start value"));
            inputPorts.Add(new PortInfo<int>("max loops"));
            inputPorts.Add(new PortInfo<bool>("condition").MarkLoop());
            inputPorts.Add(new PortInfo<float>("end loop").MarkLoop());
            outputPorts.Add(new PortInfo<float>("result",GetResult));
        }

        private float GetResult(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out _startValue);
            TryGetPortValue(1, id, out _maxLoops);
            _currentIndex = 0;
            //start pulling
            _currentValue = _startValue;
            
            //loop
            while(_currentIndex<=_maxLoops){
                //increment the index
                _currentIndex++;
                // need to generate new id so we do not get cached values
                var pullId = new CalculationId();
                TryGetPortValue(2, pullId, out bool condition);
                if(!condition) { return _lastValue = _currentValue; }
                if(!TryGetPortValue(3, pullId, out float pullValue))
                    //if the loop is not complete return the start value
                    return _lastValue = _startValue;
                _currentValue = pullValue;
            }
            //end loop
            return _lastValue = _currentValue;
        }

        public int CurrentIndex => _currentIndex;
        public float CurrentValue => _currentValue;
        
    }
}