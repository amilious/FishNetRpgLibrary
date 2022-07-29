using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using Amilious.Core.Attributes;
using Amilious.Core.Editor.Extensions;

namespace Amilious.Core.Editor.Editors {
    
    public class AmiliousEditor : UnityEditor.Editor {
        
        private GUIStyle _style;
        private GUIStyle _tabButtonStyle;
        private bool _initialized;
        
        private readonly Dictionary<string, List<TabInfo>> _tabs = new Dictionary<string, List<TabInfo>>();
        
        private readonly List<string> _dontDraw = new List<string>();
        private readonly List<LinkInfo> _links = new List<LinkInfo>();

        private const string UNITY_ASSET_STORE_ICON = "AssetStore Icon";
        private const string WEBSITE_ICON = "BuildSettings.Web";
        
        private void InitializeAmiliousEditor() {
            if(_initialized) return;
            _initialized = true;
            _style = new GUIStyle { fixedHeight = 12, alignment = TextAnchor.MiddleLeft,
                fontSize = 10, fontStyle = FontStyle.Bold,
                //_style.fixedWidth = 85;
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                normal = { textColor = Color.white }
            };
            //hide the script
            _dontDraw.Add("m_Script");
            _tabButtonStyle ??= new GUIStyle(EditorStyles.miniButtonMid) 
                { fontSize = 10, fontStyle = FontStyle.Bold };
            //check link attributes
            var linkAttributes = target.GetType().
                GetCustomAttributes(typeof(EditorLinkAttribute), true).Cast<EditorLinkAttribute>();
            foreach(var attribute in linkAttributes)
                AddLinkButton(attribute.ToolTip, attribute.IconResourcePath, attribute.Link, attribute.LinkName);
            //get tabs
            var iterator = serializedObject.GetIterator( );
            var enterChildren = true;
            while (iterator.NextVisible(enterChildren)) {
                enterChildren = false;
                var attributes = 
                    iterator.GetAttributes(typeof(AmiliousTabAttribute),false).Cast<AmiliousTabAttribute>();
                foreach(var attribute in attributes)
                    AddToTab(attribute.TabName,serializedObject.FindProperty(iterator.name), attribute.Priority);
            }
            Initialize();
        }

        private struct LinkInfo {
            public GUIContent GUIContent { get; set; }
            public string Link { get; set; }
        }

        private struct TabInfo {
            public string TabName { get; set; }
            public SerializedProperty Property { get; set; }
            public int Priority { get; set; }
        }

        public override void OnInspectorGUI() {
            InitializeAmiliousEditor();
            DrawLinks();
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            BeforeDefault();
            DrawPropertiesExcluding(serializedObject,_dontDraw.ToArray());
            DrawTabs();
            AfterDefault();
            if (EditorGUI.EndChangeCheck()){ serializedObject.ApplyModifiedProperties();}
        }

        private static Type _renderType;
        private static MethodInfo _renderMethod;
        private static bool _initializedGetSpritePreview;
        public static Texture2D GetSpritePreview(Sprite icon, Color color, int width, int height) {
            if(!_initializedGetSpritePreview) {
                _initializedGetSpritePreview = true;
                _renderType ??= GetType("UnityEditor.SpriteUtility");
                if(_renderType == null) return null;
                _renderMethod ??= _renderType.GetMethod("RenderStaticPreview",
                    new[] { typeof(Sprite), typeof(Color), typeof(int), typeof(int) });
            }
            if(_renderMethod == null) return null;
            return _renderMethod.Invoke("RenderStaticPreview", parameters:
                new object[] { icon, color, width, height }) as Texture2D;
        }

        protected void SkipDraw(params string[] propertyName) => _dontDraw.AddRange(propertyName);

        protected void SkipDraw(params SerializedProperty[] property) =>
            SkipDraw(property.Select(x => x.name).ToArray());
        
        protected virtual void BeforeDefault(){}
        protected virtual void AfterDefault() {}
        protected virtual void Initialize() { }

        private void DrawLinks() {
            if(_links.Count == 0) return;
            GUILayout.BeginHorizontal("Project Links:",EditorStyles.helpBox, GUILayout.Height(20));
            GUILayout.FlexibleSpace();
            //buttons
            var options = new [] {GUILayout.ExpandWidth(false), GUILayout.MaxHeight(22) };
            foreach(var link in _links) 
                if(GUILayout.Toggle(false,link.GUIContent,_style,options)) Application.OpenURL(link.Link);
            GUILayout.EndHorizontal();
        }
        
        private static Type GetType(string typeName) {
            var type = Type.GetType(typeName);
            if(type!=null) return type;
            if(typeName.Contains(".")) {
                var assemblyName = typeName[..typeName.IndexOf('.')];
                var assembly = Assembly.Load(assemblyName);
                if(assembly==null) return null;
                type=assembly.GetType(typeName);
                if(type!=null) return type;
            }
            var currentAssembly = Assembly.GetExecutingAssembly();
            var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
            foreach(var assemblyName in referencedAssemblies) {
                var assembly = Assembly.Load(assemblyName);
                if(assembly == null) continue;
                type=assembly.GetType(typeName);
                if(type!=null) return type;
            }
            return null;
        }
        
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
                EditorGUILayout.PropertyField(property.Property);
            } 
            EditorGUI.indentLevel = 0;
        }

        protected void AddToTab(string tab, SerializedProperty property, int priority = 0) {
            _tabs.TryAdd(tab, new List<TabInfo>());
            _tabs[tab].Add( new TabInfo{TabName = tab, Property = property, Priority = priority});
            _tabs[tab].Sort(SortTabs);
            SkipDraw(property);
        }

        private int SortTabs(TabInfo a, TabInfo b) {
            return b.Priority - a.Priority;
        }

        protected bool AddLinkButton(string toolTip, string iconResourcePath, string link, string linkName = null) {
            var icon = Resources.Load<Texture>(iconResourcePath);
            if(icon == null) icon = EditorGUIUtility.IconContent(iconResourcePath).image;
            if(icon != null) return AddLinkButton(toolTip, icon, link, linkName);
            Debug.LogErrorFormat("Unable to load a Texture from the path \"{0}\"!",iconResourcePath);
            return false;
        }

        protected bool AddLinkButton(string toolTip, Texture icon, string link, string linkName = null) {
            if(string.IsNullOrWhiteSpace(link)||icon==null) return false;
            _links.Add(new LinkInfo{Link = link, 
                GUIContent = new GUIContent(linkName, icon, toolTip)});
            return true;
        }
        
    }
}