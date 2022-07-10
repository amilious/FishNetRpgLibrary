using UnityEngine;
using UnityEngine.UIElements;

namespace Amilious.FunctionGraph.Editor {
    
    /// <summary>
    /// This class is used to display an inspector for <see cref="FunctionNodeView"/>'s.
    /// </summary>
    public class FunctionNodeInspectorView : VisualElement {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to hold the editor.
        /// </summary>
        private UnityEditor.Editor _editor;
        
        /// <summary>
        /// This field is used to hold the imgui container.
        /// </summary>
        private IMGUIContainer _editorContainer;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region UXML Factory ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This factory allows us to use the inspector view with the UI Builder.
        /// </summary>
        public new class UxmlFactory : UxmlFactory<FunctionNodeInspectorView, UxmlTraits> { }

        #endregion
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is a reference to the currently display
        /// </summary>
        public FunctionNodeView SelectedNode { get; private set; }    
        
        #endregion
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to display the given <see cref="FunctionNodeView"/> in the inspector.
        /// </summary>
        /// <param name="nodeView">The node that you want to display.</param>
        public void UpdateSelection(FunctionNodeView nodeView) {
            Reset();
            SelectedNode = nodeView;
            _editor = UnityEditor.Editor.CreateEditor(nodeView.Node);
            _editorContainer = new IMGUIContainer(_editor.OnInspectorGUI);
            Add(_editorContainer);
        }

        /// <summary>
        /// This method is used to clear the inspector.
        /// </summary>
        public void Reset() {
            SelectedNode = null;
            Clear();
            if(_editor != null) Object.DestroyImmediate(_editor);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}