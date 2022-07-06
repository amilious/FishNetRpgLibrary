using System;
using System.Collections.Generic;
using System.Linq;
using Amilious.FunctionGraph.Nodes.Hidden;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Amilious.FunctionGraph.Editor {

    public class GraphSerializedData {

        public GraphSerializedData() { }

        public List<NodeSerializeData> NodeData { get; set; }
        
        public List<ConnectionSerializedData> ConnectionData { get; set; }
        
        public List<GroupSerializeData> Groups { get; set; }

        public GraphSerializedData(IEnumerable<GraphElement> elements, FunctionGraphView view) {
           NodeData = new List<NodeSerializeData>();
            ConnectionData = new List<ConnectionSerializedData>();
            Groups = new List<GroupSerializeData>();
            foreach(var element in elements) {
                //add the nodes
                if(element is FunctionNodeView node) {
                    if(node.Node is HiddenNode) continue;
                    NodeData.Add(new NodeSerializeData(node, view));
                }
                //add the edges
                if(element is Edge edge) ConnectionData.Add(new ConnectionSerializedData(edge));
                //add the groups
                if(element is Group group) Groups.Add(new GroupSerializeData(group));
            }
        }

        public void PasteValues(FunctionGraphView view) {
            var database = new Dictionary<string, FunctionNodeView>();
            //add the nodes
            foreach(var node in NodeData) {
                var pos = new Vector2(node.XPos, node.YPos);
                pos.x -= view.viewTransform.position.x;
                pos.y -= view.viewTransform.position.y;
                var nodeView = view.CreateNode(node.GetCastedType(), pos);
                database.Add(node.Guid,nodeView);
            }
            //add the connections
            foreach(var con in ConnectionData) {
                if(!database.TryGetValue(con.InputNode,out var input)||
                   !database.TryGetValue(con.OutputNode,out var output)) continue;
                var edge = output.Output[con.OutputPort].ConnectTo(input.Input[con.InputPort]);
                view.Add(edge);
                view.HandleCreatingEdgeConnection(edge);
            }
            //add the groups
            foreach(var group in Groups) {
                if(!group.Members.Any(x=>database.ContainsKey(x))) continue;
                var graphGroup = new Group {
                    userData = GUID.Generate().ToString(),
                    title = group.Name
                };
                foreach(var member in group.Members) {
                    if(!database.TryGetValue(member, out var nodeView)) continue;
                    graphGroup.Add(nodeView);
                }
                view.Add(graphGroup);
            }
        }

    }

    [Serializable]
    public class GroupSerializeData {
        
        public GroupSerializeData(){}

        public GroupSerializeData(Group group) {

            Members = new List<string>();

            Name = group.title;

            foreach(var element in group.containedElements) {
                if(element is not FunctionNodeView node) continue;
                Members.Add(node.Node.guid);
            }

        }
        
        public List<string> Members { get; set; }
        
        public string Name { get; set; }
        
    }
    
    [Serializable]
    public class NodeSerializeData {

        public NodeSerializeData() { }

        public NodeSerializeData(FunctionNodeView nodeView, FunctionGraphView view) {
            Type = nodeView.Node.GetType().Name;
            Guid = nodeView.Node.guid;
            XPos = nodeView.Node.Position.x - view.viewTransform.position.x;
            YPos = nodeView.Node.Position.y - view.viewTransform.position.y;
        }

        public string Type { get; set; }
        public string Guid { get; set; }
        
        public float XPos { get; set; }
        
        public float YPos { get; set; }

        public Type GetCastedType() {
            var type = TypeCache.GetTypesDerivedFrom<FunctionNode>()
                .Where(t => !t.IsAbstract&&!typeof(HiddenNode).IsAssignableFrom(t)).FirstOrDefault(x=>x.Name==Type);
            return type;
        }

    }

    [Serializable]
    public class ConnectionSerializedData {
        
        public string InputNode { get; set; }
        
        public string OutputNode { get; set; }
        
        public int InputPort { get; set; }
        
        public int OutputPort { get; set; }
        
        public ConnectionSerializedData() { }

        public ConnectionSerializedData(Edge edge) {
            InputNode = edge.InputNode().guid;
            OutputNode = edge.OutputNode().guid;
            InputPort = edge.InputPort();
            OutputPort = edge.OutputPort();
        }
    }
    
    
}