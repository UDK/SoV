using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Manager.ClassSystem
{
    [Serializable]
    public struct Mapping
    {
        public SpaceClasses Downgrade;
        public SpaceClasses Source;
        public SpaceClasses Upgrade;
        //Сделал float, чтобы не было боксингов и анбоксингов при преобразовании
        public float criticalMassUpgrade;
        public float criticalMassDowngrade;
    }
}
