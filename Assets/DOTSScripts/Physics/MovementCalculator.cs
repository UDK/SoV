using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Physics;
using Unity.Core;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.DOTSScripts.Physics
{
    public static class MovementCalculator
    {
        private static float GetRequiredVelocity(
            float aFinalSpeed,
            float currentSpeed,
            float deltaTime)
        {
            float m = Mathf.Clamp01(deltaTime);
            return Mathf.Abs(aFinalSpeed - currentSpeed) * m / (1 - m);
        }

        public static float3 GetNeedVelocity(
            float3 velocity,
            float2 iv,
            float targetSpeed,
            float deltaTime)
        {
            //Debug.Log(physicsVelocity.Linear);
            if (iv.x != 0 || iv.y != 0)
            {
                var currentSpeed = math.sqrt(math.pow(velocity.x, 2) + math.pow(velocity.y, 2));
                float3 desiredSpeed = 0;
                if (currentSpeed < targetSpeed)
                {
                    desiredSpeed = math.float3(iv * targetSpeed, 0);
                }

                desiredSpeed = math.lerp(velocity, desiredSpeed, deltaTime);
                return desiredSpeed;
            }

            return velocity;
        }
    }
}
