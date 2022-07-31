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
//  Website:        http://www.amilious,comUnity          Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious since 2022                              //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

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