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

                Movements.MoveByCircle(self, container.Target, container);
                Rotations.RotateBySpeed(self, container);
            };

        public static Action Back(
            GameObject self,
            SpaceShipContainer container) =>
            () =>
            {
                Vector3 toTarget = (container.Target.transform.position - self.transform.position).normalized;
                var dot = Vector3.Dot(
                    self.transform.right,
                    toTarget);

                foreach (var weapon in container.Weapons)
                {
                    weapon.Attack(container.Target, container.AllianceGuid);
                }
                var distance = Vector2.Distance(
                    container.Target.transform.position,
                    self.transform.position);

                if (dot < -0.60 &&
                    distance > container.AttackDistance * 2)
                {
                    container.StateMachine.Push(
                        ShipStates.Moving);
                }
                else if (dot < 0)
                {
                    if (dot < -0.5)
                    {
                        Movements.MoveByCircle(self, container.Target, container);
                    }
                    container.MovementBehaviour.SmoothlySetVelocity(
                        container.MovementBehaviour.Velocity.normalized *
                        container.MovementBehaviour.MaxVelocity);
                }
                else
                {
                    Movements.MoveByCircle(self, container.Target, container);
                }

                Rotations.RotateBySpeed(self, container);
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
