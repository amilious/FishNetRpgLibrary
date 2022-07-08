using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Amilious.FunctionGraph.Editor {
    
    public class InspectorView : VisualElement {
        
        public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { }

        public UnityEditor.Editor Editor { get; private set; }
        public IMGUIContainer EditorContainer { get; private set; }
            
        public FunctionNodeView SelectedNode { get; private set; }    
        
        public InspectorView() { }

        public void UpdateSelection(FunctionNodeView nodeView) {
            Reset();
            SelectedNode = nodeView;
            Editor = UnityEditor.Editor.CreateEditor(nodeView.Node);
            EditorContainer = new IMGUIContainer(Editor.OnInspectorGUI);
            Add(EditorContainer);
        }


        public void Reset() {
            SelectedNode = null;
            Clear();
            if(Editor != null) Object.DestroyImmediate(Editor);
        }
    }
    
}