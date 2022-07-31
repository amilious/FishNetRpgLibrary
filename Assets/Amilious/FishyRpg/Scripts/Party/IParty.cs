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

using Amilious.FishyRpg.Entities;

namespace Amilious.FishyRpg.Party {
    
    /// <summary>
    /// This is an interface used for a party system.
    /// </summary>
    public interface IParty {

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the size of the party.
        /// </summary>
        public int Size { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to check if an entity belongs to your party.
        /// </summary>
        /// <param name="entity">The entity that you want to check.</param>
        /// <param name="includeFollowers">If true this method will also count the followers of party members.</param>
        /// <returns>True if the entity is in your party.</returns>
        public bool Contains(Entity entity, bool includeFollowers = true);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}