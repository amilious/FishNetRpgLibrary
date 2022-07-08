using System;
using System.Collections.Generic;
using System.Linq;
using Amilious.Core.Extensions;
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
        
        public Dictionary<string,int> PasteTimes { get; set; }

        public void AddPasteTime(ITransform viewTransform) {
            var key = viewTransform.GetStringKey();
            PasteTimes.TryGetValue(key, out var times);
            times++;
            PasteTimes[key] = times;
        }

        public int GetPasteTimes(ITransform viewTransform) {
            var key = viewTransform.GetStringKey();
            PasteTimes.TryGetValue(key, out var times);
            return times;
        }

        public GraphSerializedData(IEnumerable<GraphElement> elements, FunctionGraphView view) {
           NodeData = new List<NodeSerializeData>();
            ConnectionData = new List<ConnectionSerializedData>();
            Groups = new List<GroupSerializeData>();
            PasteTimes = new Dictionary<string, int>();
            var offset = view.viewTransform.position;
            var scale = Vector3.Scale(Vector3.one,view.viewTransform.scale);
            foreach(var element in elements) {
                //add the nodes
                if(element is FunctionNodeView node) {
                    if(node.Node is HiddenNode) continue;
                    NodeData.Add(new NodeSerializeData(node, offset,scale));
                }
                //add the edges
                if(element is Edge edge) ConnectionData.Add(new ConnectionSerializedData(edge));
                //add the groups
                if(element is Group group) Groups.Add(new GroupSerializeData(group));
            }
            AddPasteTime(view.viewTransform);
        }

        public void PasteValues(FunctionGraphView view) {
            view.ClearSelection();
            var database = new Dictionary<string, FunctionNodeView>();
            var offset = view.viewTransform.position;
            var correction = Vector3.Scale(Vector3.one,view.viewTransform.scale);
            var pastedTimes = GetPasteTimes(view.viewTransform);
            var pasteOffset = Vector2.one * 10 * pastedTimes;
            //add the nodes
            foreach(var node in NodeData) {
                var pos = new Vector2(node.XPos - offset.x - correction.x + pasteOffset.x, 
                    node.YPos - offset.y - correction.y - pasteOffset.y);
                var nodeView = view.CreateNode(node.GetCastedType(), pos);
                view.AddToSelection(nodeView);
                database.Add(node.Guid,nodeView);
            }
            //add the connections
            foreach(var con in ConnectionData) {
                if(!database.TryGetValue(con.InputNode,out var input)||
                   !database.TryGetValue(con.OutputNode,out var output)) continue;
                var edge = output.Output[con.OutputPort].ConnectTo(input.Input[con.InputPort]);
                view.Add(edge);
                view.AddToSelection(edge);
                view.HandleCreatingEdgeConnection(edge);
            }
            //add the groups
            var members = new List<GraphElement>();
            foreach(var group in Groups) {
                if(!group.Members.Any(x=>database.ContainsKey(x))) continue;
                members.Clear();
                foreach(var member in group.Members) {
                    if(!database.TryGetValue(member, out var nodeView)) continue;
                    members.Add(nodeView);
                    //graphGroup.Add(nodeView);
                }
                var groupObj = view.AddGroup(group.Name);
                groupObj.AddElements(members);
                view.AddToSelection(groupObj);
            }
            view.FrameSelection();
            AddPasteTime(view.viewTransform);
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

        public NodeSerializeData(FunctionNodeView nodeView, Vector2 offset, Vector2 scale) {
            Type = nodeView.Node.GetType().Name;
            Guid = nodeView.Node.guid;
            var start = new Vector2(nodeView.Node.Position.x, nodeView.Node.Position.y);
            start += offset;
            start += scale;
            XPos = start.x;
            YPos = start.y;
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