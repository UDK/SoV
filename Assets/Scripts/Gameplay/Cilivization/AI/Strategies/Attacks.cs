using Assets.Scripts.Gameplay.Cilivization.AI.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.AI.Strategies
{
    public static class Attacks
    {
        public static Action CircleAround(
            GameObject self,
            SpaceShipContainer container) =>
            () =>
            {
                Rotations.RotateBySpeed(self, container);
                foreach (var weapon in container.Weapons)
                {
                    weapon.Attack(container.Target, container.AllianceGuid);
                }

                var distance = Vector2.Distance(
                    container.Target.transform.position,
                    self.transform.position);
                if (distance > container.AttackDistance)
                {
                    container.StateMachine.Push(
                        ShipStates.Moving);
                    return;
                }

                // get current magnitude
                var magnitude = container.MovementBehaviour.Magnitude;

                // get vector center <- obj
                var gravityVector = container.Target.transform.position - self.transform.position;

                // check whether left or right of target
                var left = Vector2.SignedAngle(container.MovementBehaviour.Velocity, gravityVector) > 0;

                // get new vector which is 90° on gravityDirection 
                // and world Z (since 2D game)
                // normalize so it has magnitude = 1
                var newDirection = Vector3.Cross(gravityVector, Vector3.forward).normalized;

                // invert the newDirection in case user is touching right of movement direction
                if (!left) newDirection *= -1;

                // set new direction but keep speed(previously stored magnitude)
                container.MovementBehaviour.SmoothlySetVelocity(newDirection * magnitude);
            };

        public static Action Back(
            GameObject self,
            SpaceShipContainer container) =>
            () =>
            {
                Rotations.RotateBySpeed(self, container);
                foreach (var weapon in container.Weapons)
                {
                    weapon.Attack(container.Target, container.AllianceGuid);
                }

                var distance = Vector2.Distance(
                    container.Target.transform.position,
                    self.transform.position);
                if (distance > container.AttackDistance)
                {
                    container.StateMachine.Push(
                        ShipStates.Moving);
                    return;
                }

                // get current magnitude
                var magnitude = container.MovementBehaviour.Magnitude;

                // get vector center <- obj
                var gravityVector = container.Target.transform.position - self.transform.position;

                // check whether left or right of target
                var left = Vector2.SignedAngle(container.MovementBehaviour.Velocity, gravityVector) > 0;

                // get new vector which is 90° on gravityDirection 
                // and world Z (since 2D game)
                // normalize so it has magnitude = 1
                var newDirection = Vector3.Cross(gravityVector, Vector3.forward).normalized;

                // invert the newDirection in case user is touching right of movement direction
                if (!left) newDirection *= -1;

                // set new direction but keep speed(previously stored magnitude)
                container.MovementBehaviour.SmoothlySetVelocity(newDirection * magnitude);
            };

        public static Action Distance(
            GameObject self,
            SpaceShipContainer container) =>
            () =>
            {
                Rotations.RotateToTarget(self, container);

                foreach (var weapon in container.Weapons)
                {
                    weapon.Attack(container.Target, container.AllianceGuid);
                }

                var distance = Vector2.Distance(
                    container.Target.transform.position,
                    self.transform.position);
                if (distance > container.AttackDistance)
                {
                    container.StateMachine.Push(
                        ShipStates.Moving);
                    return;
                }

                container.MovementBehaviour.SetVelocity(Vector3.zero);
            };
    }
}
