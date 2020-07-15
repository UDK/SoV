using Assets.Scripts.Gameplay.Cilivization.AI.States;
using Assets.Scripts.Gameplay.Cilivization.AI.Weapons;
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
        // used for visual detection of enemies
        public float VisionAngle = 45;
        public int RayCount = 3;
        public float SightDist = 10f;

        [NonSerialized]
        public Guid AllianceGuid;

        // determines distance for attack
        public float AttackDistance = 3f;

        public GameObject Homing;

        public GameObject Target;

        [NonSerialized]
        public IWeapon[] Weapons;

        [NonSerialized]
        public MovementBehaviour MovementBehaviour;

        // state machine
        public AStateMachine<ShipStates> StateMachine;

        // for debug
        public GameObject[] Targets;
    }
}
