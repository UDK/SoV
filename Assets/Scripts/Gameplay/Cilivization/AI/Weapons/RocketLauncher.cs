using Assets.Scripts.Gameplay.Cilivization.AI.Shells;
using Assets.Scripts.Gameplay.SpaceObject;
using Assets.Scripts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.AI.Weapons
{
    public class RocketLauncher: WeaponBase
    {
        protected override void CoreAttack(
            GameObject target,
            Guid allianceGuid)
        {
            var heading = target.transform.position - transform.position;
            var right = transform.right;
            var dot = Vector3.Dot(
                    right,
                    heading);
            var angle = Vector3.Angle(right, heading);
            if (angle < 20)
            {
                InitShell(target, allianceGuid);
            }
        }
    }
}
