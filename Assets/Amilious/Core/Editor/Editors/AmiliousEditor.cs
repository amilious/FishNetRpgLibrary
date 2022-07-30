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
        
        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        public const string UNITY_ASSET_STORE_ICON = "AssetStore Icon";
        public const string WEBSITE_ICON = "BuildSettings.Web";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private static Type _renderType;
        private static MethodInfo _renderMethod;
        private static bool _initializedGetSpritePreview;
        private GUIStyle _style;
        private GUIStyle _tabButtonStyle;
        private bool _initialized;
        private readonly Dictionary<string, TabInfo> _tabs = new ();
        private readonly List<string> _dontDraw = new ();
        private readonly List<LinkInfo> _links = new ();
        private readonly List<string> _drawnTabs = new();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override void OnInspectorGUI() {
            InitializeAmiliousEditor();
            _drawnTabs.Clear();
            DrawLinks();
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            BeforeDefaultDraw();
            // draw default properties
            var iterator = serializedObject.GetIterator();
            var enterChildren = true;
            while (iterator.NextVisible(enterChildren)) {
                enterChildren = false;
                if (!_dontDraw.Contains<string>(iterator.name))
                    EditorGUILayout.PropertyField(iterator, true);
                else {
                    //skipped drawing the property
                    DrawTabGroup(iterator.name);
                }
            }
            //end draw default properties
            AfterDefaultDraw();
            if (EditorGUI.EndChangeCheck()){ serializedObject.ApplyModifiedProperties();}
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get a Texture2D for the given sprite.
        /// </summary>
        /// <param name="icon">The sprite that you want to make a texture from.</param>
        /// <param name="color">The color.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>A Texture2D created from the given sprite.</returns>
        protected static Texture2D GetSpritePreview(Sprite icon, Color color, int width, int height) {
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

        /// <summary>
        /// This method is used to skip drawing a property with the default property drawer.
        /// </summary>
        /// <param name="propertyName">The names of the properties that you want to skip drawing.</param>
        protected void SkipDraw(params string[] propertyName) => _dontDraw.AddRange(propertyName);

        /// <summary>
        /// This method is used to skip drawing a property with the default property drawer.
        /// </summary>
        /// <param name="property">The properties that you want to skip drawing.</param>
        protected void SkipDraw(params SerializedProperty[] property) =>
            SkipDraw(property.Select(x => x.name).ToArray());
        
        /// <summary>
        /// This method is called before drawing the default properties.
        /// </summary>
        protected virtual void BeforeDefaultDraw(){}
        
        /// <summary>
        /// This method is called after drawing the default properties.
        /// </summary>
        protected virtual void AfterDefaultDraw() {}
        
        /// <summary>
        /// This method is called to initialize data for the editor.
        /// </summary>
        protected virtual void Initialize() { }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to add a serialized property to a tab.
        /// </summary>
        /// <param name="tabInfo">The tab info.</param>
        private void AddToTab(TabInfo tabInfo) {
            _tabs[tabInfo.Property.name] = tabInfo;
            SkipDraw(tabInfo.Property);
        }

        /// <summary>
        /// This method is used to add a link button to the top of the inspector.
        /// </summary>
        /// <param name="toolTip">The tooltip that you want to used for the button.</param>
        /// <param name="iconResourcePath">The icon that you want to use for the button.</param>
        /// <param name="link">The url that should be triggered when the button is clicked.</param>
        /// <param name="linkName">The text displayed on the button.</param>
        private void AddLinkButton(string toolTip, string iconResourcePath, string link, string linkName = null) {
            var icon = Resources.Load<Texture>(iconResourcePath);
            icon ??= EditorGUIUtility.IconContent(iconResourcePath).image;
            if(icon != null) { AddLinkButton(toolTip, icon, link, linkName); return; }
            Debug.LogErrorFormat("Unable to load a Texture from the path \"{0}\"!",iconResourcePath);
        }

        /// <summary>
        /// This method is used to add a link button to the top of the inspector.
        /// </summary>
        /// <param name="toolTip">The tooltip that you want to used for the button.</param>
        /// <param name="icon">The icon that you want to use for the button.</param>
        /// <param name="link">The url that should be triggered when the button is clicked.</param>
        /// <param name="linkName">The text displayed on the button.</param>
        private void AddLinkButton(string toolTip, Texture icon, string link, string linkName = null) {
            if(string.IsNullOrWhiteSpace(link)||icon==null) return;
            _links.Add(new LinkInfo{Link = link, 
                GUIContent = new GUIContent(linkName, icon, toolTip)});
        }
        
        /// <summary>
        /// This method is used to sort tab items based on priority.
        /// </summary>
        /// <param name="a">The first item.</param>
        /// <param name="b">The second item.</param>
        /// <returns>The sorted value.</returns>
        private static int SortTabs(TabInfo a, TabInfo b) {
            return b.Order - a.Order;
        }
        
        /// <summary>
        /// This method is called to handle the drawing of tabs.
        /// </summary>
        private void DrawTabGroup(string tabPropertyName) {
            if(_drawnTabs.Contains(tabPropertyName)) return;
            if(!_tabs.ContainsKey(tabPropertyName)) return;
            //build the tab list
            var tabGroup = _tabs[tabPropertyName].TabGroup;
            var tabProperties = _tabs.Values.Where(x => x.TabGroup == tabGroup).ToList();
            foreach(var tp in tabProperties) _drawnTabs.Add(tp.Property.name);
            //get the tab group information
            var editorPrefName = $"TabGroup/{target.GetType().Name}/{tabGroup}";
            var tabs = tabProperties.GroupBy(prop => prop.TabName).
                ToDictionary(grp => grp.Key, grp => grp.ToList());
            var tabNames = tabs.Keys.ToArray();
            var currentTab = Mathf.Min(EditorPrefs.GetInt(editorPrefName), tabs.Count - 1);
            var currentTabName = tabNames[currentTab];
            //draw tabs
            
            EditorGUILayout.Separator();
            var boxStyle = new GUIStyle(EditorStyles.helpBox);
            if(tabGroup != string.Empty) {
                EditorGUILayout.BeginVertical(boxStyle);
                EditorGUILayout.LabelField(tabGroup);
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(-5f);
            }

            EditorGUILayout.BeginHorizontal(new GUIStyle(GUIStyle.none){padding = new RectOffset(2,0,0,0)});
            EditorGUI.BeginChangeCheck();
            EditorPrefs.SetInt(editorPrefName, GUILayout.Toolbar(currentTab, tabs.Keys.ToArray(), _tabButtonStyle));
            if(EditorGUI.EndChangeCheck()) GUI.FocusControl(null);
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(-5f);
            EditorGUILayout.BeginVertical(boxStyle);
            EditorGUI.indentLevel = 1;
            EditorGUILayout.BeginVertical(); 
            EditorGUILayout.Separator();
            tabs[currentTabName].Sort(SortTabs);
            foreach(var tabItem in tabs[currentTabName]) {
                EditorGUILayout.PropertyField(tabItem.Property);
            } 
            EditorGUILayout.Separator();
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel = 0;
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// This method is called to handle the drawing of links.
        /// </summary>
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
        
        /// <summary>
        /// This method is used to get a type based on the passed string.
        /// </summary>
        /// <param name="typeName">The name of the type.</param>
        /// <returns>The type.</returns>
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
        
        /// <summary>
        /// When unity validates this inspector it should reinitialize.
        /// </summary>
        private void OnValidate() { _initialized = false; }

        /// <summary>
        /// This method is called to initialize the editor.
        /// </summary>
        private void InitializeAmiliousEditor() {
            if(_initialized) return;
            _initialized = true;
            _dontDraw.Clear();
            _tabs.Clear();
            _links.Clear();
            _style = new GUIStyle { fixedHeight = 12, alignment = TextAnchor.MiddleLeft,
                fontSize = 10, fontStyle = FontStyle.Bold,
                //_style.fixedWidth = 85;
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                normal = { textColor = Color.white }
            };
            //hide the script
            _dontDraw.Add("m_Script");
            _tabButtonStyle ??= new GUIStyle(EditorStyles.miniButtonMid) {
                fontSize = 10, fontStyle = FontStyle.Bold,
            };
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
                    AddToTab(new TabInfo(attribute,serializedObject.FindProperty(iterator.name)));
            }
            Initialize();
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}