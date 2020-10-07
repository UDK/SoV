using Assets.Scripts.Gameplay.SpaceObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.AI.Strategies
{
    public static class TargetSearchings
    {
        public static Action SearchTarget(
            GameObject self,
            IStrategyContainer container) =>
            () =>
            {
                Ray enemyRay = new Ray(
                    self.transform.position,
                    self.transform.forward * container.SightDist);

                float halfAngle = container.VisionAngle / container.RayCount * (container.RayCount * 0.5f);
                for (int i = 0; i < container.RayCount; i++)
                {
                    float angle = i * (container.VisionAngle / (container.RayCount - 1));
                    Vector3 direction = Quaternion.AngleAxis(
                        self.transform.localRotation.eulerAngles.z + angle - halfAngle,
                        self.transform.forward) * Vector3.right;

                    Debug.DrawRay(self.transform.position, direction * container.SightDist, Color.red, 0.1f);
                    var filter = new ContactFilter2D();
                    filter.NoFilter();
                    RaycastHit2D[] raycastHits = new RaycastHit2D[10];
                    var rayHit = Physics2D.Raycast(
                        self.transform.position,
                        direction,
                        filter,
                        raycastHits,
                        container.SightDist);

                    var targets = raycastHits.Select(x => x.collider?.gameObject).ToArray();
                    foreach(var t in targets)
                    {
                        var go = t?.GetComponent<IGameplayObject>();
                        if(go != null &&
                            (t.TryGetComponent(out SpaceBody _) ||
                            t.TryGetComponent(out SpaceShipAI _)) &&
                            go.AllianceGuid != container.AllianceGuid)
                        {
                            container.Target = t;
                            container.StateMachine.Push(
                                States.ShipStates.Attacking);
                            return;
                        }
                    }
                }

                Movements.CircleAroundTarget(self, container.Homing, container);
                Rotations.RotateBySpeed(self, container);
            };
    }
}
