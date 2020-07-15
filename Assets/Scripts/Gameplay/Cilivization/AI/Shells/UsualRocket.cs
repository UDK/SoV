using Assets.Scripts.Gameplay.SpaceObject;
using Assets.Scripts.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.AI.Shells
{
    public class UsualRocket : MonoBehaviour, IGameplayObject, IShell
    {
        public float BaseDamage = 0.1f;

        public float LifeDistance { get; set; } = 10f;

        public GameObject Target { get; set; }

        public Guid AllianceGuid { get; set; }

        private MovementBehaviour _movementBehaviour;

        public bool Homing;

        private Vector3 _startPosition { get; set; }

        public UsualRocket()
        {
        }

        public void Initiate()
        {
            _movementBehaviour = GetComponent<MovementBehaviour>();
            var heading = Target.transform.position - transform.position;
            _movementBehaviour.SetVelocity(heading.normalized * _movementBehaviour.MaxVelocity);
            float angle = Mathf.Atan2(_movementBehaviour.Velocity.y,
                _movementBehaviour.Velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void FixedUpdate()
        {
            if (Target == null)
            {
                ShellCollection<UsualRocket>.Destroy(gameObject);
            }
            if (Homing)
            {
                var heading = Target.transform.position - transform.position;
                _movementBehaviour.SetVelocity(heading.normalized * _movementBehaviour.MaxVelocity);
                float angle = Mathf.Atan2(_movementBehaviour.Velocity.y,
                    _movementBehaviour.Velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            else
            {
                if (Vector3.Distance(transform.position, _startPosition) > LifeDistance)
                {
                    ShellCollection<UsualRocket>.Destroy(gameObject);
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var go = collision.gameObject.GetComponent<IGameplayObject>();
            if (go == null
                || go.AllianceGuid == AllianceGuid)
            {
                return;
            }

            go.MakeDamage(BaseDamage);
            ShellCollection<UsualRocket>.Destroy(gameObject);
        }

        public void MakeDamage(float damage)
        {
        }
    }
}
