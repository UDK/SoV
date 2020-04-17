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
        public float AccelarationDeltaTime = 0.1f;

        /// <summary>
        /// points / s
        /// </summary>
        public float MaxVelocity = 2f;

        public Vector3 Velocity =>
            _rigidbody2D.velocity;

        public float Magnitude =>
            _magnitude;

        //private Vector3 _velocity = Vector3.zero;
        private float _magnitude = 0f;

        private bool _block = false;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Smoothly set velocity
        /// </summary>
        /// <param name="force">Normalized vector3 for pointing direction for moving</param>
        public void SmoothlySetVelocity(Vector3 force)
        {
            if (_block)
            {
                return;
            }

            if (force.x != 0 || force.y != 0 || force.z != 0)
            {
                _magnitude = _rigidbody2D.velocity.magnitude;
                
                /*Debug.Log(currentSpeed);*/
                if (_magnitude > MaxVelocity)
                {
                    _rigidbody2D.velocity = math.lerp(_rigidbody2D.velocity, Vector2.zero, 0.2f);
                }
                else
                {
                    _rigidbody2D.AddForce(force, ForceMode2D.Force);
                }
            }
        }

        /// <summary>
        /// Rough set speed
        /// </summary>
        /// <param name="iv">Normalized vector3 for pointing direction for moving</param>
        public void SetVelocity(Vector3 velocity)
        {
            _rigidbody2D.velocity = velocity;
            _magnitude = _rigidbody2D.velocity.magnitude;
        }

        private void FixedUpdate()
        {
        }
    }
}
