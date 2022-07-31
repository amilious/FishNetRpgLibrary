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

using UnityEngine;
using Amilious.Core;
using Amilious.FishyRpg.Entities;

namespace Amilious.FishyRpg.Requirements {
    
    /// <summary>
    /// This requirement is used to make sure the player has a correct party size.
    /// </summary>
    [CreateAssetMenu(fileName = "NewPartySizeRequirement", menuName = FishNetRpg.REQUIREMENT_MENU_ROOT+"Party Size")]
    public class PartySizeRequirement : AbstractRequirement {

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, Tooltip("The comparison type for the requirement")]  
        private ComparisonMethod<int> comparisonMethod = new ComparisonMethod<int>();
        [SerializeField, Min(1), Tooltip("The size of the party to use for the requirement")]
        private int size = 1;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool MeetsRequirement(Entity entity) {
            if(entity == null || entity is not Player player) return false;
            if(size == 1 && player.Party == null) return true;
            return comparisonMethod.Compare(size, player.Party.Size);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}