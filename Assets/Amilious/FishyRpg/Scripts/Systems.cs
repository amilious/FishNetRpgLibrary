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
using Amilious.FishyRpg.Resource;
using Amilious.FishyRpg.Experience;
using Amilious.FishyRpg.Statistics;

namespace Amilious.FishyRpg {

    /// <summary>
    /// This enum is used to dictate what system a modifier is for.
    /// </summary>
    public enum Systems {
        
        /// <summary>
        /// This value represents the stats system.
        /// </summary>
        StatsSystem, 
        
        /// <summary>
        /// This value represents the level and xp system.
        /// </summary>
        LevelSystem, 
        
        /// <summary>
        /// This value represents the resources system.
        /// </summary>
        ResourceSystem
    }
    
    /// <summary>
    /// This class is used to add methods to the <see cref="Systems"/> enum.
    /// </summary>
    public static class SystemsExtension {

        #region Private Variables //////////////////////////////////////////////////////////////////////////////////////
        
        private static readonly Type Stats = typeof(StatManager);
        private static readonly Type Level = typeof(LevelManager);
        private static readonly Type Resource = typeof(ResourceManager);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the system type from <see cref="Systems"/>.
        /// </summary>
        /// <param name="system">The system that you want to get the type for.</param>
        /// <returns>The manager type for the system.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if there is an unhandled system.</exception>
        public static Type GetSystemType(this Systems system) {
            return system switch {
                Systems.StatsSystem => Stats,
                Systems.LevelSystem => Level,
                Systems.ResourceSystem => Resource,
                _ => throw new ArgumentOutOfRangeException(nameof(system), system, null)
            };
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}