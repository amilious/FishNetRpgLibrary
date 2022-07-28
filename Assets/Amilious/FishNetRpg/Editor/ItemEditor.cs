using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Amilious.FishNetRpg.Items;
using Amilious.Inspector.Editor.Editors;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Amilious.FishNetRpg.Editor {
    [CustomEditor(typeof(Item), true)]
    public class ItemEditor : AmiliousScriptableObjectEditor {

        private static bool _initialized;
        private static Type _renderType;
        private static MethodInfo _renderMethod;
        private static GUIStyle _style;
        private SerializedProperty _displayName;
        private SerializedProperty _maxStack;
        private SerializedProperty _pickup;
        private SerializedProperty _rarity;
        private SerializedProperty _pickupRequirements;
        private SerializedProperty _inventoryAppliedModifiers;


        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height) {
            Initialize();
            var item = target as Item;
            if(item == null||item.Icon==null||_renderMethod==null) return base.RenderStaticPreview(assetPath, subAssets, width, height);
            var texture = _renderMethod.Invoke("RenderStaticPreview",
                new object[] { item.Icon, Color.white, width, height }) as Texture2D;
            return texture == null ? base.RenderStaticPreview(assetPath, subAssets, width, height) : texture;
        }

        public static void Initialize() {
            if(_initialized) return;
            _initialized = true;
            _renderType ??= GetType("UnityEditor.SpriteUtility");
            if(_renderType == null) return;
            _renderMethod ??= _renderType.GetMethod("RenderStaticPreview",new [] { typeof(Sprite),typeof(Color),typeof(int),typeof(int) });
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

        private static GUIStyle Style {
            get {
                _style ??= new GUIStyle(EditorStyles.largeLabel){ richText = true, wordWrap = true};
                return _style;
            }
        }

        public Dictionary<string, List<SerializedProperty>> tabs = new Dictionary<string, List<SerializedProperty>>();
        
        public void AddToTab(string tab, SerializedProperty property) {
            tabs.TryAdd(tab, new List<SerializedProperty>());
            tabs[tab].Add(property);
            DontDraw(property.name);
        }

        protected override void BeforeDefault() {
            base.BeforeDefault();
            tabs.Clear();
            //get properties
            if(_displayName == null) {
                _displayName = serializedObject.FindProperty("displayName");
                _maxStack = serializedObject.FindProperty("maxStack");
                _pickup = serializedObject.FindProperty("pickup");
                _rarity = serializedObject.FindProperty("rarity");
                _pickupRequirements = serializedObject.FindProperty("pickupRequirements");
                _inventoryAppliedModifiers = serializedObject.FindProperty("inventoryAppliedModifiers");
            }

            //draw properties
            DontDraw("displayName", "icon", "maxStack", "pickup", "rarity");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField(serializedObject.FindProperty("icon"), typeof(Sprite), GUIContent.none,GUILayout.Height(80), GUILayout.Width(80));
            EditorGUILayout.BeginVertical();
            EditorGUIUtility.labelWidth -= 35;
            EditorGUILayout.PropertyField(_displayName);
            if(_maxStack.intValue>1) EditorGUILayout.PropertyField(_maxStack);
            else EditorGUILayout.PropertyField(_maxStack, new GUIContent("Not Stackable"));
            EditorGUILayout.PropertyField(_pickup);
            var rarity = _rarity.objectReferenceValue as ItemRarity;
            if(rarity==null)EditorGUILayout.PropertyField(_rarity);
            else {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Rarity",new GUIStyle(EditorStyles.label){normal = {textColor = rarity.Color}}, 
                    GUILayout.Width(EditorGUIUtility.labelWidth));
                EditorGUILayout.PropertyField(_rarity,GUIContent.none);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUIUtility.labelWidth += 35;
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            AddTabs();
        }

        public virtual void AddTabs() {
            AddToTab("Item",_pickupRequirements);
            AddToTab("Item",_inventoryAppliedModifiers);
        }

        protected override void AfterDefault() {
            //draw tabs
            var prefName = target.GetType().Name;
            var visibleTab = Mathf.Min(EditorPrefs.GetInt(prefName),tabs.Count-1);
            var tabNames = tabs.Keys.ToArray();
            EditorGUILayout.Separator();
            EditorGUI.BeginChangeCheck();
            EditorPrefs.SetInt(prefName, GUILayout.Toolbar(visibleTab, tabNames, new GUIStyle(EditorStyles.miniButtonMid){fontSize = 10, fontStyle = FontStyle.Bold}));
            if(EditorGUI.EndChangeCheck()) GUI.FocusControl(null);
            EditorGUI.indentLevel = 1;
            foreach(var property in tabs[tabNames[visibleTab]]) {
                EditorGUILayout.PropertyField(property);
            }
            EditorGUI.indentLevel = 0;
        }
        
    }
}