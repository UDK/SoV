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
            Assets.Scripts.Gameplay.Cilivization.AI.Strategies.IStrategyContainer>>;

namespace Assets.Scripts.Gameplay.Cilivization.AI.Strategies
{
    public class UnitStrategyManager
    {
        private readonly sdType _strategies =
            new sdType
            {
                { StrategyType.Fighter, AggressiveAttackStrategy },
                { StrategyType.Artillery, DistanceAttackStrategy },
                { StrategyType.Bomber, BombingAttackStrategy },
            };
        private GameObject _self;
        private IStrategyContainer _container;

        public UnitStrategyManager(
            GameObject self,
            IStrategyContainer container)
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
            IStrategyContainer container)
        {
            container.StateMachine
                .Set(
                    ShipStates.Moving,
                    Middleware(
                        TargetCheckMiddleware(container),
                        Movements.GoAfterTarget(self, container)))
                .Set(
                    ShipStates.SearchingOfTarget,
                    TargetSearchings.SearchTarget(self, container))
                .Set(
                    ShipStates.Attacking,
                    Middleware(
                        TargetCheckMiddleware(container),
                        Attacks.CircleAround(self, container)));
        }

        private static void DistanceAttackStrategy(
            GameObject self,
            IStrategyContainer container)
        {
            container.StateMachine
                .Set(
                    ShipStates.Moving,
                    Middleware(
                        TargetCheckMiddleware(container),
                        Movements.GoAfterTarget(self, container)))
                .Set(
                    ShipStates.SearchingOfTarget,
                    TargetSearchings.SearchTarget(self, container))
                .Set(
                    ShipStates.Attacking,
                    Middleware(
                        TargetCheckMiddleware(container),
                        Attacks.Distance(self, container)));
        }

        private static void BombingAttackStrategy(
            GameObject self,
            IStrategyContainer container)
        {
            container.StateMachine
                .Set(
                    ShipStates.Moving,
                    Middleware(
                        TargetCheckMiddleware(container),
                        Movements.GoAfterTarget(self, container)))
                .Set(
                    ShipStates.SearchingOfTarget,
                    TargetSearchings.SearchTarget(self, container))
                .Set(
                    ShipStates.Attacking,
                    Middleware(
                        TargetCheckMiddleware(container),
                        Attacks.Back(self, container)));
        }


        private static Func<bool> TargetCheckMiddleware(IStrategyContainer container) =>
            () =>
            {
                if (container.Target == null)
                {
                    container.StateMachine.Push(ShipStates.SearchingOfTarget);
                    return false;
                }

                return true;
            };

        /// <summary>
        /// Middleware for actions
        /// </summary>
        /// <param name="preAction">returns true if postAction can be continued</param>
        /// <param name="postAction">some postaction</param>
        /// <returns></returns>
        private static Action Middleware(Func<bool> preAction, Action postAction) =>
            () =>
            {
                if (preAction())
                {
                    postAction();
                }
            };
    }
}
