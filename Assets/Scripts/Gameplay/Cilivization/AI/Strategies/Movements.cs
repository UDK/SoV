using Assets.Scripts.Gameplay.Cilivization.AI.States;
using Assets.Scripts.Helpers;
using Assets.Scripts.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.AI.Strategies
{
    public static class Movements
    {
        public static Action GoAfterTarget(
            GameObject self,
            SpaceShipContainer container) =>
            () =>
            {
                if(container.Target == null)
                {
                    return;
                }

                Rotations.RotateBySpeed(self, container);

                var distance = Vector2.Distance(
                    container.Target.transform.position,
                    self.transform.position);
                var heading =
                    container.Target.transform.position - self.transform.position;

                if (distance < container.AttackDistance)
                {
                    container.StateMachine.Push(
                        ShipStates.Attacking);
                }
                else
                {
                    container.MovementBehaviour.SmoothlySetVelocity(
                        heading.normalized * container.MovementBehaviour.MaxVelocity);
                }
            };
    }
}
