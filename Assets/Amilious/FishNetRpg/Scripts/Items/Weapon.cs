using Amilious.Core.Attributes;
using Amilious.FishNetRpg.Attributes;
using UnityEngine;

namespace Amilious.FishNetRpg.Items {
    
    [ItemEditorBadge("ItemBadges/WeaponBadge64")]
    [CreateAssetMenu(fileName = "NewWeapon", 
        menuName = FishNetRpg.ITEM_MENU_ROOT+"New Weapon", order = 21)]
    public class Weapon : EquipableItem {
        
        [SerializeField, AmiliousTab("Weapon")]
        private GameObject weaponPrefab;

    }
}