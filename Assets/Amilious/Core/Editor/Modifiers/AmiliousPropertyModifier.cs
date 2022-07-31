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

using System;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using Amilious.Core.Attributes;
using Amilious.Core.Editor.Drawers;

namespace Amilious.Core.Editor.Modifiers {

    /// <summary>
    /// This class is used to add a modify a property.  This variation will also handle casting the attribute.
    /// </summary>
    public abstract class AmiliousPropertyModifier<T> : AmiliousPropertyModifier where T : AmiliousModifierAttribute {
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private T _casted;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the attribute associated with this modifier.
        /// </summary>
        protected T Attribute {
            get {
                if(_casted != null) return _casted;
                _casted = attribute as T;
                return _casted;
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }

    /// <summary>
    /// This class is used to add a modify a property.
    /// </summary>
    public abstract class AmiliousPropertyModifier : PropertyDrawer {

        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        private bool _hide;
        private bool _initialized;
        private RangeAttribute _range;
        private AmiliousPropertyDrawer _drawer;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the property that the modifier is applied to.
        /// </summary>
        // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter
        public AmiliousPropertyDrawer Drawer => _drawer;
        
        /// <summary>
        /// This attribute is used to indicate if the modifier is called before drawing.
        /// </summary>
        public bool CalledBeforeDrawer { get; private set; } = false;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is called before OnGUI is called.
        /// </summary>
        /// <param name="property">The property that will be drawn.</param>
        /// <param name="label">The properties label.</param>
        /// <param name="hidden">If true the property has been marked as hidden, otherwise false.</param>
        public virtual void BeforeOnGUI(SerializedProperty property, GUIContent label, bool hidden) { }

        /// <summary>
        /// This method is called after OnGUI.
        /// </summary>
        /// <param name="property">The property that was drawn.</param>
        /// <param name="hidden">If true the property has been marked as hidden, otherwise false.</param>
        public virtual void AfterOnGUI(SerializedProperty property, bool hidden) { }

        /// <summary>
        /// This method is used to check if the property modifier modifies the height.
        /// </summary>
        /// <param name="property">The property being drawn.</param>
        /// <param name="label">The properties label.</param>
        /// <param name="hidden">The property is hidden if true.</param>
        /// <returns>The amount that the height is modified by.</returns>
        public virtual float ModifyHeight(SerializedProperty property, GUIContent label, bool hidden) { return 0;}

        /// <summary>
        /// This method will be called to check if the drawing of the property should be canceled.
        /// </summary>
        /// <param name="property">The property in question.</param>
        /// <returns>True if the drawing of the property should be canceled, otherwise false.</returns>
        public virtual bool ShouldCancelDraw(SerializedProperty property) => false;

        /// <inheritdoc />
        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            Initialize(property);
            if(CalledBeforeDrawer) { Drawer.OnGUI(position,property,label); return; }
            if(Drawer != null) return;
            BeforeOnGUI(property,label,_hide);
            //check for range attribute
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

        /// <inheritdoc />
        public sealed override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            Initialize(property);
            if(CalledBeforeDrawer) { return Drawer.GetPropertyHeight(property,label); }
            var height = _hide? 0 : base.GetPropertyHeight(property, label);
            height += ModifyHeight(property, label, _hide);
            return height;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private And Protected Methods //////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to initialize the property modifier.
        /// </summary>
        /// <param name="property">The property that the modifier is for.</param>
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
            Initialize();
        }

        /// <summary>
        /// This method is called during initialization.
        /// </summary>
        protected virtual void Initialize() { }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}