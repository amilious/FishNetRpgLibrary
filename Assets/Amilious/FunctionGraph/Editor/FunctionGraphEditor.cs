using System;
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
    
    /// <summary>
    /// This class is used as an editor for <see cref="IFunctionProvider"/>'s.
    /// </summary>
    public class FunctionGraphEditor : EditorWindow {

        #region Fields /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field contains the graph view.
        /// </summary>
        private FunctionGraphView _graphView;
        
        /// <summary>
        /// This field contains the node inspector.
        /// </summary>
        private FunctionNodeInspectorView _functionNodeInspectorView;
        
        /// <summary>
        /// This label is used to display the function title.
        /// </summary>
        private Label _functionTitle;

        /// <summary>
        /// This label is used to display the selected node's title.
        /// </summary>
        private Label _inspectorTitle;
        
        /// <summary>
        /// This button is used to clear the unconnected nodes.
        /// </summary>
        private Button _clearUnconnected;
        
        /// <summary>
        /// This button is used to focus the input node.
        /// </summary>
        private Button _focusInput;
        
        /// <summary>
        /// This button is used to focus the result node.
        /// </summary>
        private Button _focusResult;
        
        /// <summary>
        /// This labels is used to display the number of selected groups.
        /// </summary>
        private Label _selectedGroups;
        
        /// <summary>
        /// This label is used to display the number of selected edges or connections.
        /// </summary>
        private Label _selectedConnections;
        
        /// <summary>
        /// This label is used to display the number of selected nodes.
        /// </summary>
        private Label _selectedNodes;
        
        /// <summary>
        /// This label is used to display the total number of nodes.
        /// </summary>
        private Label _totalNodes;
        
        /// <summary>
        /// This label is used to display the total number of edges or connections.
        /// </summary>
        private Label _totalConnections;
        
        /// <summary>
        /// This label is used to display the total number of groups.
        /// </summary>
        private Label _totalGroups;
        
        /// <summary>
        /// This label is used to display the graph's current scale.
        /// </summary>
        private Label _scale;
        
        /// <summary>
        /// This label is used to display the graph's current position.
        /// </summary>
        private Label _position;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the currently opened <see cref="IFunctionProvider"/>.
        /// </summary>
        public static IFunctionProvider Provider { get; private set; }
        
        /// <summary>
        /// This property contains the <see cref="ScriptableObject"/> of the currently opened
        /// <see cref="IFunctionProvider"/>.
        /// </summary>
        public static AmiliousScriptableObject ProviderScriptableObject { get; private set; }
        
        /// <summary>
        /// This property contains the current instance of the <see cref="FunctionGraphEditor"/>.
        /// </summary>
        public static FunctionGraphEditor Instance { get; set; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Static Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to open the editor window.
        /// </summary>
        [MenuItem("Amilious/FunctionGraphEditor")]
        public static void OpenWindow() {
            var window = GetWindow<FunctionGraphEditor>(false,"Function Graph Editor");
            if(window == null) return;
            Instance = window;
            window.OnSelectionChange();
        }
        
        /// <summary>
        /// This method is called when someone double clicks on an asset.
        /// </summary>
        /// <param name="instanceId">The instance id of the double clicked asset.</param>
        /// <param name="line">The line of the double clicked asset.</param>
        /// <returns>True if opened otherwise false.</returns>
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceId, int line) {
            if(EditorUtility.InstanceIDToObject(instanceId) is not IFunctionProvider) return false;
            OpenWindow();
            return true;
        }
        
        /// <summary>
        /// This method is called when an <see cref="IFunctionProvider"/> is being deleted.
        /// </summary>
        /// <param name="guid">The guid of the asset that is being deleted.</param>
        public static void AssetBeingDeleted(string guid) {
            if(ProviderScriptableObject == null) return;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(ProviderScriptableObject, out var currentGuid, out long _);
            if(currentGuid != guid) return;
            //the currently loaded asset is being deleted.
            if(Instance == null) return;
            Instance._functionNodeInspectorView.Reset();
            Instance._graphView.Reset();
            Instance.ResetTitles();
        }
        
        /// <summary>
        /// This method is called when an <see cref="IFunctionProvider"/> is being renamed.
        /// </summary>
        /// <param name="guid">The guid of the asset.</param>
        /// <param name="newName">The new name of the asset.</param>
        public static void AssetBeingRenamed(string guid, string newName) {
            if(ProviderScriptableObject == null) return;
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(ProviderScriptableObject, out var currentGuid, out long _);
            if(currentGuid != guid) return;
            if(Instance!=null)Instance.ResetTitles(newName);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is called by unity when loading the editor.
        /// </summary>
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

        /// <summary>
        /// This method is used to try load the passed <see cref="IFunctionProvider"/>.
        /// </summary>
        /// <param name="functionProvider">The function that you want to load.</param>
        /// <returns>True if able to load the given function, otherwise false.</returns>
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

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private & Protected Methods ////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to focus the result node.
        /// </summary>
        private void FocusResult() => _graphView?.FocusResult();

        /// <summary>
        /// This method is used to focus the input node.
        /// </summary>
        private void FocusInput() => _graphView?.FocusInput();

        /// <summary>
        /// This method is called when the graph elements have changed.
        /// </summary>
        /// <param name="nodes">The number of nodes.</param>
        /// <param name="connections">The number of connections.</param>
        /// <param name="groups">The number of groups.</param>
        private void CountUpdated(int nodes, int connections, int groups) {
            if(_totalNodes!=null)_totalNodes.text = nodes.ToString();
            if(_totalConnections!=null)_totalConnections.text = connections.ToString();
            if(_totalGroups!=null)_totalGroups.text = groups.ToString();
        }

        /// <summary>
        /// This method is used to clear all the nodes that do not have any connections.
        /// </summary>
        private void ClearUnconnected() => _graphView.ClearUnconnected();

        /// <summary>
        /// This method is called when the GraphView's scale or position is updated.
        /// </summary>
        /// <param name="graphview">The graph view that changed.</param>
        private void ViewChanged(GraphView graphview) {
            var pos = graphview.viewTransform.position;
            _position.text = $"{pos.x}x{pos.y}";
            var scale = graphview.viewTransform.scale;
            _scale.text = $"{scale.x}";
        }

        /// <summary>
        /// This method is called when the selection changes.
        /// </summary>
        /// <param name="obj">The currently selected objects.</param>
        private void OnGraphSelectionChanged(IReadOnlyList<ISelectable> obj) {
            var nodes = obj.Count(x => x is FunctionNodeView);
            _selectedNodes.text = nodes.ToString();
            _selectedConnections.text = obj.Count(x => x is Edge).ToString();
            _selectedGroups.text = obj.Count(x => x is Group).ToString();
            if(nodes == 0) return;
            _functionNodeInspectorView?.UpdateSelection(obj.First(x=>x is FunctionNodeView) as FunctionNodeView);
            ResetTitles();
        }

        /// <summary>
        /// This method is used to redraw the titles.
        /// </summary>
        /// <param name="newName">This value will replace the function's name if it is being renamed.</param>
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

        /// <summary>
        /// This method is called when the selected asset changes.
        /// </summary>
        protected void OnSelectionChange() => TryLoad(Selection.activeObject);

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}