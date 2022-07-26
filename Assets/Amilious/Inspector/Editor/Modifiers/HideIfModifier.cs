using Amilious.Inspector.Attributes;
using UnityEditor;

namespace Amilious.Inspector.Editor.Modifiers {
    
    [CustomPropertyDrawer(typeof(HideIfAttribute))]
    public class HideIfModifier : AmiliousPropertyModifier {
        
        public bool Hide(SerializedProperty property) {
            var castedAttribute = (HideIfAttribute)attribute;
            var hiderProperty = property.serializedObject.FindProperty(castedAttribute.PropertyName);
            if(hiderProperty != null) {
                return hiderProperty.propertyType switch {
                    SerializedPropertyType.Generic => false,
                    SerializedPropertyType.Integer => castedAttribute.Validate(hiderProperty.intValue),
                    SerializedPropertyType.Boolean => castedAttribute.Validate(hiderProperty.boolValue),
                    SerializedPropertyType.Float => castedAttribute.Validate(hiderProperty.floatValue),
                    SerializedPropertyType.String => castedAttribute.Validate(hiderProperty.stringValue),
                    SerializedPropertyType.Color => castedAttribute.Validate(hiderProperty.colorValue),
                    SerializedPropertyType.ObjectReference => castedAttribute.Validate(hiderProperty.objectReferenceValue),
                    SerializedPropertyType.Enum => castedAttribute.ValidateEnumValue(hiderProperty.enumValueIndex, hiderProperty.enumValueFlag),
                    SerializedPropertyType.Vector2 => castedAttribute.Validate(hiderProperty.vector2Value), 
                    SerializedPropertyType.Vector3 => castedAttribute.Validate(hiderProperty.vector3Value),
                    SerializedPropertyType.Vector4 => castedAttribute.Validate(hiderProperty.vector4Value), 
                    SerializedPropertyType.Rect => castedAttribute.Validate(hiderProperty.rectValue),
                    SerializedPropertyType.ArraySize => castedAttribute.Validate(hiderProperty.arraySize), 
                    SerializedPropertyType.Character => castedAttribute.Validate(hiderProperty.stringValue[0]),
                    SerializedPropertyType.AnimationCurve => castedAttribute.Validate(hiderProperty.animationCurveValue),
                    SerializedPropertyType.Bounds => castedAttribute.Validate(hiderProperty.boundsValue), 
                    SerializedPropertyType.Quaternion => castedAttribute.Validate(hiderProperty.quaternionValue),
                    SerializedPropertyType.ExposedReference => castedAttribute.Validate(hiderProperty.exposedReferenceValue),
                    SerializedPropertyType.FixedBufferSize => castedAttribute.Validate(hiderProperty.fixedBufferSize),
                    SerializedPropertyType.Vector2Int => castedAttribute.Validate(hiderProperty.vector2IntValue),
                    SerializedPropertyType.Vector3Int => castedAttribute.Validate(hiderProperty.vector3IntValue),
                    SerializedPropertyType.RectInt => castedAttribute.Validate(hiderProperty.rectIntValue),
                    SerializedPropertyType.BoundsInt => castedAttribute.Validate(hiderProperty.boundsIntValue),
                    SerializedPropertyType.ManagedReference => castedAttribute.Validate(hiderProperty.managedReferenceValue),
                    SerializedPropertyType.Hash128 => castedAttribute.Validate(hiderProperty.hash128Value),
                    _ => false
                };
            }

            var field = property.serializedObject?.GetType()?.GetField(castedAttribute.PropertyName);
            if(field != null) { return castedAttribute.Validate(field.GetValue(property.serializedObject.context)); }

            var prop = property.serializedObject?.GetType()?.GetProperty(castedAttribute.PropertyName);
            if(prop != null) { return castedAttribute.Validate(prop.GetValue(property.serializedObject.context)); }

            var method = property.serializedObject?.GetType().GetMethod(castedAttribute.PropertyName);
            if(method != null && method.GetParameters().Length < 1 &&
               method.ReturnParameter != null) {
                var result = method.Invoke(property.serializedObject.context, null);
                return castedAttribute.Validate(result);
            }
            return false;
        }

        public override bool ShouldCancelDraw(SerializedProperty property) =>Hide(property);
        
    }
}