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

namespace Assets.Scripts.Gameplay.Cilivization.AI.Strategies
{
    public interface IStrategyContainer : ISpaceShipAI
    {
        GameObject Homing { get; set; }

        GameObject Target { get; set; }

        List<WeaponBase> Weapons { get; }

        Movement MovementBehaviour { get; }

        // state machine
        AStateMachine<ShipStates> StateMachine { get; }
    }
}
