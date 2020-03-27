using UnityEngine;
using UnityEditor;

namespace Assets.Scripts.Physics
{
    public class MovementPhysics : IForcePhysics
    {
        public static IForcePhysics Instance =>
            new MovementPhysics();

        private static float GetRequiredVelocity(float aFinalSpeed, float currentSpeed)
        {
            float m = Mathf.Clamp01(Time.fixedDeltaTime);
            return Mathf.Abs(aFinalSpeed - currentSpeed) * m / (1 - m);
        }

        /// <summary>
        /// Gives Vector2 force
        /// </summary>
        /// <param name="who">Object that is pulled</param>
        /// <param name="iv">Direction</param>
        /// <param name="influence">Influence parameter that charges end force</param>
        /// <returns>Force for moving by sob</returns>
        public Vector2 PullForce(Rigidbody2D who, Vector2 iv, float influence)
        {
            var currentSpeed = who.velocity.magnitude;
            var acceleration = GetRequiredVelocity(influence, currentSpeed) / Time.fixedDeltaTime;
            return iv.normalized * who.mass * acceleration;
        }
    }
}
