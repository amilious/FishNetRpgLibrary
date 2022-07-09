using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using PlasticPipe.PlasticProtocol.Messages;

namespace Amilious.FunctionGraph.Nodes.Loops {
    
    [FunctionNode("This node is used as the starting point of a loop.")]
    public class StartLoop : LoopNodes {

        [SerializeField] private FunctionNode loopNode;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<float>("value",GetValue).MarkLoop());
            outputPorts.Add(new PortInfo<int>("index",GetIndex).MarkLoop());
        }

        private bool? _isConnectedToLoop;
        private ILoopSource _lastSource = null;

        public ILoopSource GetSource() {
            //get cached source
            if(_isConnectedToLoop.HasValue) return _isConnectedToLoop.Value?_lastSource:null;
            //calculate source
            if(!HasOutputConnectionToLoop(out var loop)) {
                _isConnectedToLoop = false;
                return null;
            }
            if(loop is ILoopSource source) {
                _isConnectedToLoop = true;
                return _lastSource = source;
            }
            _isConnectedToLoop = false;
            return null;
        }

        private CalculationId _lastId;
        private int _lastIndex;
        private float _lastValue;
        
        private void GetValues(CalculationId id) {
            if(_lastId == id) return;
            _lastId = id;
            var source = GetSource();
            _lastIndex = source?.CurrentIndex ?? default;
            _lastValue = source?.CurrentValue ?? default;
        }

        private int GetIndex(CalculationId id) {
            GetValues(id);
            return _lastIndex;
        }

        private float GetValue(CalculationId id) {
            GetValues(id);
            return _lastValue;
        }

    }
}