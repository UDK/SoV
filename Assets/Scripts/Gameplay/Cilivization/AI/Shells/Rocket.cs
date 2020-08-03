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
    public class Rocket : ShellBase
    {
        public float LifeDistance = 10f;

        public bool Homing = false;

        private void FixedUpdate()
        {
            if (Target == null)
            {
                ShellCollection.Destroy(gameObject);
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
                    ShellCollection.Destroy(gameObject);
                }
            }
        }
    }
}
