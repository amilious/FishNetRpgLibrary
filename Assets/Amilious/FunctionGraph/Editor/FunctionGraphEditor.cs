using UnityEngine;
using System.Linq;
using UnityEditor;
using Amilious.Core;
using UnityEditor.Callbacks;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using UnityEditor.Experimental.GraphView;

namespace Amilious.FunctionGraph.Editor {
    public class FunctionGraphEditor : EditorWindow {

        public static IFunctionProvider Provider { get; private set; }
        public static AmiliousScriptableObject ProviderScriptableObject { get; private set; }

        public static FunctionGraphEditor Instance;
        
        private FunctionGraphView _graphView;
        private FunctionNodeInspectorView _functionNodeInspectorView;
        private Label _functionTitle;
        private Label _inspectorTitle;
        private Button _clearUnconnected;
        private Button _focusInput;
        private Button _focusResult;
        private Label _selectedGroups;
        private Label _selectedConnections;
        private Label _selectedNodes;
        private Label _totalNodes;
        private Label _totalConnections;
        private Label _totalGroups;
        private Label _scale;
        private Label _position;
        
        [MenuItem("Amilious/FunctionGraphEditor")]
        public static void OpenWindow() {
            var window = GetWindow<FunctionGraphEditor>(false,"Function Graph Editor");
            if(window == null) return;
            Instance = window;
            window.OnSelectionChange();
        }


        public void CreateGUI() {
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
            _graphView.OnSelectionChanged += OnGraphSelectionChanged;
            _graphView.viewTransformChanged += ViewChanged;
            _graphView.OnCountUpdated += CountUpdated;
            _functionNodeInspectorView = root.Q<FunctionNodeInspectorView>();
            _functionTitle = root.Q<Label>("functionTitle");
            _inspectorTitle = root.Q<Label>("inspectorTitle");
            _clearUnconnected = root.Q<Button>("clearUnconnected");
            _focusInput = root.Q<Button>("focusInput");
            _focusResult = root.Q<Button>("focusResult");
            _clearUnconnected.clicked += ClearUnconnected;
            _focusInput.clicked += FocusInput;
            _focusResult.clicked += FocusResult;
            _selectedGroups = root.Q<Label>("selectedGroups");
            _selectedConnections = root.Q<Label>("selectedConnections");
            _selectedNodes = root.Q<Label>("selectedNodes");
            _totalGroups = root.Q<Label>("totalGroups");
            _totalConnections = root.Q<Label>("totalConnections");
            _totalNodes = root.Q<Label>("totalNodes");
            _scale = root.Q<Label>("scale");
            _position = root.Q<Label>("position");
            ResetTitles();
            OnSelectionChange();
            OnGraphSelectionChanged(_graphView.selection);
            ViewChanged(_graphView);
            _graphView.TriggerCountUpdate();
        }

        private void FocusResult() => _graphView?.FocusResult();

        private void FocusInput() => _graphView?.FocusInput();

        private void CountUpdated(int nodes, int connections, int groups) {
            if(_totalNodes!=null)_totalNodes.text = nodes.ToString();
            if(_totalConnections!=null)_totalConnections.text = connections.ToString();
            if(_totalGroups!=null)_totalGroups.text = groups.ToString();
        }

        private void ClearUnconnected() => _graphView.ClearUnconnected();

        private void ViewChanged(GraphView graphview) {
            var pos = graphview.viewTransform.position;
            _position.text = $"{pos.x}x{pos.y}";
            var scale = graphview.viewTransform.scale;
            _scale.text = $"{scale.x}";
        }

        private void OnGraphSelectionChanged(IReadOnlyList<ISelectable> obj) {
            var nodes = obj.Count(x => x is FunctionNodeView);
            _selectedNodes.text = nodes.ToString();
            _selectedConnections.text = obj.Count(x => x is Edge).ToString();
            _selectedGroups.text = obj.Count(x => x is Group).ToString();
            if(nodes == 0) return;
            _functionNodeInspectorView.UpdateSelection(obj.First(x=>x is FunctionNodeView) as FunctionNodeView);
            ResetTitles();
        }

        private void ResetTitles(string newName = null) {
            if(_functionTitle == null || _inspectorTitle == null) return;
            if(ProviderScriptableObject != null) {
                newName ??= ProviderScriptableObject.name;
                _functionTitle.text = "<b>Function Node:</b> " +
                                      $"(<color=#3a86ff>{ProviderScriptableObject.GetType().Name}</color>) " +
                                      $"<color=#8338ec>{newName}</color>";
            }else {
                _functionTitle.text = "<b>Function Node:</b> <i>No Loaded Function Provider</i>";
            }
            _inspectorTitle.text = _functionNodeInspectorView?.SelectedNode?.Node == null ? 
                "<b>Selected Node:</b> <i>No Node Selected</i>" : 
                $"<b>Selected Node:</b> <color=#8338ec>{_functionNodeInspectorView.SelectedNode.Node.name}</color>";
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
            if(Instance == null) return;
            Instance._functionNodeInspectorView.Reset();
            Instance._graphView.Reset();
            Instance.ResetTitles();
        }
        
        public static void AssetBeingRenamed(string guid, string newName) {
            if(ProviderScriptableObject == null) return;
            if(ProviderScriptableObject.AssetGuid != guid) return;
            if(Instance!=null)Instance.ResetTitles(newName);
        }

    }
}