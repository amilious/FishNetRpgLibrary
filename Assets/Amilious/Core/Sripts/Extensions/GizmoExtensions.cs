using UnityEngine;

namespace Amilious.Core.Extensions {
    
    /// <summary>
    /// This class is used to add extension methods to gizmos.
    /// </summary>
    public static class GizmoExtensions {
        
        /// <summary>
        /// Draws a wire arc.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="dir">The direction from which the anglesRange is taken into account</param>
        /// <param name="anglesRange">The angle range, in degrees.</param>
        /// <param name="radius"></param>
        /// <param name="maxSteps">How many steps to use to draw the arc.</param>
        public static void DrawWireArc(Vector3 position, Vector3 dir, float anglesRange, float radius, float maxSteps = 20) {
            var srcAngles = MathV.AnglesFromDirection(position, dir);
            var initialPos = position;
            var posA = initialPos;
            var stepAngles = anglesRange / maxSteps;
            var angle = srcAngles - anglesRange / 2;
            for (var i = 0; i <= maxSteps; i++) {
                var rad = Mathf.Deg2Rad * angle;
                var posB = initialPos;
                posB += new Vector3(radius * Mathf.Cos(rad), 0, radius * Mathf.Sin(rad));
                Gizmos.DrawLine(posA, posB);
                angle += stepAngles;
                posA = posB;
            }
            Gizmos.DrawLine(posA, initialPos);
        }
        
    }
}