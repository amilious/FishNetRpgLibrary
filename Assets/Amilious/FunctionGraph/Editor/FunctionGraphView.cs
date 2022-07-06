using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Amilious.Core.Extensions;
using Amilious.FunctionGraph.Nodes.Hidden;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor.Experimental.GraphView;

namespace Amilious.FunctionGraph.Editor {
    
    public class FunctionGraphView : GraphView {

        public new class UxmlFactory : UxmlFactory<FunctionGraphView, UxmlTraits> { }


        public Action<FunctionNodeView> OnNodeSelected;
        public Action<FunctionNodeView> OnNodeUnselected;

        private IFunctionProvider _function;

        public FunctionGraphView() {

            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new EdgeManipulator());

            var styleSheet =
                AssetDatabase.LoadAssetAtPath<StyleSheet>(
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

        private void OnPaste(string operationName, string data) {
            var nodeDate = JsonConvert.DeserializeObject<GraphSerializedData>(data);
            nodeDate?.PasteValues(this);
        }

        private bool CanIPaste(string data) {
            try {
                var nodeDate = JsonConvert.DeserializeObject<GraphSerializedData>(data);
                return nodeDate != null;
            }catch(Exception) { return false;}
        }

        private string OnCopy(IEnumerable<GraphElement> elements) {
            var data = new GraphSerializedData(elements,this);
            return JsonConvert.SerializeObject(data);
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
            foreach(var element in elements) {
                if(!(element is FunctionNodeView nodeView)) continue;
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

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt) {
            if(_function is null) return;
            var position = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
            var mouse = evt.mousePosition;
            mouse.x += 120;
            mouse.y += 15;
            evt.menu.AppendAction("Add Node", a => {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(mouse)),
                    ScriptableObject.CreateInstance<FunctionNodeSearchProvider>().AddCallback(CreateNode, position));
            });
            evt.menu.AppendSeparator();
            if(selection.Count > 0) {
                evt.menu.AppendAction("Create Group", a => {
                    var id = GUID.Generate().ToString();
                    var group = new Group { title = "New Group", userData = id};
                    _function.GraphData.groups.Add(new FunctionGroup{id = id, title = group.title});
                    Add(group);
                    group.AddElements(selection.FindAll(x=>x is FunctionNodeView).Cast<GraphElement>());
                });
            }
            base.BuildContextualMenu(evt);

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
                    var connections = function.GetInputConnections(n, i);
                    connections.ForEach(c => {
                        var outputView = FindNodeView(c.outputNode);
                        var edge = outputView.Output[c.outputPort].ConnectTo(inputView.Input[c.inputPort]);
                        AddElement(edge);
                    });
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
            return graphViewChange;
        }

        public void HandleRemovingGroup(Group group, List<GraphElement> cancel = null) {
            if(group == null || _function == null) return;
            _function.GraphData.RemoveGroup(group.GetId());
        }

        public void HandleRemovingEdge(Edge edge, List<GraphElement> cancel =null) {
            if(edge == null||_function==null) return;
            _function.RemoveConnection(edge.InputNode(),edge.OutputNode(),edge.InputPort(),edge.OutputPort());
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
            _function.AddConnection(edge.InputNode(), edge.OutputNode(), edge.InputPort(),edge.OutputPort());
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
            var nodeView = new FunctionNodeView(node) {
                OnNodeSelected = OnNodeSelected,
                OnNodeUnselected = OnNodeUnselected
            };
            AddElement(nodeView);
            return nodeView;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) {
            var list = ports.ToList().Where(endPort =>
                endPort.direction != startPort.direction && endPort.node != startPort.node &&
                endPort.portType == startPort.portType).ToList();
            //allow ints to connect to floats but not the other way around
            if(startPort.direction == Direction.Output && startPort.portType == typeof(int)) {
                list.AddRange(ports.Where(endPort =>
                    endPort.direction == Direction.Input && endPort.portType == typeof(float)));
            }
            if(startPort.direction == Direction.Input && startPort.portType == typeof(float)) {
                list.AddRange(ports.Where(endPort =>
                    endPort.direction == Direction.Output && endPort.portType == typeof(int)));
            }
            //check for loops and duplicate connections
            var remove = new List<Port>();
            foreach(var port in list) {
                var start = startPort.FunctionNode();
                var end = port.FunctionNode();
                if(startPort.HasConnectionTo(port)) { remove.Add(port); continue; }
                if(FindSubNode(end, start)) { remove.Add(port); continue; }
                if(FindSubNode(start, end)) { remove.Add(port); continue; }
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

        public void Reset() {
            _function = null;
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
        }
        
        
    }
    
}
