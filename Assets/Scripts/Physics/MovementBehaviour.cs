using Assets.Scripts.Helpers;
using System;
using System.Collections;
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
        /// points / s
        /// </summary>
        public float Magnitude { get; set; }

        /// <summary>
        /// points / s
        /// </summary>
        public float MaxVelocity = 2f;

        public Vector3 Velocity =>
            _velocity;

        private Vector3 _velocity = Vector3.zero;

        public bool Block = false;

        /// <summary>
        /// Smoothly set velocity
        /// </summary>
        /// <param name="velocity">Normalized vector3 for pointing direction for moving</param>
        public void SmoothlySetVelocity(Vector3 velocity)
        {
            if (Block)
            {
                return;
            }
            //Debug.Log(physicsVelocity.Linear);
            /*var currentSpeed2 = math.sqrt(math.pow(velocity.x, 2) + math.pow(velocity.y, 2) + math.pow(velocity.z, 2));
            if(currentSpeed2 > MaxVelocity)
            {
                Debug.LogError("Speed over");
                return;
            }*/
            if (velocity.x != 0 || velocity.y != 0 || velocity.z != 0)
            {
                Magnitude = math.sqrt(math.pow(_velocity.x, 2) + math.pow(_velocity.y, 2) + math.pow(_velocity.z, 2));

                /*Debug.Log(currentSpeed);*/
                if (Magnitude > MaxVelocity)
                {
                    velocity = Vector3.zero;
                }

                _velocity = math.lerp(_velocity, velocity, AccelarationDeltaTime);
            }
        }

        /// <summary>
        /// Rough set speed
        /// </summary>
        /// <param name="iv">Normalized vector3 for pointing direction for moving</param>
        public void SetVelocity(Vector3 velocity)
        {
            if (Block)
            {
                return;
            }
            Magnitude = Vector3.Magnitude(velocity);// math.sqrt(math.pow(velocity.x, 2) + math.pow(velocity.y, 2) + math.pow(velocity.z, 2));
            _velocity = velocity;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!LayerEnums.IsBody(collision.gameObject.layer))
            {
                return;
            }

            MovementBehaviour enemy = collision.gameObject.GetComponent<MovementBehaviour>();
            if (Magnitude > enemy.Magnitude)
            {
                var speed1 = Velocity.normalized * 0.1f;
                Vector2 normal = collision.contacts[0].normal;
                var reflect = Vector2.Reflect(speed1, normal);
                var reflectEnemy = reflect * -0.99f;
                SetVelocity(reflect);
                enemy.SetVelocity(reflectEnemy);
            }
        }

        private void FixedUpdate()
        {
            this.transform.Translate(_velocity);
        }
    }
}
