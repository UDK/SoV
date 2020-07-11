using Assets.Scripts.Gameplay.Cilivization.AI.States;
using Assets.Scripts.Gameplay.Cilivization.AI.Strategies;
using Assets.Scripts.Gameplay.Cilivization.AI.Weapons;
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
    public class ShipAIBase : MonoBehaviour, IGameplayObject
    {
        [SerializeField]
        public SpaceShipContainer Container;

        public Guid AllianceGuid 
        { 
            get
            {
                return Container.AllianceGuid;
            }
            set
            {
                Container.AllianceGuid = value;
            }
        }

        public GameObject[] Weapons;

        public StrategyType StrategyType;

        private UnitStrategyManager _strategyManager;

        public ShipAIBase()
        {
        }

        private void Start()
        {
            Container.Weapons = Weapons.Select(x =>
                x.GetComponent<MonoBehaviour>() as IWeapon)
                    .ToArray();
            AllianceGuid = Guid.NewGuid();
            Container.StateMachine = new AStateMachine<ShipStates>();
            _strategyManager = new UnitStrategyManager(gameObject, Container);

            _strategyManager.ApplyStrategy(
                StrategyType);

            Container.StateMachine.Push(ShipStates.Moving);
            Container.MovementBehaviour = GetComponent<MovementBehaviour>();
        }

        private void FixedUpdate()
        {
            // remove
            _strategyManager.ApplyStrategy(
                StrategyType);
            Container.StateMachine.Update();
        }
    }
}
