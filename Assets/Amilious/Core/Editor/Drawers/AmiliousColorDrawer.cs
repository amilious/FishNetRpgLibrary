using UnityEditor;
using UnityEngine;
using Amilious.Core.Extensions;

namespace Amilious.Core.Editor.Drawers {
    
    /// <summary>
    /// This drawer is used instead of the default color drawer.
    /// </summary>
    [CustomPropertyDrawer(typeof(Color))]
    [CustomPropertyDrawer(typeof(AmiliousColorDrawer))]
    public class AmiliousColorDrawer : AmiliousPropertyDrawer {
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void AmiliousOnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var oldColor = property.colorValue;
            var oldHex = '#'+oldColor.HtmlRGBA();
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
            var newColor = EditorGUI.ColorField(position,oldColor);
            if(!newHex.StartsWith('#')) newHex = '#' + newHex;
            if(newColor != oldColor) property.colorValue = newColor;
            else if(newHex != oldHex && ColorUtility.TryParseHtmlString(newHex, out newColor)){
                property.colorValue = newColor;
            }
            EditorGUI.EndProperty();
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}