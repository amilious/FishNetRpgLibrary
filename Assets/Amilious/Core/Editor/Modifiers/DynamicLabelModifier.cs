using UnityEditor;
using UnityEngine;
using Amilious.Core.Attributes;

namespace Amilious.Core.Editor.Modifiers {
    
    /// <summary>
    /// This modifier is used to dynamically set the label of a property.
    /// </summary>
    [CustomPropertyDrawer(typeof(DynamicLabelAttribute))]
    public class DynamicLabelModifier : AmiliousPropertyModifier<DynamicLabelAttribute> {
        
        #region Protected Override Methods /////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override void BeforeOnGUI(SerializedProperty property, GUIContent label, bool hidden) {
            var prop = property.serializedObject.FindProperty(Attribute.NameOfLabelField);
            if(prop == null){ 
                label.text = Attribute.NameOfLabelField;
                return;
            }
            label.text = prop.stringValue ?? prop.objectReferenceValue.ToString();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}