using Amilious.Core;
using Amilious.Core.Attributes;
using Amilious.Core.Extensions;
using UnityEngine;

namespace Amilious.FishNetRpg.Items {
    
    [CreateAssetMenu(fileName = "NewItemRarity", 
        menuName = FishNetRpg.ITEM_MENU_ROOT+"New Rarity", order = 0)]
    public class ItemRarity : AmiliousScriptableObject {

        [SerializeField] private string rarityName;
        [SerializeField] private int rarityValue = 0;
        [SerializeField, AmiliousColor] private Color rarityColor = UnityEngine.Color.white;

        public string RarityName => rarityName;

        public int RarityValue => rarityValue;

        public Color Color => rarityColor;
        
        protected override void BeforeSerialize() {
            if(string.IsNullOrWhiteSpace(rarityName)) rarityName = name.SplitCamelCase();
        }

    }
}