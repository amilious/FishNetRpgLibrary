using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using UnityEngine;

namespace Amilious.FunctionGraph.Nodes.Loops {
    
    [FunctionNode("This node is used to act as a for loop.")]
    public class ForLoop : LoopNodes, ILoopSource {

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
            inputPorts.Add(new PortInfo<float>("end loop").MarkLoop());
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
            _currentValue = _startValue;
            
            //loop
            while(_currentIndex<=_lastIndex){
                //increment the index
                _currentIndex++;
                // need to generate new id so we do not get cached values
                var pullId = new CalculationId();
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