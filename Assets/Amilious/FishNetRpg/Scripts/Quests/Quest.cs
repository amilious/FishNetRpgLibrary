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

using System.Linq;
using UnityEngine;
using Amilious.Core;
using System.Collections.Generic;
using Amilious.FishNetRpg.Entities;
using Amilious.FishNetRpg.Requirements;

namespace Amilious.FishNetRpg.Quests {
    
    [CreateAssetMenu(fileName = "NewQuest", menuName = FishNetRpg.QUEST_MENU_ROOT+"Quest")]
    public class Quest : AmiliousScriptableObject {

        [SerializeField] private string questName;
        [SerializeField] private List<AbstractRequirement> requirements = new ();

        public bool MeetsAllRequirements(Entity entity) {
            return requirements.All(requirement => requirement.MeetsRequirement(entity));
        }
        
    }
}