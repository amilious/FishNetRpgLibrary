using System;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Reflection;
using Amilious.Core.Attributes;
using System.Collections.Generic;
using Amilious.Core.Editor.Extensions;
using Amilious.Core.Editor.Modifiers;

namespace Amilious.Core.Editor.Drawers {
    
    /// <summary>
    /// This is a helper class to make property drawers.
    /// </summary>
    public abstract class AmiliousPropertyDrawer : PropertyDrawer {

        #region Static Variables ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This variable is used to check if the static initializer has already been ran.
        /// </summary>
        private static bool _staticInitialized;
        
        /// <summary>
        /// This dictionary contains all of the property modifiers.
        /// </summary>
        public static readonly Dictionary<Type, Type> AllPropertyModifiers = new ();
        
        /// <summary>
        /// This dictionary contains all of the drawers.
        /// </summary>
        public static readonly Dictionary<string, Type> AllAmiliousDrawers = new ();
        
        #endregion
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to store if the drawer has been initialized or not.
        /// </summary>
        private bool _initialized;
        
        /// <summary>
        /// This field is used to store if the drawer should be drawn or not.
        /// </summary>
        private bool _hideDraw;
        
        /// <summary>
        /// This dictionary is used to store all of the modifiers that are applied to a property.
        /// </summary>
        private readonly Dictionary<AmiliousModifierAttribute, AmiliousPropertyModifier> _modifiers = new ();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Sealed Override Methods ////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            //initialize the property if not yet done.
            InitializeDrawer(property);
            
            //call before draw
            foreach(var modifier in _modifiers.Values) modifier.BeforeOnGUI(property,label,_hideDraw);
            
            //draw gui
            if(!_hideDraw) AmiliousOnGUI(position, property, label);
            
            //call after draw
            foreach(var modifier in _modifiers.Values) modifier.AfterOnGUI(property,_hideDraw);

        }

        /// <inheritdoc />
        public sealed override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            InitializeDrawer(property);
            var height = _hideDraw? 0f: AmiliousGetPropertyHeight(property, label);
            foreach(var modifier in _modifiers.Values) height += modifier.ModifyHeight(property, label, _hideDraw);
            return height;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc cref="GetPropertyHeight"/>
        protected virtual float AmiliousGetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label);
        }

        /// <inheritdoc cref="OnGUI"/>
        protected virtual void AmiliousOnGUI(Rect position, SerializedProperty property, GUIContent label) {
            base.OnGUI(position,property,label);
        }

        /// <summary>
        /// This method is called to initialize the drawer.
        /// </summary>
        protected virtual void Initialize() { }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to reinitialize the property drawers.
        /// </summary>
        [MenuItem("Amilious/Editor/Reinitialize Property Drawers")]
        private static void ReInitialize() {
            AllAmiliousDrawers.Clear();
            AllPropertyModifiers.Clear();
            _staticInitialized = false;
            StaticInitialize();
            Debug.Log(AmiliousCore.MakeTitle("Reinitialized Amilious Property Drawers"));
        }
        
        /// <summary>
        /// This method is used to initialize the drawer.
        /// </summary>
        /// <param name="property">The property that the drawer is for.</param>
        private void InitializeDrawer(SerializedProperty property) {
            if(_initialized) {
                _hideDraw = _modifiers.Values.Any(x => x.ShouldCancelDraw(property));
                return;
            }
            _initialized = true;
            StaticInitialize();
            var modifiers = fieldInfo.GetCustomAttributes(typeof(AmiliousModifierAttribute), true).Cast<AmiliousModifierAttribute>().ToList();
            foreach(var modifier in modifiers) {
                if(TryCreatePropertyModifier(modifier, out var modifierDrawer))
                    _modifiers.Add(modifier, modifierDrawer);
                if(modifierDrawer.ShouldCancelDraw(property)) _hideDraw = true;
            }
            Initialize();
        }

        /// <summary>
        /// This method is used to try create a property modifier.
        /// </summary>
        /// <param name="modifierAttribute">The attribute that you want to get the modifier for.</param>
        /// <param name="modifier">The modifier for the given attribute.</param>
        /// <returns>True if able to get a modifier for the given attribute.</returns>
        private bool TryCreatePropertyModifier(AmiliousModifierAttribute modifierAttribute,
            out AmiliousPropertyModifier modifier) {
            modifier = null;
            if(modifierAttribute == null) return false;
            if(!AllPropertyModifiers.TryGetValue(modifierAttribute.GetType(), out var modifierType)) {
                Debug.Log("The attribute type is not in the static dictionary!");
                return false;
            }
            //crete instance
            modifier = (AmiliousPropertyModifier)Activator.CreateInstance(modifierType);
            const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            //set the fieldInfo for the property
            var fieldI = GetType().GetField("m_FieldInfo", bindingFlags);
            fieldI?.SetValue(modifier,fieldInfo);
            //set the attribute associated with the modifier
            var fieldA = GetType().GetField("m_Attribute", bindingFlags);
            fieldA?.SetValue(modifier,modifierAttribute);
            //set the drawer value
            var fieldD = GetType().GetField("_drawer", bindingFlags);
            fieldD?.SetValue(modifier,this);
            return true;
        }

        /// <summary>
        /// This method is used to initialize the static fields for amilious drawers.
        /// </summary>
        private static void StaticInitialize() {
            if(_staticInitialized) return;
            _staticInitialized = true;
            
            //look in all assemblies
            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                
                //get custom property modifiers that pass a modifier attribute
                var customDrawers = assembly.GetTypes().Where(t=>t.IsDefined(typeof(CustomPropertyDrawer),
                    false)&& t.IsSubclassOf(typeof(AmiliousPropertyModifier))).ToList();
            
                //build the modifiers dictionary
                foreach(var drawer in customDrawers) {
                    var cd = drawer.GetCustomAttribute<CustomPropertyDrawer>();
                    if(!cd.TryGetDrawersPropertyType(out var type)) 
                        Debug.Log("Unable to get the type of the property drawer!");
                    else {
                        if(!type.IsSubclassOf(typeof(AmiliousModifierAttribute))) continue;
                        if(!AllPropertyModifiers.ContainsKey(type)) AllPropertyModifiers.Add(type, drawer);
                    }
                
                }
                
                //build the drawer's dictionary
                var amiliousDrawers = assembly.GetTypes().Where(t=>t.IsSubclassOf(
                    typeof(AmiliousPropertyDrawer))&& t.IsDefined(typeof(CustomPropertyDrawer),false)).ToList();
                foreach(var drawer in amiliousDrawers) {
                    var cds = drawer.GetCustomAttributes<CustomPropertyDrawer>();
                    foreach(var cd in cds){
                        cd.TryGetDrawersPropertyType(out var type);
                        if(!AllAmiliousDrawers.ContainsKey(type.Name)) AllAmiliousDrawers.Add(type.Name, drawer);
                    }
                }
            }

        }
        
        /// <summary>
        /// This is a static constructor that will initialize the static data the first time this class is referenced.
        /// </summary>
        static AmiliousPropertyDrawer() => StaticInitialize();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}