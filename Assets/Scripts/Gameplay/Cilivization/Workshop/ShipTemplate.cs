using Assets.Scripts.Gameplay.Cilivization.AI;
using Assets.Scripts.Gameplay.Cilivization.AI.Modilfiers;
using Assets.Scripts.Gameplay.Cilivization.AI.Shells;
using Assets.Scripts.Gameplay.Cilivization.Descriptions;
using Assets.Scripts.Helpers;
using Assets.Scripts.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.Workshop
{
    public class ShipTemplate
    {
        public string Name { get; set; }

        public ShipCost Cost { get; set; } =
            new ShipCost();

        public ShipCharacteristics ShipCharacteristics { get; set; } =
            new ShipCharacteristics();

        public GameObject Hull { get; private set; }

        public List<ModuleTemplate> Modules { get; private set; } =
            new List<ModuleTemplate>();

        public ShipTemplate()
        {
        }

        public void CalculateCost()
        {
            if(Hull == null)
            {
                return;
            }
            var hullDescription = Hull.GetComponent<Description>();
            Cost.Merilium = hullDescription.Merilium;
            Cost.Money = hullDescription.Money;
            Cost.Titan = hullDescription.Titan;
            Cost.Uranus = hullDescription.Uranus;
            Cost.Time = hullDescription.Time;
            foreach (var module in Modules)
            {
                if (module.ChosenModule == null)
                {
                    continue;
                }
                var mDesc = module.ChosenModule.GetComponent<Description>();
                Cost.Merilium += mDesc.Merilium;
                Cost.Money += mDesc.Money;
                Cost.Titan += mDesc.Titan;
                Cost.Uranus += mDesc.Uranus;
                Cost.Time += mDesc.Time;
            }
        }

        public void CalculateCharacteristics()
        {
            if(Hull == null)
            {
                return;
            }
            var hullDescription = Hull.GetComponent<SpaceShipAI>();
            ShipCharacteristics.HP = hullDescription.Container.HP;
            ShipCharacteristics.Speed = Hull.GetComponent<Movement>().MaxVelocity;
            foreach(var module in Modules)
            {
                if(module.ChosenModule == null)
                {
                    continue;
                }
                if(module.ChosenModule.TryGetComponent(out ShellBase shellBase))
                {
                    if(ShipCharacteristics.MinAttackDistance > shellBase.AttackDistance
                        || ShipCharacteristics.MinAttackDistance == 0)
                    {
                        ShipCharacteristics.MinAttackDistance = shellBase.AttackDistance;
                    }
                    if (ShipCharacteristics.MaxAttackDistance < shellBase.AttackDistance
                        || ShipCharacteristics.MaxAttackDistance == 0)
                    {
                        ShipCharacteristics.MaxAttackDistance = shellBase.AttackDistance;
                    }
                }
                else if (module.ChosenModule.TryGetComponent(out Modifier modifier))
                {
                    ShipCharacteristics.HP *= modifier.HP;
                    ShipCharacteristics.Speed *= modifier.Speed;
                    ShipCharacteristics.AttackDistanceMultiplier *= modifier.AttackDistance;
                }
            }
            ShipCharacteristics.MinAttackDistance *= ShipCharacteristics.AttackDistanceMultiplier;
            ShipCharacteristics.MaxAttackDistance *= ShipCharacteristics.AttackDistanceMultiplier;
        }

        public void SetNewHull(GameObject hull)
        {
            if(Hull == hull)
            {
                return;
            }
            Modules.Clear();
            Hull = hull;
            foreach (Transform child in hull.transform)
            {
                Modules.Add(new ModuleTemplate
                {
                    ModuleSlot = child.gameObject,
                    ChosenModule = null,
                });
            }
        }
    }

    public class ShipCost : ICost
    {
        public int Money { get; set; }
        public int Merilium { get; set; }
        public int Titan { get; set; }
        public int Uranus { get; set; }
        public int Time { get; set; }
    }

    public class ShipCharacteristics
    {
        public float MinAttackDistance { get; set; }
        public float MaxAttackDistance { get; set; }
        public float AttackDistanceMultiplier { get; set; } = 1;
        public float HP { get; set; }
        public float Speed { get; set; }
    }

    public class ModuleTemplate
    {
        public GameObject ModuleSlot { get; set; }

        public GameObject ChosenModule { get; set; }
    }
}
