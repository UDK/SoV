using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.Workshop
{
    [Serializable]
    public class ShipBuildTemplateMB : IShipBuildTemplate
    {
        [field: SerializeField]
        public string Name { get; set; }

        [field: SerializeField]
        public GameObject Hull { get; set; }

        [field: SerializeField]
        public List<ModuleTemplate> Modules { get; set; }

        public static implicit operator ShipBuildTemplate(ShipBuildTemplateMB obj)
        {
            return new ShipBuildTemplate(obj.Name, obj.Hull, obj.Modules);
        }
    }
}
