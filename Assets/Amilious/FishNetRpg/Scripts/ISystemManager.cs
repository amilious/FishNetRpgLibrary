using System;

namespace Amilious.FishNetRpg {
    
    /// <summary>
    /// This interface is used to generalize rpg systems.
    /// </summary>
    public interface ISystemManager {
        
        /// <summary>
        /// This property contains the system identifier.
        /// </summary>
        public Systems System { get; }
        
        /// <summary>
        /// This property is used to get the <see cref="Type"/> of the manager.
        /// </summary>
        public Type SystemType { get; }
        
    }
    
}