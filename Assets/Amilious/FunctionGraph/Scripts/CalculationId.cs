using System;
using System.Collections.Generic;

namespace Amilious.FunctionGraph {
    
    [Serializable]
    public class CalculationId {

        private static long _idPool = 1;
        private static object _poolLock =  new object();

        protected static long GetNextId() {
            lock(_poolLock) {
                var id = _idPool;
                _idPool++;
            }
            return _idPool;
        }

        public long Id { get; }
        
        public CalculationId() {
            Id = GetNextId();
        }
        
        

    }
    
}