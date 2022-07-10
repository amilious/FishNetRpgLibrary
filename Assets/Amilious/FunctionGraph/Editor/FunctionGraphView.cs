using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Amilious.Core.Extensions;
using Amilious.FunctionGraph.Editor.Serialization;
using Amilious.FunctionGraph.Nodes.Hidden;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor.Experimental.GraphView;

namespace Amilious.FunctionGraph.Editor {
    
    public class FunctionGraphView : GraphView {

        public new class UxmlFactory : UxmlFactory<FunctionGraphView, UxmlTraits> { }

        public const string SERIALIZE_PREFIX = "Amilious/FunctionGraph ";

        public delegate void OnCountUpdatedDelegate(int nodes, int connections, int groups);

        public event Action<IReadOnlyList<ISelectable>> OnSelectionChanged;
        public event OnCountUpdatedDelegate OnCountUpdated;

        private IFunctionProvider _function;

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
       
        private void ElementsAddedToGroup(Group group, IEnumerable<GraphElement> elements) {
            var funGroup = _function.GraphData.GroupFromId(group.GetId());
            foreach(var element in elements) {
                if(element is not FunctionNodeView nodeView) continue;
                funGroup.nodeIds.Add(nodeView.Node.guid);
            }
        }

        private void ElementsRemovedFromGroup(Group group, IEnumerable<GraphElement> elements) {
            var funGroup = _function.GraphData.GroupFromId(group.GetId());
            if(funGroup == null) return;
            foreach(var element in elements) {
                if(element is not FunctionNodeView nodeView) continue;
                funGroup.nodeIds.Remove(nodeView.Node.guid);
            }
        }

        private void GroupTitleChanged(Group group, string title) {
            var funGroup = _function.GraphData.GroupFromId(group.GetId());
            funGroup.title = title;
        }

        private void ViewChanged(GraphView graphview) {
            if(_function == null) return;
            _function.GraphData.position = graphview.viewTransform.position;
            _function.GraphData.scale = graphview.viewTransform.scale;
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

        public Group AddGroup(string groupName = "New Group") {
            var id = GUID.Generate().ToString();
            var group = new Group { title = groupName, userData = id};
            _function.GraphData.groups.Add(new FunctionGroup{id = id, title = group.title});
            Add(group);
            TriggerCountUpdate();
            return group;
        }

        public FunctionNodeView CreateNode(Type type, Vector2 position) {
            if(!_function.TryCreateNode(type, out var node)) return null;
            node.Position = position;
            return CreateNodeView(node);
        }

        private FunctionNodeView FindNodeView(FunctionNode node) {
            return GetNodeByGuid(node.guid) as FunctionNodeView;
        }

        private FunctionNodeView FindNodeView(string guid) {
            return GetNodeByGuid(guid) as FunctionNodeView;
        }

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

        public void TriggerCountUpdate() {
            if(_function==null){ OnCountUpdated?.Invoke(0,0,0); return; }
            OnCountUpdated?.Invoke(nodes.Count(),edges.Count(),_function.GraphData.groups.Count);
        }

        public void HandleRemovingGroup(Group group, List<GraphElement> cancel = null) {
            if(group == null || _function == null) return;
            _function.GraphData.RemoveGroup(group.GetId());
        }

        public void HandleRemovingEdge(Edge edge, List<GraphElement> cancel =null) {
            if(edge == null||_function==null) return;
            _function.RemoveConnection(edge.Connection());
        }

        
        public void HandleRemoveNode(FunctionNodeView node, List<GraphElement> cancel =null) {
            if(node == null || _function == null) return;
            if(node.Node is HiddenNode && cancel != null) { cancel.Add(node); return; }
            _function.DeleteNode(node.Node);
        }

        /// <summary>
        /// This method is used to create the connection for the newly created edge.
        /// </summary>
        /// <param name="edge">The newly created edge.</param>
        public void HandleCreatingEdgeConnection(Edge edge) {
            if(edge == null || _function == null) return;
            _function.AddConnection(edge.Connection());
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

        private FunctionNodeView CreateNodeView(FunctionNode node) {
            if(_function == null) return null;
            var nodeView = new FunctionNodeView(node,_function);
            AddElement(nodeView);
            TriggerCountUpdate();
            return nodeView;
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

        /// <summary>
        /// This is a recursive check to try find a node by following the connects of the starting node.
        /// </summary>
        /// <param name="lookFor">The node that you are looking for.</param>
        /// <param name="startingNode">The node that you want to start looking in.</param>
        /// <returns>True if the node that you were looking for was found, otherwise false.</returns>
        private static bool FindSubNode(FunctionNode lookFor, FunctionNode startingNode) {
            return startingNode.guid == lookFor.guid || startingNode.inputConnections.
                Any(con => FindSubNode(lookFor, con.outputNode));
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
        
    }
    
}
