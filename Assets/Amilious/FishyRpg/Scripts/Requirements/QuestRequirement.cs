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
using System.Linq;
using Amilious.FishyRpg.Quests;
using Amilious.FishyRpg.Entities;
using System.Collections.Generic;

namespace Amilious.FishyRpg.Requirements {
    
    /// <summary>
    /// This class is used as a quest requirement.
    /// </summary>
    [CreateAssetMenu(fileName = "NewQuestRequirement", menuName = FishyRpg.REQUIREMENT_MENU_ROOT+"Quest")]
    public class QuestRequirement : AbstractRequirement{

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, Tooltip("The quests related to this requirement.")] 
        private List<Quest> quests = new List<Quest>();
        [SerializeField, Tooltip("The acceptable statuses of the quests.")] 
        private QuestStatus questStatus = QuestStatus.Completed;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool MeetsRequirement(Entity entity) {
            if(entity == null || entity is not Player player) return false;
            return quests.All(quest => questStatus.HasFlag(player.QuestManager[quest]));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}