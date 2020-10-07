using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization
{
    public class Database : MonoBehaviour
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
        public List<ModuleMapping> ModuleMappings =
            new List<ModuleMapping>();

        public List<GameObject> GetShells(
            GameObject module) =>
            ModuleMappings.First(x =>
                module.name == x.ModuleSlot.name).AvailableModules;

        [Serializable]
        public class ModuleMapping
        {
            public GameObject ModuleSlot;

            public List<GameObject> AvailableModules;
        }
    }
}
