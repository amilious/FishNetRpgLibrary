namespace Amilious.FunctionGraph.Nodes.Loops {
    public interface ILoopSource {
        
        public int CurrentIndex { get; }

        public float CurrentValue { get; }
    }
}