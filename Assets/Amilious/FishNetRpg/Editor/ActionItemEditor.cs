using Amilious.FishNetRpg.Items;
using UnityEditor;
using UnityEngine;

namespace Amilious.FishNetRpg.Editor {
    
    [CustomEditor(typeof(ActionItem))]
    public class ActionItemEditor : ItemEditor {
        
        private SerializedProperty _consumable;
        private SerializedProperty _cooldown;
        private SerializedProperty _cooldownGroups;
        private SerializedProperty _actionRequirements;
        private SerializedProperty _triggerAppliedModifiers;
        
        protected override void BeforeDefault() {
            base.BeforeDefault();
            if(_consumable == null) {
                _consumable = serializedObject.FindProperty("consumable");
                _cooldown = serializedObject.FindProperty("cooldown");
                _cooldownGroups = serializedObject.FindProperty("cooldownGroups");
                _actionRequirements = serializedObject.FindProperty("actionRequirements");
                _triggerAppliedModifiers = serializedObject.FindProperty("triggerAppliedModifiers");
            }
            AddToTab("Action",_consumable);
            AddToTab("Action",_cooldown);
            AddToTab("Action",_cooldownGroups);
            AddToTab("Action",_actionRequirements);
            AddToTab("Action",_triggerAppliedModifiers);
        }
        
        protected override Texture2D IconBadge => 
            _iconBadge ??= Resources.Load<Texture2D>("ItemBadges/ActionItemBadge64");
        
    }
    
}