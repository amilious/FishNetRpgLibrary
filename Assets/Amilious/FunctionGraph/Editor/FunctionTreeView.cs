using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Amilious.FunctionGraph.Nodes.Hidden;
using UnityEditor.Experimental.GraphView;

namespace Amilious.FunctionGraph.Editor {
    
    public class FunctionTreeView : GraphView {

        public new class UxmlFactory : UxmlFactory<FunctionTreeView, GraphView.UxmlTraits> { }

        private readonly Dictionary<Group, string> _groups = new Dictionary<Group, string>();

        public Action<FunctionNodeView> OnNodeSelected;
        public Action<FunctionNodeView> OnNodeUnselected;

        private IFunctionProvider _function;
        
        public FunctionTreeView() {
            
            Insert(0,new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new EdgeManipulator());
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Amilious/FunctionGraph/Editor/FunctionTreeEditor.uss");
            styleSheets.Add(styleSheet);
            viewTransformChanged += ViewChanged;
            elementsAddedToGroup += ElementsAddedToGroup;
            elementsRemovedFromGroup += ElementsRemovedFromGroup;
            groupTitleChanged += GroupTitleChanged;
        }

        private void ElementsAddedToGroup(Group group, IEnumerable<GraphElement> elements) {
            if(!_groups.TryGetValue(group, out var guid)) return;
            var funGroup = _function.GraphData.GroupFromId(guid);
            foreach(var element in elements) {
                if(element is not FunctionNodeView nodeView) continue;
                funGroup.nodeIds.Add(nodeView.Node.guid);
            }
        }

        private void ElementsRemovedFromGroup(Group group, IEnumerable<GraphElement> elements) {
            if(!_groups.TryGetValue(group, out var guid)) return;
            var funGroup = _function.GraphData.GroupFromId(guid);
            foreach(var element in elements) {
                if(!(element is FunctionNodeView nodeView)) continue;
                funGroup.nodeIds.Remove(nodeView.Node.guid);
            }
        }

        private void GroupTitleChanged(Group group, string title) {
            if(!_groups.TryGetValue(group, out var guid)) return;
            var funGroup = _function.GraphData.GroupFromId(guid);
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
                    var group = new Group { title = "New Group" };
                    _groups.Add(group,id);
                    _function.GraphData.groups.Add(new FunctionGroup{id = id, title = group.title});
                    Add(group);
                    group.AddElements(selection.FindAll(x=>x is FunctionNodeView).Cast<GraphElement>());
                });
            }
            base.BuildContextualMenu(evt);

        }

        private void CreateNode(Type type, Vector2 position) {
            if(!_function.TryCreateNode(type, out var node)) return;
            Debug.Log(position.ToString());
            node.position = position;
            CreateNodeView(node);
        }

        private FunctionNodeView FindNodeView(FunctionNode node) {
            return GetNodeByGuid(node.guid) as FunctionNodeView;
        }

        private FunctionNodeView FindNodeView(string guid) {
            return GetNodeByGuid(guid) as FunctionNodeView;
        }

        public void PopulateView(IFunctionProvider function) {
            _function = function;
            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            _groups.Clear();
            function.Initialize();
            viewTransform.position = _function.GraphData.position;
            viewTransform.scale = _function.GraphData.scale;
            //add nodes
            function.Nodes.ForEach(CreateNodeView);
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
                var group = new Group { title = data.title };
                _groups.Add(group,data.id);
                data.nodeIds.ForEach(node=>group.AddElement(FindNodeView(node)));
                Add(group);
            });
            graphViewChanged += OnGraphViewChanged;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange) {
            if(graphViewChange.elementsToRemove != null) {
                var cancel = new List<GraphElement>();
                graphViewChange.elementsToRemove.ForEach(element => {
                    switch(element) {
                        case FunctionNodeView { Node: HiddenNode }: cancel.Add(element); break;
                        case FunctionNodeView nodeView: _function.DeleteNode(nodeView.Node); break;
                        case Edge edge: {
                            var input = edge.input.node as FunctionNodeView;
                            var output = edge.output.node as FunctionNodeView;
                            var inputId = input.Input.IndexOf(edge.input);
                            var outputId = output.Output.IndexOf(edge.output);
                            _function.RemoveConnection(input.Node,output.Node,inputId, outputId);
                            break;
                        }
                        case Group group:
                            _function.GraphData.RemoveGroup(_groups[group]);
                            _groups.Remove(group); break;
                    }
                });
                foreach(var ele in cancel) graphViewChange.elementsToRemove.Remove(ele);
            }

            graphViewChange.edgesToCreate?.ForEach(edge => {
                if(edge.input.node is not FunctionNodeView input || 
                   edge.output.node is not FunctionNodeView output) return;
                var inputId = input.Input.IndexOf(edge.input);
                var outputId = output.Output.IndexOf(edge.output);
                _function.AddConnection(input.Node, output.Node, inputId, outputId);
            });
            return graphViewChange;
        }

        private void CreateNodeView(FunctionNode node) {
            var nodeView = new FunctionNodeView(node) {
                OnNodeSelected = OnNodeSelected,
                OnNodeUnselected = OnNodeUnselected
            };
            AddElement(nodeView);
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

        private bool FindSubNode(FunctionNode lookFor, FunctionNode currentNode) {
            if(currentNode.guid == lookFor.guid) return true;
            foreach(var con in currentNode.inputConnections) {
                if(FindSubNode(lookFor, con.outputNode)) return true;
            }
            return false;
        }
    }
    
}
