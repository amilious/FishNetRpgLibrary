using Amilious.Core;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Amilious.FunctionGraph.Editor {
    public class FunctionGraphEditor : EditorWindow {

        public static IFunctionProvider Provider { get; private set; }
        public static AmiliousScriptableObject ProviderScriptableObject { get; private set; }

        private static FunctionGraphEditor _instance;
        
        private FunctionGraphView _graphView;
        private InspectorView _inspectorView;
        private Label _functionTitle;
        private Label _inspectorTitle;
    
        [MenuItem("Amilious/FunctionGraphEditor")]
        public static void OpenWindow() {
            var window = GetWindow<FunctionGraphEditor>(false,"Function Graph Editor");
            if(window == null) return;
            _instance = window;
            window.OnSelectionChange();
        }


        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Amilious/FunctionGraph/Editor/FunctionGraphEditor.uxml");
            visualTree.CloneTree(root);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Amilious/FunctionGraph/Editor/FunctionGraphEditor.uss");
            root.styleSheets.Add(styleSheet);
        
            //get views
            _graphView = root.Q<FunctionGraphView>();
            _graphView.OnNodeSelected = OnNodeSelectionChanged;
            _graphView.OnNodeUnselected = OnNodeUnselected;
            _inspectorView = root.Q<InspectorView>();
            _functionTitle = root.Q<Label>("functionTitle");
            _inspectorTitle = root.Q<Label>("inspectorTitle");
            ResetTitles();
            OnSelectionChange();
        }

        private void ResetTitles() {
            if(_functionTitle == null || _inspectorTitle == null) return;
            if(ProviderScriptableObject != null) {
                _functionTitle.text = "<b>Function Node:</b> " +
                    $"(<color=#3a86ff>{ProviderScriptableObject.GetType().Name}</color>) " +
                    $"<color=#8338ec>{ProviderScriptableObject.name}</color>";
            }else {
                _functionTitle.text = "<b>Function Node:</b> <i>No Loaded Function Provider</i>";
            }
            _inspectorTitle.text = _inspectorView?.SelectedNode?.Node == null ? 
                "<b>Selected Node:</b> <i>No Node Selected</i>" : 
                $"<b>Selected Node:</b> <color=#8338ec>{_inspectorView.SelectedNode.Node.name}</color>";
        }

        protected void OnSelectionChange() => TryLoad(Selection.activeObject);

        public bool TryLoad(Object functionProvider) {
            //run checks on the object
            if(functionProvider is not IFunctionProvider provider) return false;
            if(functionProvider is not AmiliousScriptableObject amiliousScriptableObject) {
                Debug.LogWarningFormat(
                    "Trying to load IFunctionProvider \"{0}\" but it does not inherit from the required {1}!",
                    provider.GetType().Name, nameof(AmiliousScriptableObject));
                return false;
            }
            if(!AssetDatabase.CanOpenAssetInEditor(amiliousScriptableObject.GetInstanceID())) return false;
            //store the object
            Provider = provider;
            ProviderScriptableObject = amiliousScriptableObject;
            _graphView.PopulateView(Provider);
            ResetTitles();
            return true;
        }
        
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceId, int line) {
            if(EditorUtility.InstanceIDToObject(instanceId) is not IFunctionProvider) return false;
            OpenWindow();
            return true;
        }
        
        public static void AssetBeingDeleted(string guid) {
            if(ProviderScriptableObject == null) return;
            if(ProviderScriptableObject.AssetGuid != guid) return;
            //the currently loaded asset is being deleted.
            if(_instance == null) return;
            _instance._inspectorView.Reset();
            _instance._graphView.Reset();
            _instance.ResetTitles();
        }
        
        

        private void OnNodeSelectionChanged(FunctionNodeView nodeView) {
            _inspectorView.UpdateSelection(nodeView);
            ResetTitles();
        }

        private void OnNodeUnselected(FunctionNodeView nodeView) {
        
        }

        
    }
}