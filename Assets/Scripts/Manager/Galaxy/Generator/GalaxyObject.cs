using Assets.Scripts.Manager.ClassSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Manager.Galaxy.Generator
{
    [Serializable]
    public class GalaxyObject
    {
        [SerializeField]
        public SpaceClasses SpaceClass;

        public float UpperProbabilityGround;
    }
}
