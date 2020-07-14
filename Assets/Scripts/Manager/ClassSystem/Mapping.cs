using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Manager.ClassSystem
{
    public struct Mapping
    {
        public SpaceClasses Downgrade;
        public SpaceClasses Source;
        public SpaceClasses Upgrade;
        public int criticalMassUpgrade;
        public int criticalMassDowngrade;
    }
}
