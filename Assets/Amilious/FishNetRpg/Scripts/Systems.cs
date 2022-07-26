using System;
using Amilious.FishNetRpg.Experience;
using Amilious.FishNetRpg.Resource;
using Amilious.FishNetRpg.Statistics;

namespace Amilious.FishNetRpg {

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
    
    public static class SystemsExtension {

        private static readonly Type Stats = typeof(StatManager);
        private static readonly Type Level = typeof(LevelManager);
        private static readonly Type Resource = typeof(ResourceManager);
        
        public static Type GetSystemType(this Systems system) {
            return system switch {
                Systems.StatsSystem => Stats,
                Systems.LevelSystem => Level,
                Systems.ResourceSystem => Resource,
                _ => throw new ArgumentOutOfRangeException(nameof(system), system, null)
            };
        }
        
    }
    
}