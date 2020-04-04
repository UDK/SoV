using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    public class MovementBehaviour : MonoBehaviour
    {
        public float AccelarationDeltaTime = 0.001f;

        /// <summary>
        /// points /s
        /// </summary>
        public float TargetVelocity = 1f;

        public Vector3 Velocity =>
            _velocity;

        private Vector3 _velocity = Vector3.zero;

        /// <summary>
        /// Set direction for object
        /// </summary>
        /// <param name="iv">Normalized vector3 for pointing direction for moving</param>
        public void MoveInDirection(Vector3 iv, float? targetVelocity = null)
        {
            //Debug.Log(physicsVelocity.Linear);
            if (iv.x != 0 || iv.y != 0 || iv.z != 0)
            {
                var currentSpeed = math.sqrt(math.pow(_velocity.x, 2) + math.pow(_velocity.y, 2)+ math.pow(_velocity.z, 2));
                Debug.Log(currentSpeed);
                if (currentSpeed > TargetVelocity)
                {
                    iv = Vector3.zero;
                }
                else
                {
                    iv *= targetVelocity ?? TargetVelocity;
                }

                _velocity = math.lerp(_velocity, iv, AccelarationDeltaTime);
            }
        }

        /// <summary>
        /// Set speed
        /// </summary>
        /// <param name="iv">Normalized vector3 for pointing direction for moving</param>
        public void SetVelocity(Vector3 velocity)
        {
            _velocity = velocity;
        }

        private void FixedUpdate()
        {
            this.transform.Translate(_velocity);
        }
    }
}
