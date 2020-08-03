using Assets.Scripts.Gameplay.Cilivization.AI.States;
using Assets.Scripts.Gameplay.Cilivization.AI.WeaponSlots;
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
    [Serializable]
    public class SpaceShipContainer
    {
        public float HP = 177;

        // used for visual detection of enemies
        public float VisionAngle = 45;

        public int RayCount = 3;

        public float SightDist = 10f;

        [NonSerialized]
        public Guid AllianceGuid;

        // determines minimal distance for attack
        [NonSerialized]
        public float MinAttackDistance = 3f;

        public GameObject Homing;

        public GameObject Target;

        [NonSerialized]
        public WeaponBase[] Weapons;

        [NonSerialized]
        public Movement MovementBehaviour;

        // state machine
        [NonSerialized]
        public AStateMachine<ShipStates> StateMachine;
    }
}
