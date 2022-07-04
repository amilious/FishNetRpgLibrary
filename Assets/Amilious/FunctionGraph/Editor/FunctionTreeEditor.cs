using System.Runtime.Remoting.Messaging;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Amilious.FunctionGraph.Editor {
    public class FunctionTreeEditor : EditorWindow {

        public IFunctionProvider Provider { get; private set; }
        public ScriptableObject ProviderScriptableObject { get; private set; }
        
        private FunctionTreeView treeView;
        private InspectorView inspectorView;
    
        [MenuItem("Amilious/FunctionGraphEditor")]
        public static void OpenWindow() {
            var window = GetWindow<FunctionTreeEditor>(false,"Function Graph Editor");
            if(window!=null)window.OnSelectionChange();
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Amilious/FunctionGraph/Editor/FunctionTreeEditor.uxml");
            visualTree.CloneTree(root);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Amilious/FunctionGraph/Editor/FunctionTreeEditor.uss");
            root.styleSheets.Add(styleSheet);
        
            //get views
            treeView = root.Q<FunctionTreeView>();
            treeView.OnNodeSelected = OnNodeSelectionChanged;
            treeView.OnNodeUnselected = OnNodeUnselected;
            inspectorView = root.Q<InspectorView>();
        
            OnSelectionChange();
        }

        protected void OnSelectionChange() {
            if(Selection.activeObject is not (IFunctionProvider provider and ScriptableObject so)) return;
            if(!AssetDatabase.CanOpenAssetInEditor(so.GetInstanceID())) return;
            Provider = provider;
            ProviderScriptableObject = so;
            treeView.PopulateView(Provider);
        }
        
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceId, int line) {
            if(EditorUtility.InstanceIDToObject(instanceId) is not IFunctionProvider provider) return false;
            OpenWindow();
            return true;
        }


        private void OnNodeSelectionChanged(FunctionNodeView nodeView) {
            inspectorView.UpdateSelection(nodeView);
        }

        private void OnNodeUnselected(FunctionNodeView nodeView) {
        
        }
    
    }
}