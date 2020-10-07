using Assets.Scripts.Gameplay.Cilivization.AI.Shells;
using Assets.Scripts.Gameplay.SpaceObject;
using Assets.Scripts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.AI.WeaponSlots
{
    public class TurretSlot : WeaponBase
    {
        protected override void CoreAttack(
            GameObject target,
            Guid allianceGuid)
        {
            InitShell(target, allianceGuid);
        }
    }
}
