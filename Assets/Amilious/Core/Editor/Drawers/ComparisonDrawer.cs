using System.Linq;
using Amilious.Core.Attributes;
using Amilious.Core.Editor.Extensions;
using Amilious.Core.Extensions;
using UnityEditor;
using UnityEngine;

namespace Amilious.Core.Editor.Drawers {
    
    
    //[CustomPropertyDrawer(typeof(ComparisonMethod<>))]
    public class ComparisonDrawer : AmiliousPropertyDrawer {
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void AmiliousOnGUI(Rect position, SerializedProperty property, GUIContent label) {

            var compare = property.FindPropertyRelative("compareType");
            var delta =property.FindPropertyRelative("approximateDelta");

            EditorGUI.PropertyField(position, compare, label);

            /*var oldColor = property.colorValue;
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
            var newColor = EditorGUI.ColorField(position,null,oldColor,true,att.ShowAlpha,att.UseHDR);
            if(!newHex.StartsWith('#')) newHex = '#' + newHex;
            if(newColor != oldColor) property.colorValue = newColor;
            else if(newHex != oldHex && ColorUtility.TryParseHtmlString(newHex, out newColor)){
                property.colorValue = newColor;
            }
            EditorGUI.EndProperty();*/
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}