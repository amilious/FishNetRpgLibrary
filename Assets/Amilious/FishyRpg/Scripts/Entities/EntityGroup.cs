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
using System.Collections.Generic;

namespace Amilious.FishyRpg.Entities {
    
    /// <summary>
    /// This class is used to create a group of entity types.
    /// </summary>
    [CreateAssetMenu(fileName = "NewEntityGroup", 
        menuName = FishyRpg.ENTITY_MENU_ROOT+"New Entity Group", order = FishyRpg.ENTITY_START+1)]
    public class EntityGroup : AmiliousScriptableObject {

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, Tooltip("The entity types that belong to this group.")] 
        private List<EntityType> entityTypes = new List<EntityType>();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to check if an <see cref="EntityType"/> belongs to this entity group.
        /// </summary>
        /// <param name="entityType">The entity type that you want to check the group for.</param>
        /// <returns>True if the given entity type belongs to the group.</returns>
        public bool ContainsEntityType(EntityType entityType) => entityTypes.Contains(entityType);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}