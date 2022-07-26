using Amilious.FishNetRpg.Entities;

namespace Amilious.FishNetRpg.Requirements {
    
    public interface IRequirementProvider {

        public bool MeetsAllRequirements(Entity entity);

    }
    
}