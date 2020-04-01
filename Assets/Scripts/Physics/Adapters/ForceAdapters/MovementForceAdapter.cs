using UnityEngine;
using UnityEditor;

namespace Assets.Scripts.Physics.Adapters.ForceAdapters
{
    public class MovementForceAdapter : IForceAdapter
    {
        public static IForceAdapter Instance =>
            new MovementForceAdapter();

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
            if (iv.x == 0 && iv.y == 0)
            {
                return Vector2.zero;
            }
            float minSpeed = influence;
            influence *= 2;

            var currentSpeed = who.velocity.magnitude;

            var oneSideX = iv.x != 0 && Mathf.Sign(iv.x) == Mathf.Sign(who.velocity.x);
            var oneSideY = iv.y != 0 && Mathf.Sign(iv.y) == Mathf.Sign(who.velocity.y);
            var absX = Mathf.Abs(who.velocity.x);
            var absY = Mathf.Abs(who.velocity.y);
            var sub = influence - currentSpeed;
            if (sub < minSpeed)
            {
                currentSpeed = minSpeed;
                if(oneSideX && oneSideY)
                {
                    if (absX > absY)
                    {
                        iv.x *= -1;
                    }
                    else
                    {
                        iv.y *= -1;
                    }
                }
                else
                {
                    if (oneSideX)
                    {
                        iv.x = 0;
                        iv.y = -Mathf.Sign(who.velocity.y);
                    }
                    if (oneSideY)
                    {
                        iv.y = 0;
                        iv.x = -Mathf.Sign(who.velocity.x);
                    }
                }
            }

            var acceleration = GetRequiredVelocity(influence, currentSpeed) / Time.fixedDeltaTime;
            return iv * who.mass * acceleration;
        }
    }
}
