using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Amilious.Core.Editor.Editors {
    
    public abstract class AmiliousBaseEditor : UnityEditor.Editor {

        protected virtual string Youtube { get; } = null;
        protected virtual string Discord { get; } = null;
        protected virtual string AssetStore { get; } = null;
        protected virtual string Website { get; } = null;
        
        private static Texture _discordLogo;
        private static Texture _youtubeLogo;
        private static Texture _assetStoreLogo;
        private static Texture _websiteIcon;
        private GUIStyle _style;
        private GUIStyle _tabButtonStyle;
        private GUIContent _assetStore;
        private GUIContent _website;
        private GUIContent _discord;
        private GUIContent _youtube;
        private bool _initialized;
        
        private readonly Dictionary<string, List<SerializedProperty>> _tabs = new Dictionary<string, List<SerializedProperty>>();
        
        private readonly List<string> _dontDraw = new List<string>();
        
        private void OnEnable() {
            _style = new GUIStyle { fixedHeight = 12, alignment = TextAnchor.MiddleLeft,
                fontSize = 10, fontStyle = FontStyle.Bold,
                //_style.fixedWidth = 85;
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                normal = { textColor = Color.white }
            };
            _tabButtonStyle ??= new GUIStyle(EditorStyles.miniButtonMid) { fontSize = 10, fontStyle = FontStyle.Bold };
            _discordLogo ??= Resources.Load<Texture>("Icons/discord");
             _youtubeLogo ??= Resources.Load<Texture>("Icons/youtube");
            _assetStoreLogo ??=  EditorGUIUtility.IconContent("AssetStore Icon").image;
            _websiteIcon ??=  EditorGUIUtility.IconContent("BuildSettings.Web").image;
            _discord ??= new GUIContent("Discord", _discordLogo, "Click to go to the discord server.");
            _website ??= new GUIContent("Website", _websiteIcon, "Click to go to the Amilious website.");
            _assetStore ??= new GUIContent("Asset Store", _assetStoreLogo, "Click to go to the unity asset store.");
            _youtube ??= new GUIContent("Youtube", _youtubeLogo, "Click to go to youtube.");
        }

        public override void OnInspectorGUI() {
            _dontDraw.Clear();
            _tabs.Clear();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            //buttons
            var options = new [] {GUILayout.ExpandWidth(false), GUILayout.MaxHeight(22) };
            if(Website!=null) if(GUILayout.Toggle(false, _website,_style, options)){ Application.OpenURL(Website);}
            if(AssetStore!=null)if(GUILayout.Toggle(false, _assetStore, _style, options)){ Application.OpenURL(AssetStore);}
            if(Discord!=null)if(GUILayout.Toggle(false,_discord,_style, options)){ Application.OpenURL(Discord);}
            if(Youtube!=null)if(GUILayout.Toggle(false,_youtube,_style, options)){ Application.OpenURL(Youtube);}
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            BeforeDefault();
            _dontDraw.Add("m_Script");
            DrawPropertiesExcluding(serializedObject,_dontDraw.ToArray());
            DrawTabs();
            AfterDefault();
            if (EditorGUI.EndChangeCheck()){ serializedObject.ApplyModifiedProperties();}
        }

        protected void SkipDraw(params string[] propertyName) => _dontDraw.AddRange(propertyName);

        protected void SkipDraw(params SerializedProperty[] property) =>
            SkipDraw(property.Select(x => x.name).ToArray());
        
        protected virtual void BeforeDefault(){}
        protected virtual void AfterDefault() {}

        private void DrawTabs() {
            if(_tabs.Count == 0) return;
            //draw tabs
            var prefName = target.GetType().Name;
            var visibleTab = Mathf.Min(EditorPrefs.GetInt(prefName),_tabs.Count-1);
            var tabNames = _tabs.Keys.ToArray();
            EditorGUILayout.Separator();
            if(_tabs.Count > 1) {
                EditorGUI.BeginChangeCheck();
                EditorPrefs.SetInt(prefName, GUILayout.Toolbar(visibleTab, tabNames, _tabButtonStyle));
                if(EditorGUI.EndChangeCheck()) GUI.FocusControl(null);
            }
            else EditorGUILayout.LabelField(tabNames[visibleTab], EditorStyles.largeLabel);
            EditorGUI.indentLevel = 1;
            foreach(var property in _tabs[tabNames[visibleTab]]) {
                EditorGUILayout.PropertyField(property);
            }
            EditorGUI.indentLevel = 0;
        }

        public void AddToTab(string tab, SerializedProperty property) {
            _tabs.TryAdd(tab, new List<SerializedProperty>());
            _tabs[tab].Add(property);
            SkipDraw(property);
        }

    }
}