using UnityEngine;
using UnityEngine.AI;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extensions to the NavMeshPath class.
    /// </summary>
    public static class NavMeshPathExtensions {
        
        /// <summary>
        /// This method is used to get the distance of the current path.
        /// </summary>
        /// <param name="path">The path you want to get the distance of.</param>
        /// <returns>The distance of the path.</returns>
        public static float CalculateDistance(this NavMeshPath path) {
            var total = 0f;
            if(path.corners.Length < 2) return 0;
            for(var i = 0; i < path.corners.Length-1; i++) {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            return total;
        }
        
    }
}