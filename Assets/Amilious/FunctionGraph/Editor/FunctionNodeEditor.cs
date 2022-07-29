using Amilious.Core.Editor.Editors;
using UnityEditor;

namespace Amilious.FunctionGraph.Editor {
    [CustomEditor(typeof(FunctionNode),true)]
    public class FunctionNodeEditor : AmiliousBaseEditor {

        private string _description;

        protected string Description {
            get {
                if(_description != null) return _description;
                _description = FunctionNode.GetAttribute(target.GetType())?.Description;
                return _description;
            }
        }
        
        protected override void BeforeDefault() {
            EditorGUILayout.HelpBox(Description, MessageType.None);
            EditorGUILayout.Space(10);
        }
    }
}