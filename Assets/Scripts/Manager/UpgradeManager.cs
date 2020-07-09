using Assets.Scripts.Gameplay;
using Assets.Scripts.GlobalControllers;
using Assets.Scripts.Manager.ClassSystem;
using Assets.Scripts.Manager.Galaxy;
using Assets.Scripts.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public partial class GameManager : IUpgradeManager
    {
        [Serializable]
        public struct UpgradeContainer
        {
            public SpaceClasses SpaceClasses;
            public UpgraderBase UpgradeBase;
        }

        [Serializable]
        public struct Mapping
        {
            public SpaceClasses Downgrade;
            public SpaceClasses Source;
            public SpaceClasses Upgrade;
            public int criticalMassUpgrade;
            public int criticalMassDowngrade;
        }

        [SerializeField]
        public UpgradeContainer[] AvailableUpgrades;

        [SerializeField]
        public Mapping[] AvailableMapping;

        public void Upgrade(SpaceBody spaceBody, Mapping mapping)
        {
            if(mapping.criticalMassUpgrade < spaceBody.Mass)
            {
                Upgrade(mapping.Upgrade, spaceBody);
            }
            else if(mapping.criticalMassDowngrade > spaceBody.Mass)
            {
                Upgrade(mapping.Downgrade, spaceBody);
            }
        }

        private void Upgrade(SpaceClasses spaceClass, SpaceBody spaceBody)
        {
            AvailableUpgrades.First(x => x.SpaceClasses == spaceClass)
                .UpgradeBase.Upgrade(this, spaceBody);
            spaceBody.mappingUpgradeSpaceObject = AvailableMapping.First(x => x.Source == spaceBody.SpaceClass);
        }

        public SpaceBody GetFullObject(SpaceClasses spaceClass)
        {
            SpaceBody spaceBody = GenerateSpaceBody;
            spaceBody.SpaceClass = spaceClass;
            spaceBody.upgradeManager = this;
            Upgrade(spaceClass, spaceBody);
            spaceBody.mappingUpgradeSpaceObject = AvailableMapping.First(x => x.Source == spaceBody.SpaceClass);
            return spaceBody;
        }
    }
}
