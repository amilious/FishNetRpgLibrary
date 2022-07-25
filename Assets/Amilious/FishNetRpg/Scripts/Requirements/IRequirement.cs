using Amilious.FishNetRpg.Entities;

namespace Amilious.FishNetRpg.Requirements {
    
    public interface IRequirement {

        /// <summary>
        /// This method is used to check if the requirement is met.
        /// </summary>
        /// <param name="entity">The entity that you want to check the requirement against.</param>
        /// <returns>True if the given entity meets the requirement.</returns>
        public bool MeetsRequirement(Entity entity);

    }
    
}