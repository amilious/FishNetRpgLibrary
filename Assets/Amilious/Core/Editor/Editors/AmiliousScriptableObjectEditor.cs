////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Text;
using UnityEditor;
using UnityEngine;

namespace Amilious.Core.Editor.Editors {
    
    /// <summary>
    /// This editor is used as the default editor for amilious scriptable objects.
    /// </summary>
    [CustomEditor(typeof(AmiliousScriptableObject),true, isFallback = true)]
    public class AmiliousScriptableObjectEditor : AmiliousEditor {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private static GUIStyle _boxStyle;
        private readonly StringBuilder _sb = new ();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get and cache the box style.
        /// </summary>
        private static GUIStyle BoxStyle => _boxStyle ?? 
                new GUIStyle(EditorStyles.helpBox){ richText = true, wordWrap = true, fontStyle = FontStyle.Bold};
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void BeforeDefaultDraw() {
            if(target is not AmiliousScriptableObject item) return;
            _sb.Clear();
            _sb.Append("Amilious Scriptable Object: ");
            _sb.Append("<color=#ff8888>").Append(item.GetType().Name).Append("</color>").Append('\n');
            _sb.Append("Item Id: <color=#88FF88>" + item.Id + "</color>\n");
            if(!item.IsInResourceFolder && item.NeedsToBeLoadableById) {
                _sb.Append("<color=#FF8888>This asset needs to be moved to a <color=#88FFFF>Resource/</color>");
                _sb.Append(" folder or a subfolder within a <color=#88FFFF>Resource/</color>");
                _sb.Append(" folder so that it can be loaded at runtime!</color>");
            }
            else if(item.NeedsToBeLoadableById)
                _sb.Append("Resource Path: <color=#8888ff>").Append(item.ResourcePath).Append("</color>");
            else _sb.Append("<color=#FF8888>This object is configured to be loadable by id!</color>");
            if(item.NeedsToBeLoadableById) GUILayout.Box(_sb.ToString(),BoxStyle);
            else GUILayout.Box(new GUIContent(_sb.ToString(),
                $"Override {nameof(item.NeedsToBeLoadableById)} if you need this item to be loadable by id."), BoxStyle);
            GUILayout.Space(5);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}