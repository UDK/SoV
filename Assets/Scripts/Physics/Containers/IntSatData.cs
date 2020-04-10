using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Physics.Containers
{
    /// <summary>
    /// Intermediate sattelite data
    /// </summary>
    public class IntSatData
    {
        public MovementBehaviour MovementBehaviour { get; set; }

        //Нужно будет удалить
        public MonoBehaviour RawMonoBehaviour { get; set; }

        public Collider2D Collider2D { get; set; }

        public int HitsBeforeOrbit { get; set; }

        public int TempI { get; set; }

        public static implicit operator MovementBehaviour(IntSatData satellite)
        {
            return satellite.MovementBehaviour;
        }

        public static implicit operator Collider2D(IntSatData satellite)
        {
            return satellite.Collider2D;
        }

        public static implicit operator IntSatData(GameObject collider2D)
        {
            return new IntSatData
            {
                MovementBehaviour = collider2D.GetComponent<MovementBehaviour>(),
                RawMonoBehaviour = collider2D.GetComponent<MonoBehaviour>(),
                Collider2D = collider2D.GetComponent<Collider2D>(),

            };
        }
    }


}
