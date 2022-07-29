using System;
using System.Reflection;
using Amilious.Core.Editor.Drawers;
using UnityEditor;
using UnityEngine;

namespace Amilious.Core.Editor.Modifiers {
    public abstract class AmiliousPropertyModifier : PropertyDrawer {

        private bool _hide;
        private bool _initialized;
        private RangeAttribute _range;
        private AmiliousPropertyDrawer _drawer;

        // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
        public AmiliousPropertyDrawer Drawer => _drawer;

        public bool CalledBeforeDrawer { get; private set; } = false;

        public virtual void BeforeOnGUI(SerializedProperty property, GUIContent label, bool hidden) { }

        public virtual void AfterOnGUI(SerializedProperty property, bool hidden) { }

        /// <summary>
        /// This method is used to check if the property modifier modifies the height.
        /// </summary>
        /// <param name="property">The property being drawn.</param>
        /// <param name="label">The properties label.</param>
        /// <param name="hidden">The property is hidden if true.</param>
        /// <returns>The amount that the height is modified by.</returns>
        public virtual float ModifyHeight(SerializedProperty property, GUIContent label, bool hidden) { return 0;}

        public virtual bool ShouldCancelDraw(SerializedProperty property) => false;

        private void Initialize(SerializedProperty property) {
            _hide = ShouldCancelDraw(property);
            if(_initialized) return;
            _initialized = true;
            _range = fieldInfo.GetCustomAttribute<RangeAttribute>();
            if(Drawer != null || !AmiliousPropertyDrawer.AllAmiliousDrawers.TryGetValue(property.type, out var drawerType)) return;
            CalledBeforeDrawer = true;
            _drawer = (AmiliousPropertyDrawer)Activator.CreateInstance(drawerType);
            //set the fieldInfo and attribute
            var fieldI = GetType().GetField("m_FieldInfo", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldI?.SetValue(Drawer,fieldInfo);
        }
        
        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            Initialize(property);
            if(CalledBeforeDrawer) { Drawer.OnGUI(position,property,label); return; }
            if(Drawer != null) return;
            BeforeOnGUI(property,label,_hide);
            //check for rane attribute
            if(!_hide) {
                if(_range != null) {
                    if(property.propertyType == SerializedPropertyType.Integer)
                        EditorGUI.IntSlider(position, property, (int)_range.min, (int)_range.max, label);
                    else if(property.propertyType == SerializedPropertyType.Float) 
                        EditorGUI.Slider(position, property, _range.min, _range.max, label);
                    else EditorGUI.PropertyField(position, property, label, true);
                }
                else EditorGUI.PropertyField(position, property, label, true);
            }
            AfterOnGUI(property, _hide);
        }

        public sealed override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            Initialize(property);
            if(CalledBeforeDrawer) { return Drawer.GetPropertyHeight(property,label); }
            var height = _hide? 0 : base.GetPropertyHeight(property, label);
            height += ModifyHeight(property, label, _hide);
            return height;
        }
        
    }
}