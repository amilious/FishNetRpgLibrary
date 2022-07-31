/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                    //
//    _____            .__ .__   .__                             _________  __              .___.__                   //
//   /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /   _____/_/  |_  __ __   __| _/|__|  ____   ______   //
//  /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     \_____  \ \   __\|  |  \ / __ | |  | /  _ \ /  ___/   //
// /    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \      /        \ |  |  |  |  // /_/ | |  |(  <_> )\___ \    //
// \____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    /_______  / |__|  |____/ \____ | |__| \____//____  >   //
//         \/       \/                                  \/             \/                    \/                 \/    //
//                                                                                                                    //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious,com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Loops {
    
    [FunctionNode("This node is used as the starting point of a loop.")]
    public class StartLoop : LoopNodes {

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