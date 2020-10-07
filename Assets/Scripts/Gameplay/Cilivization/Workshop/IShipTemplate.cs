using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.Workshop
{
    public interface IShipBuildTemplate
    {
        string Name { get; }

        GameObject Hull { get; }

        List<ModuleTemplate> Modules { get; }
    }

    [Serializable]
    public class ModuleTemplate
    {

        [field: SerializeField]
        public GameObject ModuleSlot { get; set; }

        [field: SerializeField]
        public GameObject ChosenModule { get; set; }
    }
}
