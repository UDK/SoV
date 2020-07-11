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
            SpaceShipContainer container) =>
            () =>
            {

                Ray enemyRay = new Ray(
                    self.transform.position,
                    self.transform.forward * container.SightDist);
                //Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward * sightDist, rayColor);
                //Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right).normalized * sightDist, rayColor);
                //Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right).normalized * sightDist, rayColor);

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
                    //_target = rayHit.collider.gameObject;
                    container.Targets = raycastHits.Select(x => x.collider?.gameObject).ToArray();
                    //if (rayHit.collider.gameObject.tag == "Player")
                    //{
                    //    agent.SetDestination(him.transform.position);
                    //    _target = rayHit.collider.gameObject;
                    //}
                }
            };
    }
}
