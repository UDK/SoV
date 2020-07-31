using Assets.Scripts.Helpers;
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

        public Resources Cost { get; set; }

        public GameObject Hull { get; private set; }

        public List<WeaponTemplate> Weapons { get; private set; } =
            new List<WeaponTemplate>();

        public ShipTemplate()
        {
        }

        public void SetNewHull(GameObject hull)
        {
            if(Hull == hull)
            {
                return;
            }
            Weapons.Clear();
            Hull = hull;
            foreach (Transform child in hull.transform)
            {
                Weapons.Add(new WeaponTemplate
                {
                    Weapon = child.gameObject,
                    ChosenShell = null,
                });
            }
        }
    }

    public class WeaponTemplate
    {
        public GameObject Weapon { get; set; }

        public GameObject ChosenShell { get; set; }
    }
}
