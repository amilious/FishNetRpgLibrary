using Amilious.FishNetRpg.Items;
using Amilious.FishNetRpg.Statistics.BaseProviders;
using Amilious.Inspector.Editor.Editors;
using UnityEditor;
using UnityEngine;

namespace Amilious.FishNetRpg.Editor {
    [CustomEditor(typeof(Item), true)]
    public class ItemEditor : AmiliousBaseEditor {

        private static GUIStyle _style;

        private static GUIStyle Style {
            get {
                _style ??= new GUIStyle(EditorStyles.largeLabel){ richText = true };
                return _style;
            }
        }

        protected override void BeforeDefault() {
            if(target is not Item item) return;
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent("Item Id: <color=#88ff88>"+item.Id+"</color>", 
                "This is a unique identifier for this item."),Style);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}