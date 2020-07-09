using Assets.Scripts.Gameplay.SpaceObject;
using Assets.Scripts.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.AI.Bullets
{
    public class UsualBullet : MonoBehaviour, IGameplayObject, IShell
    {
        public float LifeDistance { get; set; } = 4f;

        public GameObject Target { get; set; }

        public Guid AllianceGuid { get; set; }

        private MovementBehaviour _movementBehaviour;

        public UsualBullet()
        {
        }

        public void Initiate()
        {
            _movementBehaviour = GetComponent<MovementBehaviour>();
            var moveVector = Target.transform.position - transform.position;
            _movementBehaviour.SetVelocity(
                moveVector.normalized * _movementBehaviour.MaxVelocity);

            float angle = Mathf.Atan2(_movementBehaviour.Velocity.y,
                _movementBehaviour.Velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var go = collision.gameObject.GetComponent<IGameplayObject>();
            if (go == null
                || go.AllianceGuid == AllianceGuid)
            {
                return;
            }

            Destroy(gameObject);
        }
    }
}
