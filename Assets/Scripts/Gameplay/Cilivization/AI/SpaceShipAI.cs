using Assets.Scripts.Gameplay.Cilivization.AI.States;
using Assets.Scripts.Gameplay.Cilivization.AI.Strategies;
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
    public class SpaceShipAI : MonoBehaviour, IStrategyContainer
    {
        [field: SerializeField]
        public float HP { get; set; } = 100;

        // used for visual detection of enemies
        [field: SerializeField]
        public float VisionAngle { get; set; } = 45;

        [field: SerializeField]
        public int RayCount { get; set; } = 3;

        [field: SerializeField]
        public float SightDist { get; set; } = 10f;

        public Guid AllianceGuid { get; set; }

        // determines minimal distance for attack
        public float MinAttackDistance { get; set; } = 3f;

        [field: SerializeField]
        public GameObject Homing { get; set; }

        [field: SerializeField]
        public GameObject Target { get; set; }

        public List<WeaponBase> Weapons { get; set; }

        public Movement MovementBehaviour { get; set; }

        // state machine
        public AStateMachine<ShipStates> StateMachine { get; set; }

        public StrategyType StrategyType;

        private UnitStrategyManager _strategyManager;

        public SpaceShipAI()
        {
        }

        private void Start()
        {
            Weapons = new List<WeaponBase>();
            foreach (Transform child in this.transform)
            {
                child.gameObject.CheckComponent<WeaponBase>(
                    wb =>
                    {
                        Weapons.Add(wb);
                    });
            }
            StateMachine = new AStateMachine<ShipStates>();
            _strategyManager = new UnitStrategyManager(gameObject, this);

            _strategyManager.ApplyStrategy(
                StrategyType);

            StateMachine.Push(ShipStates.SearchingOfTarget);
            MovementBehaviour = GetComponent<Movement>();
        }

        private void FixedUpdate()
        {
            // remove
            /*_strategyManager.ApplyStrategy(
                StrategyType);*/
            StateMachine.Update();
        }

        public void MakeDamage(float damage)
        {
        }
    }
}
