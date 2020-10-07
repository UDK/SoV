using Assets.Scripts.Gameplay.Cilivization.AI.States;
using Assets.Scripts.Gameplay.Cilivization.AI.WeaponSlots;
using Assets.Scripts.Gameplay.SpaceObject;
using Assets.Scripts.Helpers;
using Assets.Scripts.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.AI
{
    public interface ISpaceShipAI : IGameplayObject
    {
        float HP { get; set; }

        // used for visual detection of enemies
        float VisionAngle { get; set; }

        int RayCount { get; set; }

        float SightDist { get; set; }

        // determines minimal distance for attack
        float MinAttackDistance { get; set; }
    }
}
