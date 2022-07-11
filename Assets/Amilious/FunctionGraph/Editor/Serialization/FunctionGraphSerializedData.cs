using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace Amilious.FunctionGraph.Editor.Serialization {
   
    /// <summary>
    /// This class is used to serialize data for copy and paste.
    /// </summary>
    [Serializable]
    public class FunctionGraphSerializedData {

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This dictionary is used to cache function nodes during the paste method.
        /// </summary>
        private readonly Dictionary<string, FunctionNodeView> _tempDatabase = new();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property holds the serialized node data.
        /// </summary>
        public List<FunctionGraphNodeSerializeData> NodeData { get; set; }
        
        /// <summary>
        /// This property contains the serialized edge data.
        /// </summary>
        public List<FunctionGraphConnectionSerializedData> ConnectionData { get; set; }
        
        /// <summary>
        /// This property contains the serialized group data.
        /// </summary>
        public List<FunctionGraphGroupSerializedData> Groups { get; set; }
        
        /// <summary>
        /// This dictionary is used to keep track of the number of time the data has been pasted in a given position.
        /// </summary>
        public Dictionary<string,int> PasteTimes { get; set; }

        #endregion
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This default constructor is used for serialization.
        /// </summary>
        public FunctionGraphSerializedData() { }

        /// <summary>
        /// This constructor is used to create serialized data for the given elements of the given view.
        /// </summary>
        /// <param name="elements">The elements that need to be serialized.</param>
        /// <param name="view">The view that the elements belong to.</param>
        public FunctionGraphSerializedData(IEnumerable<GraphElement> elements, FunctionGraphView view) {
            NodeData = new List<FunctionGraphNodeSerializeData>();
            ConnectionData = new List<FunctionGraphConnectionSerializedData>();
            Groups = new List<FunctionGraphGroupSerializedData>();
            PasteTimes = new Dictionary<string, int>();
            var offset = view.viewTransform.position;
            var scale = Vector3.Scale(Vector3.one,view.viewTransform.scale);
            foreach(var element in elements) {
                //add the nodes
                if(element is FunctionNodeView node) {
                    if(node.Node.IsHidden) continue;
                    NodeData.Add(new FunctionGraphNodeSerializeData(node, offset,scale));
                }
                //add the edges
                if(element is Edge edge) ConnectionData.Add(new FunctionGraphConnectionSerializedData(edge));
                //add the groups
                if(element is Group group) Groups.Add(new FunctionGraphGroupSerializedData(group));
            }
            AddPasteTime(view.viewTransform);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to increment the past times by one at the given position.
        /// </summary>
        /// <param name="viewTransform">The current position of the paste.</param>
        public void AddPasteTime(ITransform viewTransform) {
            var key = viewTransform.GetStringKey();
            PasteTimes.TryGetValue(key, out var times);
            times++;
            PasteTimes[key] = times;
        }

        /// <summary>
        /// This method is used to get the number of times the data has been pasted in the given position.
        /// </summary>
        /// <param name="viewTransform">The position that you want to get the paste count for.</param>
        /// <returns>The number of times that the data has been pasted in this position.</returns>
        public int GetPasteTimes(ITransform viewTransform) {
            var key = viewTransform.GetStringKey();
            PasteTimes.TryGetValue(key, out var times);
            return times;
        }

        /// <summary>
        /// This method is used to paste the data to the given view.
        /// </summary>
        /// <param name="view">The view to paste the data onto.</param>
        public void PasteValues(FunctionGraphView view) {
            view.ClearSelection();
            _tempDatabase.Clear();
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
                _tempDatabase.Add(node.Guid,nodeView);
            }
            //add the connections
            foreach(var con in ConnectionData) {
                if(!_tempDatabase.TryGetValue(con.InputNode,out var input)||
                   !_tempDatabase.TryGetValue(con.OutputNode,out var output)) continue;
                var edge = output.Output[con.OutputPort].ConnectTo(input.Input[con.InputPort]);
                view.Add(edge);
                view.AddToSelection(edge);
                view.HandleCreatingEdgeConnection(edge);
            }
            //add the groups
            var members = new List<GraphElement>();
            foreach(var group in Groups.Where(group => group.Members.Any(x=>_tempDatabase.ContainsKey(x)))) {
                members.Clear();
                foreach(var member in group.Members) {
                    if(_tempDatabase.TryGetValue(member, out var nodeView)) members.Add(nodeView);
                }
                var groupObj = view.AddGroup(group.Name);
                groupObj.AddElements(members);
                view.AddToSelection(groupObj);
            }
            view.FrameSelection();
            AddPasteTime(view.viewTransform);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}