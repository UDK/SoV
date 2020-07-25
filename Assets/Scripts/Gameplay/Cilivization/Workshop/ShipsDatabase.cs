using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.Workshop
{
    public class ShipsDatabase : MonoBehaviour
    {
        /// <summary>
        /// hulls of spaceships
        /// </summary>
        [SerializeField]
        public List<GameObject> Hulls =
            new List<GameObject>();

        /// <summary>
        /// key is a weapon
        /// value is a shell array
        /// </summary>
        [SerializeField]
        public List<WeaponMapping> WeaponMappings =
            new List<WeaponMapping>();

        public List<GameObject> GetShells(
            GameObject weapon) =>
            WeaponMappings.First(x =>
                weapon.tag == x.Weapon.tag).AvailableShells;

        [Serializable]
        public class WeaponMapping
        {
            public GameObject Weapon;

            public List<GameObject> AvailableShells;
        }
    }
}
