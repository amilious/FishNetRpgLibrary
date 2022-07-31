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

using System;
using Amilious.FishyRpg.Entities;
using Amilious.FishyRpg.Modifiers;

namespace Amilious.FishyRpg {
    
    /// <summary>
    /// This interface is used to generalize rpg systems.
    /// </summary>
    public interface ISystemManager {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the system identifier.
        /// </summary>
        public Systems System { get; }
        
        /// <summary>
        /// This property is used to get the <see cref="Type"/> of the manager.
        /// </summary>
        public Type SystemType { get; }
        
        /// <summary>
        /// This property is used to get or return a cached reference to the <see cref="Entity"/> to whom this
        /// manager belongs to.
        /// </summary>
        public Entity Entity { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to add a modifier.  This method should only be
        /// called by the server.
        /// </summary>
        /// <param name="source">The source that is applying the modifier.</param>
        /// <param name="modifier">The modifier that you want to apply.</param>
        /// <returns>True if able to apply the modifier, otherwise false.</returns>
        public bool ApplyModifier(UnityEngine.Object source, IModifier modifier);

        /// <summary>
        /// This method is used to add a modifier.  This method should only be
        /// called by the server.
        /// </summary>
        /// <param name="sourceId">The source that is applying the modifier.</param>
        /// <param name="modifier">The modifier that you want to apply.</param>
        /// <returns>True if able to apply the modifier, otherwise false.</returns>
        public bool ApplyModifier(int sourceId, IModifier modifier);

        /// <summary>
        /// This method is used to remove a modifier.  This method should only be
        /// called by the server.
        /// </summary>
        /// <param name="source">The source that applied the modifier.</param>
        /// <param name="modifier">The modifier that you want to remove.</param>
        /// <returns>True if the modifier exists and was removed, otherwise false.</returns>
        public bool RemoveModifier(UnityEngine.Object source, IModifier modifier);

        /// <summary>
        /// This method is used to remove a modifier.  This method should only be
        /// called by the server.
        /// </summary>
        /// <param name="sourceId">The source that applied the modifier.</param>
        /// <param name="modifier">The modifier that you want to remove.</param>
        /// <returns>True if the modifier exists and was removed, otherwise false.</returns>
        public bool RemoveModifier(int sourceId, IModifier modifier);

        /// <summary>
        /// This method is used to remove all the modifiers that were assigned by the given source.
        /// </summary>
        /// <param name="source">The source.</param>
        public void RemoveModifiersFromSource(UnityEngine.Object source);

        /// <summary>
        /// This method is used to remove all modifiers of the given type.
        /// </summary>
        /// <param name="sourceId">The source of the modifiers that you want to remove.</param>
        public void RemoveModifiersFromSource(int sourceId);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}