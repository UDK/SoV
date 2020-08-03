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

                var distance = Vector2.Distance(
                    container.Target.transform.position,
                    self.transform.position);
                var heading =
                    container.Target.transform.position - self.transform.position;

                if (distance < container.MinAttackDistance)
                {
                    container.StateMachine.Push(
                        ShipStates.Attacking);
                }
                else
                {
                    container.MovementBehaviour.SmoothlySetVelocity(
                        heading.normalized * container.MovementBehaviour.MaxVelocity);
                }

                Rotations.RotateBySpeed(self, container);
            };

        public static void MoveByCircle(GameObject self, GameObject target, SpaceShipContainer container)
        {
            // get current magnitude
            var maxVelocity = container.MovementBehaviour.MaxVelocity;

            // get vector center <- obj
            var gravityVector = target.transform.position - self.transform.position;

            // check whether left or right of target
            var left = Vector2.SignedAngle(container.MovementBehaviour.Velocity, gravityVector) > 0;

            // get new vector which is 90° on gravityDirection 
            // and world Z (since 2D game)
            // normalize so it has magnitude = 1
            var newDirection = Vector3.Cross(gravityVector, Vector3.forward).normalized;

            // invert the newDirection in case user is touching right of movement direction
            if (!left) newDirection *= -1;

            // set new direction but keep speed(previously stored magnitude)
            container.MovementBehaviour.SmoothlySetVelocity(newDirection * maxVelocity);
        }

        public static void HeadToTarget(GameObject self, GameObject target, SpaceShipContainer container)
        {
            var heading =
                target.transform.position - self.transform.position;
            container.MovementBehaviour.SmoothlySetVelocity(
                heading.normalized * container.MovementBehaviour.MaxVelocity);
        }

        public static void CircleAroundTarget(GameObject self, GameObject target, SpaceShipContainer container)
        {
            var distance = Vector2.Distance(
                container.Homing.transform.position,
                self.transform.position);
            if (distance > container.SightDist)
            {
                Movements.HeadToTarget(self, container.Homing, container);
                return;
            }
            Movements.MoveByCircle(self, container.Homing, container);
        }
    }
}
