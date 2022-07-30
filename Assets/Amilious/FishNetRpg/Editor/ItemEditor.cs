using System;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using Amilious.Core.Extensions;
using Amilious.FishNetRpg.Items;
using Object = UnityEngine.Object;
using Amilious.Core.Editor.Editors;
using Amilious.FishNetRpg.Attributes;

namespace Amilious.FishNetRpg.Editor {
    
    /// <summary>
    /// This class is used to draw <see cref="Item"/>s in the inspector.
    /// </summary>
    [CustomEditor(typeof(Item), true)]
    public class ItemEditor : AmiliousScriptableObjectEditor {

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private static GUIStyle _style;
        private SerializedProperty _displayName;
        private SerializedProperty _icon;
        private SerializedProperty _maxStackSize;
        private SerializedProperty _pickup;
        private SerializedProperty _rarity;
        private Texture2D _iconBadge;
        private ItemEditorBadgeAttribute _badgeAttribute;
        private bool _loadedAttribute;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is used to get the badge icon for the item type.
        /// </summary>
        private Texture2D IconBadge {
            get {
                if(_iconBadge) return _iconBadge;
                if(_loadedAttribute) return null;
                _loadedAttribute = true;
                _badgeAttribute  = target.GetType().GetCustomAttribute<ItemEditorBadgeAttribute>();
                if(_badgeAttribute==null) return null;
                _iconBadge = Resources.Load<Texture2D>(_badgeAttribute.IconResourcePath);
                _iconBadge ??= EditorGUIUtility.IconContent(_badgeAttribute.IconResourcePath).image as Texture2D;
                if(_iconBadge == null) {
                    Debug.LogWarningFormat("<color=#FF8888>Invalid Icon Resource Path given to the {0} on {1}</color>!\n{2}",
                        nameof(ItemEditorBadgeAttribute).SetColor("8888FF"),
                        target.GetType().FullName.SetColor("8888FF"),_badgeAttribute.IconResourcePath.SetColor("FFFF88"));
                }
                return _iconBadge; 
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height) {
            Initialize();
            var item = target as Item;
            if(item == null||item.Icon==null) 
                return base.RenderStaticPreview(assetPath, subAssets, width, height);
            var texture = GetSpritePreview(item.Icon, Color.white, width, height);
            return texture == null ? base.RenderStaticPreview(assetPath, subAssets, width, height) : 
                IconBadge == null? texture : texture.AddWatermark(IconBadge,5);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void Initialize() {
            _displayName = serializedObject.FindProperty("displayName");
            _icon = serializedObject.FindProperty("icon");
            _maxStackSize = serializedObject.FindProperty("maxStackSize");
            _pickup = serializedObject.FindProperty("pickup");
            _rarity = serializedObject.FindProperty("rarity");
            SkipDraw(_displayName, _icon, _maxStackSize, _pickup, _rarity);
        }

        /// <inheritdoc />
        protected override void BeforeDefaultDraw() {
            base.BeforeDefaultDraw();
            //draw properties
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
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}