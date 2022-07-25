using Amilious.Core;
using Amilious.FishNetRpg.Entities;

namespace Amilious.FishNetRpg.Requirements {
    public abstract class AbstractRequirement : AmiliousScriptableObject, IRequirement {
        public abstract bool MeetsRequirement(Entity entity);
    }
}