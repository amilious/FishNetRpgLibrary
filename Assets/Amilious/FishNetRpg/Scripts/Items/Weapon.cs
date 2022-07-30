using UnityEngine;
using Amilious.Core.Attributes;
using Amilious.FishNetRpg.Attributes;

namespace Amilious.FishNetRpg.Items {
    
    [ItemEditorBadge("Icons/ItemBadges/WeaponBadge")]
    [CreateAssetMenu(fileName = "NewWeapon", 
        menuName = FishNetRpg.ITEM_MENU_ROOT+"New Weapon", order = 21)]
    public class Weapon : EquipableItem {
        
        [SerializeField, AmiliousTab("Weapon")]
        private GameObject weaponPrefab;

    }
}