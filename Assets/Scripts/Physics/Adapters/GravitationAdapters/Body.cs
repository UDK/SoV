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

        public Rigidbody2D Rigidbody2D { get; set; }

        public MonoBehaviour MonoBehaviour { get; set; }

        public int ChangeBecomeSatellite { get; set; }

        public bool BeginCheckIntoOrbit { get; set; } = false;

        public static implicit operator Rigidbody2D(Body satellite)
        {
            return satellite.Rigidbody2D;
        }

        public static implicit operator Body(GameObject collider2D)
        {
            return new Body
            {
                Rigidbody2D = collider2D.GetComponent<Rigidbody2D>(),
                MonoBehaviour = collider2D.GetComponent<MonoBehaviour>(),
                ChangeBecomeSatellite = 0

            };
        }
    }


}
