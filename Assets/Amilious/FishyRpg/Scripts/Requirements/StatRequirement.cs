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
using Amilious.Core.Enums;
using Amilious.FishyRpg.Entities;
using Amilious.FishyRpg.Statistics;
using UnityEngine;

namespace Amilious.FishyRpg.Requirements {
    
    [CreateAssetMenu(fileName = "NewStatRequirement", menuName = FishNetRpg.REQUIREMENT_MENU_ROOT+"Stat")]
    public class StatRequirement : AbstractRequirement {

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, Tooltip("The stat for the requirement.")] 
        private Stat stat;
        [SerializeField, Tooltip("The comparison type for the requirement")] 
        private ComparisonMethod<int> comparisonMethod = new ComparisonMethod<int>();
        [SerializeField, Tooltip("The value to compare the stat with.")] 
        private int compareValue = 0;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the stat that the requirement is for.
        /// </summary>
        public Stat Stat => stat;

        /// <summary>
        /// This property contains the comparision type for the stat requirement.
        /// </summary>
        public ComparisonType ComparisonType => comparisonMethod.ComparisonType;

        /// <summary>
        /// This property contains the comparision value for the requirement.
        /// </summary>
        public int ComparisonValue => compareValue;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool MeetsRequirement(Entity entity) {
            return entity.TryGetManager(out StatManager manager) && 
                   comparisonMethod.Compare(manager[stat].Value, compareValue);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}