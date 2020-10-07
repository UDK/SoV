using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.AI.Strategies
{
    public static class Rotations
    {
        public static void RotateBySpeed(
            GameObject self,
            IStrategyContainer container)
        {
            float angle = Mathf.Atan2(container.MovementBehaviour.Velocity.y,
                container.MovementBehaviour.Velocity.x) * Mathf.Rad2Deg;
            self.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public static void RotateToTarget(
            GameObject self,
            IStrategyContainer container)
        {
            var heading = container.Target.transform.position - self.transform.position;
            float angle = Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg;
            self.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
