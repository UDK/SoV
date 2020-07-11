using Assets.Scripts.Gameplay.Cilivization.AI.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using sdType =
    System.Collections.Generic.Dictionary<
        Assets.Scripts.Gameplay.Cilivization.AI.Strategies.StrategyType,
        System.Action<
            UnityEngine.GameObject,
            Assets.Scripts.Gameplay.Cilivization.AI.SpaceShipContainer>>;

namespace Assets.Scripts.Gameplay.Cilivization.AI.Strategies
{
    public class UnitStrategyManager
    {
        private readonly sdType _strategies =
            new sdType
            {
                { StrategyType.AggressiveAttack, AggressiveAttackStrategy },
                { StrategyType.DistanceAttack, DistanceAttackStrategy },
                { StrategyType.BombingAttack, BombingAttackStrategy },
            };
        private GameObject _self;
        private SpaceShipContainer _container;

        public UnitStrategyManager(
            GameObject self,
            SpaceShipContainer container)
        {
            _self = self;
            _container = container;
        }

        public void ApplyStrategy(
            StrategyType strategy)
        {
            _strategies[strategy](_self, _container);
        }

        private static void AggressiveAttackStrategy(
            GameObject self,
            SpaceShipContainer container)
        {
            container.StateMachine.Set(
                    ShipStates.Moving,
                    Movements.GoAfterTarget(self, container))
                .Set(
                    ShipStates.SearchingOfTarget,
                    TargetSearchings.SearchTarget(self, container))
                .Set(
                    ShipStates.Attacking,
                    Attacks.CircleAround(self, container));
        }

        private static void DistanceAttackStrategy(
            GameObject self,
            SpaceShipContainer container)
        {
            container.StateMachine.Set(
                    ShipStates.Moving,
                    Movements.GoAfterTarget(self, container))
                .Set(
                    ShipStates.SearchingOfTarget,
                    TargetSearchings.SearchTarget(self, container))
                .Set(
                    ShipStates.Attacking,
                    Attacks.Distance(self, container));
        }

        private static void BombingAttackStrategy(
            GameObject self,
            SpaceShipContainer container)
        {
            container.StateMachine.Set(
                    ShipStates.Moving,
                    Movements.GoAfterTarget(self, container))
                .Set(
                    ShipStates.SearchingOfTarget,
                    TargetSearchings.SearchTarget(self, container))
                .Set(
                    ShipStates.Attacking,
                    Attacks.Back(self, container));
        }
    }
}
