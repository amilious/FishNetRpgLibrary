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

using System.Linq;
using Amilious.Core.Attributes;
using Amilious.Core.Editor.Extensions;
using UnityEditor;
using UnityEngine;
using Amilious.Core.Extensions;

namespace Amilious.Core.Editor.Drawers {
    
    /// <summary>
    /// This drawer is used instead of the default color drawer.
    /// </summary>
    [CustomPropertyDrawer(typeof(Color))]
    [CustomPropertyDrawer(typeof(AmiliousColorAttribute))]
    public class AmiliousColorDrawer : AmiliousPropertyDrawer {
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void AmiliousOnGUI(Rect position, SerializedProperty property, GUIContent label) {

            var att = property.GetAttributes<AmiliousColorAttribute>().FirstOrDefault()??new AmiliousColorAttribute();
            
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
            var newColor = EditorGUI.ColorField(position,GUIContent.none, oldColor,true,att.ShowAlpha,att.UseHDR);
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