/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
   _____            .__ .__   .__                             ____       ___________                __                  
  /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /  _ \      \__    ___/____  ___  ____/  |_  ____   ______ 
 /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     >  _ </\      |    | _/ __ \ \  \/  /\   __\/  _ \ /  ___/ 
/    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \     /  <_\ \/      |    | \  ___/  >    <  |  | (  <_> )\___ \  
\____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    \_____\ \      |____|  \___  >/__/\_ \ |__|  \____//____  > 
        \/       \/                                  \/            \/                  \/       \/                  \/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious,com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious, Textos since 2022                      //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using Amilious.Core;
using Amilious.Core.Attributes;
using Amilious.Core.Extensions;
using UnityEngine;

namespace Amilious.FishyRpg.Items {
    
    [CreateAssetMenu(fileName = "NewItemRarity", 
        menuName = FishyRpg.ITEM_MENU_ROOT+"New Rarity", order = 0)]
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