/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                                                    //
//    _____            .__ .__   .__                             _________  __              .___.__                   //
//   /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /   _____/_/  |_  __ __   __| _/|__|  ____   ______   //
//  /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     \_____  \ \   __\|  |  \ / __ | |  | /  _ \ /  ___/   //
// /    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \      /        \ |  |  |  |  // /_/ | |  |(  <_> )\___ \    //
// \____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    /_______  / |__|  |____/ \____ | |__| \____//____  >   //
//         \/       \/                                  \/             \/                    \/                 \/    //
//                                                                                                                    //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious,com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using UnityEditor;
using Amilious.Core.Attributes;
using Amilious.Core.Editor.Extensions;

namespace Amilious.Core.Editor.Modifiers {
    
    /// <summary>
    /// This modifier is used to hide a property if a condition is met.
    /// </summary>
    [CustomPropertyDrawer(typeof(HideIfAttribute))]
    public class HideIfModifier : AmiliousPropertyModifier<HideIfAttribute> {

        #region Public Override Methods ////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool CanCacheInspectorGUI(SerializedProperty property) => false;
        
        /// <inheritdoc />
        public override bool ShouldCancelDraw(SerializedProperty property) =>Hide(property);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to check if the property should be hidden.
        /// </summary>
        /// <param name="property">The property that you want to check.</param>
        /// <returns>True if the property should be hidden, otherwise false.</returns>
        protected bool Hide(SerializedProperty property) {
            var castedAttribute = (HideIfAttribute)attribute;
            var hiderProperty = property.FindSiblingProperty(castedAttribute.PropertyName);
            hiderProperty ??= property.serializedObject.FindProperty(castedAttribute.PropertyName);
            hiderProperty ??= property.FindPropertyRelative(castedAttribute.PropertyName);
            if(hiderProperty != null) {
                return hiderProperty.propertyType switch {
                    SerializedPropertyType.Generic => false,
                    SerializedPropertyType.Integer => castedAttribute.Validate(hiderProperty.intValue),
                    SerializedPropertyType.Boolean => castedAttribute.Validate(hiderProperty.boolValue),
                    SerializedPropertyType.Float => castedAttribute.Validate(hiderProperty.floatValue),
                    SerializedPropertyType.String => castedAttribute.Validate(hiderProperty.stringValue),
                    SerializedPropertyType.Color => castedAttribute.Validate(hiderProperty.colorValue),
                    SerializedPropertyType.ObjectReference => 
                        castedAttribute.Validate(hiderProperty.objectReferenceValue),
                    SerializedPropertyType.Enum => castedAttribute.ValidateEnumValue(hiderProperty.enumValueIndex, 
                        hiderProperty.enumValueFlag),
                    SerializedPropertyType.Vector2 => castedAttribute.Validate(hiderProperty.vector2Value), 
                    SerializedPropertyType.Vector3 => castedAttribute.Validate(hiderProperty.vector3Value),
                    SerializedPropertyType.Vector4 => castedAttribute.Validate(hiderProperty.vector4Value), 
                    SerializedPropertyType.Rect => castedAttribute.Validate(hiderProperty.rectValue),
                    SerializedPropertyType.ArraySize => castedAttribute.Validate(hiderProperty.arraySize), 
                    SerializedPropertyType.Character => castedAttribute.Validate(hiderProperty.stringValue[0]),
                    SerializedPropertyType.AnimationCurve => 
                        castedAttribute.Validate(hiderProperty.animationCurveValue),
                    SerializedPropertyType.Bounds => castedAttribute.Validate(hiderProperty.boundsValue), 
                    SerializedPropertyType.Quaternion => castedAttribute.Validate(hiderProperty.quaternionValue),
                    SerializedPropertyType.ExposedReference => 
                        castedAttribute.Validate(hiderProperty.exposedReferenceValue),
                    SerializedPropertyType.FixedBufferSize => castedAttribute.Validate(hiderProperty.fixedBufferSize),
                    SerializedPropertyType.Vector2Int => castedAttribute.Validate(hiderProperty.vector2IntValue),
                    SerializedPropertyType.Vector3Int => castedAttribute.Validate(hiderProperty.vector3IntValue),
                    SerializedPropertyType.RectInt => castedAttribute.Validate(hiderProperty.rectIntValue),
                    SerializedPropertyType.BoundsInt => castedAttribute.Validate(hiderProperty.boundsIntValue),
                    SerializedPropertyType.ManagedReference => 
                        castedAttribute.Validate(hiderProperty.managedReferenceValue),
                    SerializedPropertyType.Hash128 => castedAttribute.Validate(hiderProperty.hash128Value),
                    _ => false
                };
            }

            var field = property.serializedObject?.GetType().GetField(castedAttribute.PropertyName);
            if(field != null) { return castedAttribute.Validate(field.GetValue(property.serializedObject.context)); }
            var prop = property.serializedObject?.GetType().GetProperty(castedAttribute.PropertyName);
            if(prop != null) { return castedAttribute.Validate(prop.GetValue(property.serializedObject.context)); }
            var method = property.serializedObject?.GetType().GetMethod(castedAttribute.PropertyName);
            if(method == null || method.GetParameters().Length >= 1 || method.ReturnParameter == null) return false;
            var result = method.Invoke(property.serializedObject.context, null);
            return castedAttribute.Validate(result);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}