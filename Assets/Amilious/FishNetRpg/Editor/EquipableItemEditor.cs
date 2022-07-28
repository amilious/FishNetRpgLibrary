using Amilious.FishNetRpg.Items;
using UnityEditor;
using UnityEngine;

namespace Amilious.FishNetRpg.Editor {
    
    [CustomEditor(typeof(EquipableItem))]
    public class EquipableItemEditor : ItemEditor {

        private SerializedProperty _allowedEquipmentSlots;
        private SerializedProperty _equipRequirements;
        private SerializedProperty _equipAppliedModifiers;
        
        protected override void BeforeDefault() {
            base.BeforeDefault();
            if(_allowedEquipmentSlots == null) {
                _allowedEquipmentSlots = serializedObject.FindProperty("allowedEquipmentSlots");
                _equipRequirements = serializedObject.FindProperty("equipRequirements");
                _equipAppliedModifiers = serializedObject.FindProperty("equipAppliedModifiers");
            }
            AddToTab("Equipable",_allowedEquipmentSlots);
            AddToTab("Equipable",_equipRequirements);
            AddToTab("Equipable",_equipAppliedModifiers);
        }
        
        protected override Texture2D IconBadge => 
            _iconBadge ??= Resources.Load<Texture2D>("ItemBadges/EquipmentBadge64");
        
    }
    
}