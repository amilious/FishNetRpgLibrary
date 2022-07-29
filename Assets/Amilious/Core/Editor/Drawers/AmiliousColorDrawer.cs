using Amilious.Core.Attributes;
using Amilious.Core.Extensions;
using UnityEditor;
using UnityEngine;

namespace Amilious.Core.Editor.Drawers {
    [CustomPropertyDrawer(typeof(AmiliousColorAttribute))]
    public class AmiliousColorDrawer : AmiliousPropertyDrawer {
        protected override void AmiliousOnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var oldColor = property.colorValue;
            var oldHex = '#'+oldColor.HtmlRGBA();
            var newColor = oldColor;
            var newHex = oldHex;
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.LabelField(position, label);
            position.x += EditorGUIUtility.labelWidth;
            var width = position.width -= EditorGUIUtility.labelWidth;
            if(position.width < 30){ return;}
            width -= 30;
            if(width >= 87) {
                position.width = 87;
                newHex = EditorGUI.TextField(position, oldHex);
                position.x += 85;
                width -= 87;
            }
            position.width = 30+width;
            newColor = EditorGUI.ColorField(position,oldColor);
            if(!newHex.StartsWith('#')) newHex = '#' + newHex;
            if(newColor != oldColor) property.colorValue = newColor;
            else if(newHex != oldHex && ColorUtility.TryParseHtmlString(newHex, out newColor)){
                property.colorValue = newColor;
            }
            EditorGUI.EndProperty();
        }
    }
}