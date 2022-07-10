using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor.Experimental.GraphView;
using Amilious.FunctionGraph.Nodes.Hidden;
using Amilious.FunctionGraph.Editor.Serialization;

namespace Amilious.FunctionGraph.Editor {
    
    /// <summary>
    /// This class is used to display and modify a function.
    /// </summary>
    public class FunctionGraphView : GraphView {
        
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This string is used to make sure that any serialized data is of the correct type.
        /// </summary>
        public const string SERIALIZE_PREFIX = "Amilious/FunctionGraph ";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Fields /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field contains the currently loaded <see cref="IFunctionProvider"/>.
        /// </summary>
        private IFunctionProvider _function;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region UXML Factory ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field adds the <see cref="FunctionGraphView"/> to the UI Builder.
        /// </summary>
        public new class UxmlFactory : UxmlFactory<FunctionGraphView, UxmlTraits> { }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Delegates //////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This delegate is used for the <see cref="FunctionGraphView.OnCountUpdated"/> event.
        /// <param name="nodes">The number of nodes.</param>
        /// <param name="connections">The number of connections.</param>
        /// <param name="groups">The number of groups.</param>
        /// </summary>
        public delegate void OnCountUpdatedDelegate(int nodes, int connections, int groups);               
        
        #endregion

        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This event is triggered when the graphs selection changes.
        /// </summary>
        public event Action<IReadOnlyList<ISelectable>> OnSelectionChanged;
        
        /// <summary>
        /// This event is triggered when the count of GraphElements changes.
        /// </summary>
        public event OnCountUpdatedDelegate OnCountUpdated;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to create a new instance of a <see cref="FunctionGraphView"/>.
        /// </summary>
        public FunctionGraphView() {

            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new EdgeManipulator());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                    "Assets/Amilious/FunctionGraph/Editor/FunctionGraphEditor.uss");
            styleSheets.Add(styleSheet);
            viewTransformChanged += ViewChanged;
            elementsAddedToGroup += ElementsAddedToGroup;
            elementsRemovedFromGroup += ElementsRemovedFromGroup;
            groupTitleChanged += GroupTitleChanged;
            var miniMap = new MiniMap();
            miniMap.SetPosition(new Rect(10, 10, 200, 150));
            miniMap.maxHeight = 150;
            miniMap.maxWidth = 200;
            miniMap.OnResized();
            Add(miniMap);
            serializeGraphElements = OnCopy;
            canPasteSerializedData = CanIPaste;
            unserializeAndPaste = OnPaste;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Override Methods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This is the method that is used to clear the selection.
        /// </summary>
        public override void ClearSelection() {
            base.ClearSelection();
            OnSelectionChanged?.Invoke(selection);
        }

        /// <summary>
        /// This is the method that is used to add an item to the selection.
        /// </summary>
        /// <param name="selectable">The selectable item that is being added to the selection</param>
        public override void AddToSelection(ISelectable selectable) {
            base.AddToSelection(selectable);
            OnSelectionChanged?.Invoke(selection);
        }

        /// <summary>
        /// This is the method that is used to remove an item from the selection.
        /// </summary>
        /// <param name="selectable">The selectable item that is being removed from the selection.</param>
        public override void RemoveFromSelection(ISelectable selectable) {
            base.RemoveFromSelection(selectable);
            OnSelectionChanged?.Invoke(selection);
        }

        /// <inheritdoc />
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt) {
            if(_function is null) return;
            var pos = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
            var mouse = evt.mousePosition;
            mouse.x += 120;
            mouse.y += 15;
            evt.menu.AppendAction("Add Node", _ => {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(mouse)),
                    ScriptableObject.CreateInstance<FunctionNodeSearchProvider>().UpdateCallback(CreateNode, pos));
            });
            evt.menu.AppendSeparator();
            if(selection.Count > 0) {
                evt.menu.AppendAction("Create Group", _ => {
                    var group = AddGroup();
                    group.AddElements(selection.FindAll(x=>x is FunctionNodeView).Cast<GraphElement>());
                });
            }
            base.BuildContextualMenu(evt);

        }
        
        /// <inheritdoc />
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
            var list = ports.ToList().Where(endPort =>
                endPort.direction != startPort.direction && endPort.node != startPort.node &&
                endPort.portType == startPort.portType).ToList();
            //allow ints to connect to floats but not the other way around
            if(startPort.IsOutPut() && startPort.IsType<int>()) {
                list.AddRange(ports.Where(endPort =>
                    endPort.direction == Direction.Input && endPort.portType == typeof(float)));
            }
            if(startPort.IsInput() && startPort.IsType<float>()) {
                list.AddRange(ports.Where(endPort =>
                    endPort.direction == Direction.Output && endPort.portType == typeof(int)));
            }
            //check for loops and duplicate connections
            var remove = new List<Port>();
            foreach(var port in list) {
                //check if the connection already exists
                if(startPort.HasConnectionTo(port)) { remove.Add(port); continue; }
                var inputPort = startPort.IsInput()?startPort:port;
                var outputPort = startPort.IsOutPut()?startPort:port;
                //check for loops 
                if(inputPort.FunctionNode().HasOutputConnectionToNode(outputPort.FunctionNode())) {
                    remove.Add(port); continue;
                }
                if(outputPort.FunctionNode().HasInputConnectionToNode(inputPort.FunctionNode())) {
                    remove.Add(port); continue;
                }
                //check for loop non loop mismatch
            }
            foreach(var rm in remove) list.Remove(rm);
            return list;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Serialization Methods //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is called when items are pasted.
        /// </summary>
        /// <param name="operationName">The operation name.</param>
        /// <param name="data">The serialized data that is being pasted.</param>
        private void OnPaste(string operationName, string data) {
            if(!data.StartsWith(SERIALIZE_PREFIX)) return;
            data = data.Substring(SERIALIZE_PREFIX.Length);
            var nodeDate = JsonConvert.DeserializeObject<FunctionGraphSerializedData>(data);
            nodeDate?.PasteValues(this);
            //the data will be updated when pasted but we need to write the changes
            EditorGUIUtility.systemCopyBuffer = "application/vnd.unity.graphview.elements " + 
                SERIALIZE_PREFIX + JsonConvert.SerializeObject(nodeDate);
        }

        /// <summary>
        /// This method is used to check if the provided data can be pasted in this graph.
        /// </summary>
        /// <param name="data">The data that is available to paste.</param>
        /// <returns>True if the data can be pasted, otherwise false.</returns>
        private static bool CanIPaste(string data) {
            if(!data.StartsWith(SERIALIZE_PREFIX)) return false;
            data = data[SERIALIZE_PREFIX.Length..];
            var nodeDate = JsonConvert.DeserializeObject<FunctionGraphSerializedData>(data);
            return nodeDate != null;
        }

        /// <summary>
        /// This method is used to serialize the give data when using copy.
        /// </summary>
        /// <param name="elements">The elements that are being copied.</param>
        /// <returns>The serialized data for the given elements.</returns>
        private string OnCopy(IEnumerable<GraphElement> elements) {
            var data = new FunctionGraphSerializedData(elements,this);
            return SERIALIZE_PREFIX + JsonConvert.SerializeObject(data);
        }
       
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Group Methods //////////////////////////////////////////////////////////////////////////////////////////
           
        /// <summary>
        /// This method is used to handle elements that are added to a group.
        /// </summary>
        /// <param name="group">The group the elements were added to.</param>
        /// <param name="elements">The elements that were added to the group.</param>
        private void ElementsAddedToGroup(Group group, IEnumerable<GraphElement> elements) {
            var funGroup = _function.GraphData.GroupFromId(group.GetId());
            foreach(var element in elements) {
                if(element is not FunctionNodeView nodeView) continue;
                funGroup.nodeIds.Add(nodeView.Node.guid);
            }
        }

        /// <summary>
        /// This method is used to handle elements that are removed from a group.
        /// </summary>
        /// <param name="group">The group the elements were removed from.</param>
        /// <param name="elements">The elements that were removed from the group.</param>
        private void ElementsRemovedFromGroup(Group group, IEnumerable<GraphElement> elements) {
            var funGroup = _function.GraphData.GroupFromId(group.GetId());
            if(funGroup == null) return;
            foreach(var element in elements) {
                if(element is not FunctionNodeView nodeView) continue;
                funGroup.nodeIds.Remove(nodeView.Node.guid);
            }
        }

        /// <summary>
        /// This method is called when a group's title is changed.
        /// </summary>
        /// <param name="group">The group whose title changed.</param>
        /// <param name="title">The new title of the group.</param>
        private void GroupTitleChanged(Group group, string title) {
            var funGroup = _function.GraphData.GroupFromId(group.GetId());
            funGroup.title = title;
        }
        
        /// <summary>
        /// This method is used to add a new group to the view.
        /// </summary>
        /// <param name="groupName">The name of the new group.</param>
        /// <returns>The group that was added to the view.</returns>
        public Group AddGroup(string groupName = "New Group") {
            var id = GUID.Generate().ToString();
            var group = new Group { title = groupName, userData = id};
            _function.GraphData.groups.Add(new FunctionGroup{id = id, title = group.title});
            Add(group);
            TriggerCountUpdate();
            return group;
        }
        
        /// <summary>
        /// This method is used to handle a group being removed.
        /// </summary>
        /// <param name="group">The group that is being removed.</param>
        /// <param name="cancel">The cancel deletion list.</param>
        public void HandleRemovingGroup(Group group, List<GraphElement> cancel = null) {
            if(group == null || _function == null) return;
            _function.GraphData.RemoveGroup(group.GetId());
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
        #region Node Methods ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the node view for the given node.
        /// </summary>
        /// <param name="node">The node that you want to get the node view for.</param>
        /// <returns>The node view for the given node if it exists.</returns>
        private FunctionNodeView FindNodeView(FunctionNode node) => GetNodeByGuid(node.guid) as FunctionNodeView;

        /// <summary>
        /// This method is used to get the node view for the given guid.
        /// </summary>
        /// <param name="guid">The id of the node view that you want to get.</param>
        /// <returns>The node view for the node with the given guid.</returns>
        private FunctionNodeView FindNodeView(string guid) => GetNodeByGuid(guid) as FunctionNodeView;
        
        /// <summary>
        /// This method is used to create a node view for the given node.
        /// </summary>
        /// <param name="node">The node that you want to create a view for.</param>
        /// <returns>The newly created node view.</returns>
        private FunctionNodeView CreateNodeView(FunctionNode node) {
            if(_function == null) return null;
            var nodeView = new FunctionNodeView(node,_function);
            AddElement(nodeView);
            TriggerCountUpdate();
            return nodeView;
        }         
        
        /// <summary>
        /// This method is used to create a new node and a corresponding node view.
        /// </summary>
        /// <param name="type">The type of node that you want to create.</param>
        /// <param name="position">The position of the newly created node.</param>
        /// <returns>The view of the newly created node.</returns>
        public FunctionNodeView CreateNode(Type type, Vector2 position) {
            if(!_function.TryCreateNode(type, out var node)) return null;
            node.Position = position;
            return CreateNodeView(node);
        }
        
        /// <summary>
        /// This method is used to handle removing nodes.
        /// </summary>
        /// <param name="node">The node that you want to remove.</param>
        /// <param name="cancel">The cancel remove list.</param>
        public void HandleRemoveNode(FunctionNodeView node, List<GraphElement> cancel =null) {
            if(node == null || _function == null) return;
            if(node.Node is HiddenNode && cancel != null) { cancel.Add(node); return; }
            _function.DeleteNode(node.Node);
        }

        /// <summary>
        /// This method is used to save locations of nodes after they have stopped moving.
        /// </summary>
        /// <param name="movedElements">The elements that were moved.</param>
        public static void HandleNodeMovement(List<GraphElement> movedElements) {
            movedElements?.ForEach(element => {
                if(element is not FunctionNodeView view) return;
                view.Node.Position = view.GetPosition().MinPosition();
            });
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Edge Methods ///////////////////////////////////////////////////////////////////////////////////////////
        
       /// <summary>
       /// This method is used to handle removing an edge connection.
       /// </summary>
       /// <param name="edge">The edge that is being removed.</param>
       /// <param name="cancel">The cancel list that is used to cancel removing the edge.</param>
        public void HandleRemovingEdge(Edge edge, List<GraphElement> cancel =null) {
            if(edge == null||_function==null) return;
            _function.RemoveConnection(edge.Connection());
        }
        
        /// <summary>
        /// This method is used to create the connection for the newly created edge.
        /// </summary>
        /// <param name="edge">The newly created edge.</param>
        public void HandleCreatingEdgeConnection(Edge edge) {
            if(edge == null || _function == null) return;
            _function.AddConnection(edge.Connection());
        }
        
        #endregion
        
        #region Other Private Methods //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is called whenever the graph view's scale or position changes.
        /// </summary>
        /// <param name="graphview">The graph view that changed.</param>
        private void ViewChanged(GraphView graphview) {
            if(_function == null) return;
            _function.GraphData.position = graphview.viewTransform.position;
            _function.GraphData.scale = graphview.viewTransform.scale;
        }

        /// <summary>
        /// This method is called when the graph view changes.
        /// </summary>
        /// <param name="graphViewChange">The changes.</param>
        /// <returns>The changes that should be applied.</returns>
        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange) {
            HandleNodeMovement(graphViewChange.movedElements);
            if(graphViewChange.elementsToRemove != null) {
                var cancel = new List<GraphElement>();
                graphViewChange.elementsToRemove.ForEach(element => {
                    switch(element) {
                        case FunctionNodeView nodeView: HandleRemoveNode(nodeView,cancel); break;
                        case Edge edge: HandleRemovingEdge(edge,cancel); break;
                        case Group group: HandleRemovingGroup(group, cancel); break;
                    }
                });
                foreach(var ele in cancel) graphViewChange.elementsToRemove.Remove(ele);
            }
            if(graphViewChange.edgesToCreate!=null)
                foreach(var edge in graphViewChange.edgesToCreate) HandleCreatingEdgeConnection(edge);

            var nCount = nodes.Count();
            nCount -= graphViewChange.elementsToRemove?.Where(x => x is FunctionNodeView).Count()??0;
            var cCount = edges.ToList().Count;
            cCount -= graphViewChange.elementsToRemove?.Where(x => x is Edge).Count()??0;
            cCount += graphViewChange.edgesToCreate?.Count??0;
            var gCount = _function.GraphData.groups.Count;
            OnCountUpdated?.Invoke(nCount,cCount, gCount);
            
            return graphViewChange;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Other Public Methods ///////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to set up the view for the given <see cref="IFunctionProvider"/>.
        /// </summary>
        /// <param name="function">The function that you want to set the view up for.</param>
        public void PopulateView(IFunctionProvider function) {
            Reset();
            _function = function;
            function.Initialize();
            viewTransform.position = _function.GraphData.position;
            viewTransform.scale = _function.GraphData.scale;
            //add nodes
            function.Nodes.ForEach(x=>CreateNodeView(x));
            //add edges
            function.Nodes.ForEach(n => {
                var inputView = FindNodeView(n);
                for(int i = 0; i < n.InputPortInfo.Count; i++) {
                    foreach(var con in function.GetInputConnections(n, i)){
                        var outputView = FindNodeView(con.outputNode);
                        var edge = outputView.Output[con.outputPort].ConnectTo(inputView.Input[con.inputPort]);
                        edge.userData = con;
                        AddElement(edge);
                    }
                }
            });
            //add groups
            function.GraphData.groups.ForEach(data => {
                var group = new Group { title = data.title, userData = data.id};
                //_groups.Add(group,data.id);
                data.nodeIds.ForEach(node=>group.AddElement(FindNodeView(node)));
                Add(group);
            });
            graphViewChanged += OnGraphViewChanged;
            OnCountUpdated?.Invoke(nodes.Count(),edges.Count(),_function.GraphData.groups.Count);
        }

        /// <summary>
        /// This method is used to trigger the count update event with the default values.
        /// </summary>
        public void TriggerCountUpdate() {
            if(_function==null){ OnCountUpdated?.Invoke(0,0,0); return; }
            OnCountUpdated?.Invoke(nodes.Count(),edges.Count(),_function.GraphData.groups.Count);
        }
      
        /// <summary>
        /// This method is used to reset the graph.
        /// </summary>
        public void Reset() {
            _function = null;
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
        }

        /// <summary>
        /// This method is used to remove nodes that do not have any connections.
        /// </summary>
        public void ClearUnconnected() {
            foreach(var node in nodes.Cast<FunctionNodeView>()) {
                if(node.Node.IsInputNode||node.Node.IsResultNode) continue;
                if(node.Node.inputConnections.Count!=0) continue;
                if(node.Node.outputConnections.Count!=0) continue;
                HandleRemoveNode(node);
                RemoveElement(node);
            }
            MarkDirtyRepaint();
        }

        /// <summary>
        /// This method is used to focus the input node.
        /// </summary>
        public void FocusInput() {
            if(_function == null) return;
            var input = _function.GetInputNode;
            if(input == null) return;
            var inputView = FindNodeView(input);
            if(inputView == null) return;
            ClearSelection();
            AddToSelection(inputView);
            FrameSelection();
        }

        /// <summary>
        /// This method is used to focus the result node.
        /// </summary>
        public void FocusResult() {
            if(_function == null) return;
            var result = _function.GetResultNode;
            if(result == null) return;
            var resultView = FindNodeView(result);
            if(resultView == null) return;
            ClearSelection();
            AddToSelection(resultView);
            FrameSelection();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}
