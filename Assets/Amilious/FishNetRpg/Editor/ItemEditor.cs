using System;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using Amilious.Core.Extensions;
using Amilious.FishNetRpg.Items;
using Object = UnityEngine.Object;
using Amilious.Inspector.Editor.Editors;

namespace Amilious.FishNetRpg.Editor {
    
    [CustomEditor(typeof(Item), true)]
    public class ItemEditor : AmiliousScriptableObjectEditor {

        private static bool _initialized;
        private static Type _renderType;
        private static MethodInfo _renderMethod;
        private static GUIStyle _style;
        private SerializedProperty _displayName;
        private SerializedProperty _icon;
        private SerializedProperty _maxStackSize;
        private SerializedProperty _pickup;
        private SerializedProperty _rarity;
        private SerializedProperty _pickupRequirements;
        private SerializedProperty _inventoryAppliedModifiers;
        private SerializedProperty _weight;
        private SerializedProperty _canBeTraded;
        protected Texture2D _iconBadge;

        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height) {
            Initialize();
            var item = target as Item;
            if(item == null||item.Icon==null||_renderMethod==null) 
                return base.RenderStaticPreview(assetPath, subAssets, width, height);
            var texture = _renderMethod.Invoke("RenderStaticPreview",
                new object[] { item.Icon, Color.white, width, height }) as Texture2D;
            return texture == null ? base.RenderStaticPreview(assetPath, subAssets, width, height) : 
                texture.AddWatermark(IconBadge,5);
        }

        protected virtual Texture2D IconBadge => _iconBadge ??= Resources.Load<Texture2D>("ItemBadges/ItemBadge64");
        
        private static GUIStyle Style =>
            _style ??= new GUIStyle(EditorStyles.largeLabel){ richText = true, wordWrap = true};

        public static void Initialize() {
            if(_initialized) return;
            _initialized = true;
            _renderType ??= GetType("UnityEditor.SpriteUtility");
            if(_renderType == null) return;
            _renderMethod ??= _renderType.GetMethod("RenderStaticPreview",
                new [] { typeof(Sprite),typeof(Color),typeof(int),typeof(int) });
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

        protected override void BeforeDefault() {
            base.BeforeDefault();
            //get properties
            if(_displayName == null) {
                _displayName = serializedObject.FindProperty("displayName");
                _icon = serializedObject.FindProperty("icon");
                _maxStackSize = serializedObject.FindProperty("maxStackSize");
                _pickup = serializedObject.FindProperty("pickup");
                _rarity = serializedObject.FindProperty("rarity");
                _pickupRequirements = serializedObject.FindProperty("pickupRequirements");
                _inventoryAppliedModifiers = serializedObject.FindProperty("inventoryAppliedModifiers");
                _weight = serializedObject.FindProperty("weight");
                _canBeTraded = serializedObject.FindProperty("canBeTraded");
            }

            //draw properties
            SkipDraw(_displayName, _icon, _maxStackSize, _pickup, _rarity);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField(_icon, typeof(Sprite), GUIContent.none,GUILayout.Height(80), GUILayout.Width(80));
            EditorGUILayout.BeginVertical();
            EditorGUIUtility.labelWidth -= 35;
            EditorGUILayout.PropertyField(_displayName);
            if(_maxStackSize.intValue>1) EditorGUILayout.PropertyField(_maxStackSize);
            else EditorGUILayout.PropertyField(_maxStackSize, new GUIContent("Not Stackable"));
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
            AddToTab("Item",_canBeTraded);
            AddToTab("Item",_weight);
            AddToTab("Item",_pickupRequirements);
            AddToTab("Item",_inventoryAppliedModifiers);
        }
        
    }
}