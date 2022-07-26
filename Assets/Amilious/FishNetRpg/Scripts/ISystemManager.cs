using System;
using Amilious.FishNetRpg.Entities;
using Amilious.FishNetRpg.Modifiers;

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
        
        /// <summary>
        /// This property is used to get or return a cached reference to the <see cref="Entity"/> to whom this
        /// manager belongs to.
        /// </summary>
        public Entity Entity { get; }

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
        
    }
    
}