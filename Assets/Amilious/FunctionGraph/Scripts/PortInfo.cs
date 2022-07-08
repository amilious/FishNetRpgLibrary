using System;

namespace Amilious.FunctionGraph {

    public interface IPortInfo {
        public Type Type { get; }
        public string Name { get; }
        public bool AllowMultiple { get; }
        public bool Horizontal { get; }
        string Tooltip { get; }
        public int Index { get; }
        public void SetIndex(int index);
        
        public bool IsLoopPort { get; }
        
        public bool TryGetResult<T>(CalculationId id, out T result) {
            if(this is PortInfo<T> casted) {
                var function = casted.OutputFunction;
                if(function == null) {
                    result = default(T);
                    return false;
                }
                result = function.Invoke(id);
                return true;
            }else {
                result = default(T);
                return false;
            }
        }
        
    }
    
    public class PortInfo<T> : IPortInfo {
        public Type Type { get; }
        public string Name { get; }
        public bool AllowMultiple { get; }
        public bool Horizontal { get; }
        public string Tooltip { get; set; }
        
        public int Index { get; private set; }

        public void SetIndex(int index) => Index = index;
        public bool IsLoopPort { get; private set; }

        public PortInfo<T> SetTooltip(string tooltip) { 
            Tooltip = tooltip;
            return this;
        }

        public PortInfo<T> MarkLoop() {
            IsLoopPort = true;
            return this;
        }

        public Func<CalculationId,T> OutputFunction { get; }

        public PortInfo(string name, bool allowMultiple = false, bool horizontal = true) {
            Type = typeof(T);
            Name = name;
            AllowMultiple = allowMultiple;
            Horizontal = horizontal;
            OutputFunction = null;
        }

        public PortInfo(string name,Func<CalculationId,T> outputFunction, bool allowMultiple = true, bool horizontal = true) {
            Type = typeof(T);
            Name = name;
            AllowMultiple = allowMultiple;
            Horizontal = horizontal;
            OutputFunction = outputFunction;
        }
        
    }

    
    
}