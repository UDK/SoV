using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Physics.Fabric.ForceFabrics
{
    public class GravityForce : IForce
    {
        /// <summary>
        /// Gives Vector2 force
        /// </summary>
        /// <param name="who">Object that pulls</param>
        /// <param name="iv">Second object that will be pulled</param>
        /// <param name="influence">Influence parameter that charges end force</param>
        /// <returns>Force for moving by sob</returns>
        public Vector2 PullForceFabricMethod(Movement who, Vector2 iv, float influence)
        {
            var distance = Vector2.Distance(who.transform.position, iv);
            if (distance == 0)
            {
                return new Vector2(0, 0);
            }
            // Find the Normal direction
            Vector2 normalDirection = ((Vector2)who.transform.position - iv).normalized;

            // calculate the force on the object from the planet
            Vector2 normalForce = normalDirection * influence;

            return normalForce;
        }
    }
}
