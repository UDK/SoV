using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Physics.Adapters.GravitationAdapter
{
    public class Body
    {
        public Coroutine CoroutineCheckIntoOrbit { get; set; }

        public MovementBehaviour MovementBehaviour { get; set; }

        public MonoBehaviour RawMonoBehaviour { get; set; }

        public bool BeginCheckIntoOrbit { get; set; } = false;

        public static implicit operator MovementBehaviour(Body satellite)
        {
            return satellite.MovementBehaviour;
        }

        public static implicit operator Body(GameObject collider2D)
        {
            return new Body
            {
                MovementBehaviour = collider2D.GetComponent<MovementBehaviour>(),
                RawMonoBehaviour = collider2D.GetComponent<MonoBehaviour>(),

            };
        }
    }


}
