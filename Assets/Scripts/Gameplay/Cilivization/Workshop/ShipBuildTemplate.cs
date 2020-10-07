using Assets.Scripts.Gameplay.Cilivization.AI;
using Assets.Scripts.Gameplay.Cilivization.AI.Modilfiers;
using Assets.Scripts.Gameplay.Cilivization.AI.Shells;
using Assets.Scripts.Gameplay.Cilivization.AI.WeaponSlots;
using Assets.Scripts.Gameplay.Cilivization.Base;
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
    public class ShipBuildTemplate : IShipBuildTemplate, IShipTemplate
    {
        public string Name { get; set; }

        public ShipCost Cost { get; private set; } =
            new ShipCost();

        public GameObject Hull { get; private set; }

        public List<ModuleTemplate> Modules { get; private set; } =
            new List<ModuleTemplate>();

        public GameObject BuiltTemplate { get; private set; }

        public ShipCharacteristics ShipCharacteristics { get; private set; } =
            new ShipCharacteristics();

        public ShipBuildTemplate()
        {
        }
        public ShipBuildTemplate(
            string name,
            GameObject hull,
            List<ModuleTemplate> modules)
        {
            Name = name;
            Hull = hull;
            Modules = modules;
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
            var hullDescription = Hull.GetComponent<ISpaceShipAI>();
            ShipCharacteristics = new ShipCharacteristics();
            ShipCharacteristics.HP = hullDescription.HP;
            ShipCharacteristics.Speed = Hull.GetComponent<Movement>().MaxVelocity;
            ShipCharacteristics.MinAttackDistance = hullDescription.MinAttackDistance;
            foreach(var module in Modules)
            {
                if(module.ChosenModule == null)
                {
                    continue;
                }
                if(module.ChosenModule.TryGetComponent(out WeaponBase weapon))
                {
                    if(ShipCharacteristics.MinAttackDistance > weapon.AttackDistance
                        || ShipCharacteristics.MinAttackDistance == 0)
                    {
                        ShipCharacteristics.MinAttackDistance = weapon.AttackDistance;
                    }
                    if (ShipCharacteristics.MaxAttackDistance < weapon.AttackDistance
                        || ShipCharacteristics.MaxAttackDistance == 0)
                    {
                        ShipCharacteristics.MaxAttackDistance = weapon.AttackDistance;
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
            if(Hull.name == hull.name)
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

        public IShipTemplate Build()
        {
            if(BuiltTemplate != null)
            {
                UnityEngine.Object.DestroyImmediate(BuiltTemplate);
            }

            CalculateCharacteristics();
            CalculateCost();

            var newTemplate = UnityEngine.Object.Instantiate(
                Hull,
                Hull.transform.position,
                Quaternion.identity);
            newTemplate.SetActive(false);
            for(int i = 0; i < Modules.Count; i++)
            {
                var module = newTemplate.transform.GetChild(i).gameObject.CheckComponent<WeaponBase>(
                    wb =>
                    {
                        wb.Shell =
                            Modules[i].ChosenModule;
                        wb.AttackDistance *= ShipCharacteristics.AttackDistanceMultiplier;
                    });
            }
            var hull = newTemplate.GetComponent<ISpaceShipAI>();
            hull.HP = ShipCharacteristics.HP;
            hull.MinAttackDistance = ShipCharacteristics.MinAttackDistance;
            hull.SightDist = ShipCharacteristics.MinAttackDistance * 2;
            newTemplate.GetComponent<Movement>().MaxVelocity = ShipCharacteristics.Speed;
            BuiltTemplate = newTemplate;
            return this;
        }
    }

    public class ShipCharacteristics
    {
        public float MinAttackDistance { get; set; }
        public float MaxAttackDistance { get; set; }
        public float AttackDistanceMultiplier { get; set; } = 1;
        public float HP { get; set; }
        public float Speed { get; set; }
    }
}
