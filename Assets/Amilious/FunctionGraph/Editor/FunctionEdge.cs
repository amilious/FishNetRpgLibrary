using System;
using UnityEditor.Experimental.GraphView;

namespace Amilious.FunctionGraph.Editor {
    
    public class FunctionEdge : Edge {

        public static event Action<GraphElement> OnSelect;
        public static event Action<GraphElement> OnUnselect;
        
        public override void OnSelected() {
            base.OnSelected();
            OnSelect?.Invoke(this);
        }

        public override void OnUnselected() {
            base.OnUnselected();
            OnUnselect?.Invoke(this);
        }
        
    }
    
}