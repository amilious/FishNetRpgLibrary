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
            else _sb.Append("Resource Path: <color=#8888ff>").Append(item.ResourcePath).Append("</color>");
            GUILayout.Box(_sb.ToString(),BoxStyle);
            GUILayout.Space(5);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}