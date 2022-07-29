using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Amilious.Core.Attributes;
using Amilious.Core.Editor.Extensions;
using Amilious.Core.Editor.Modifiers;
using UnityEditor;
using UnityEngine;

namespace Amilious.Core.Editor.Drawers {
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
        
        private bool _initialized;
        private bool _hideDraw;
        public readonly Dictionary<AmiliousModifierAttribute, AmiliousPropertyModifier> Modifiers = new (); 

        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            //initialize the property if not yet done.
            Initialize(property);
            
            //call before draw
            foreach(var modifier in Modifiers.Values) modifier.BeforeOnGUI(property,label,_hideDraw);
            
            //draw gui
            if(!_hideDraw) AmiliousOnGUI(position, property, label);
            
            //call after draw
            foreach(var modifier in Modifiers.Values) modifier.AfterOnGUI(property,_hideDraw);

        }

        public sealed override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            Initialize(property);
            var height = _hideDraw? 0f: AmiliousGetPropertyHeight(property, label);
            foreach(var modifier in Modifiers.Values) height += modifier.ModifyHeight(property, label, _hideDraw);
            return height;
        }

        protected virtual float AmiliousGetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label);
        }

        /// <inheritdoc cref="OnGUI"/>
        protected virtual void AmiliousOnGUI(Rect position, SerializedProperty property, GUIContent label) {
            base.OnGUI(position,property,label);
        }

        private void Initialize(SerializedProperty property) {
            if(_initialized) {
                _hideDraw = Modifiers.Values.Any(x => x.ShouldCancelDraw(property));
                return;
            }
            _initialized = true;
            StaticInitialize();
            var modifiers = fieldInfo.GetCustomAttributes(typeof(AmiliousModifierAttribute), true).Cast<AmiliousModifierAttribute>().ToList();
            foreach(var modifier in modifiers) {
                if(TryCreatePropertyModifier(modifier, out var modifierDrawer))
                    Modifiers.Add(modifier, modifierDrawer);
                if(modifierDrawer.ShouldCancelDraw(property)) _hideDraw = true;
            }
        }

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

        public static void StaticInitialize() {
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
                    var cd = drawer.GetCustomAttribute<CustomPropertyDrawer>();
                    cd.TryGetDrawersPropertyType(out var type);
                    if(!AllAmiliousDrawers.ContainsKey(type.Name)) AllAmiliousDrawers.Add(type.Name,drawer);
                }
            }

        }
        
        
    }
}