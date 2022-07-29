using Amilious.Core.Attributes;
using UnityEditor;
using UnityEngine;

namespace Amilious.Core.Editor.Modifiers {
    
    [CustomPropertyDrawer(typeof(DynamicLabelAttribute))]
    public class DynamicLabelModifier : AmiliousPropertyModifier {
        
        private DynamicLabelAttribute _casted;

        public DynamicLabelAttribute Attribute {
            get {
                if(_casted != null) return _casted;
                _casted = attribute as DynamicLabelAttribute;
                return _casted;
            }
        }

        public override void BeforeOnGUI(SerializedProperty property, GUIContent label, bool hidden) {

            var prop = property.serializedObject.FindProperty(Attribute.NameOfLabelField);
            label.text = prop.stringValue;
            
        }
        
    }
    
}