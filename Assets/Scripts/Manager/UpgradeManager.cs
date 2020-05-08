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

        [SerializeField]
        public UpgradeContainer[] AvailableUpgrades;

        public void Upgrade(SpaceClasses spaceClass, SpaceBody spaceBody)
        {
            AvailableUpgrades.First(x => x.SpaceClasses == spaceClass)
                .UpgradeBase.Upgrade(this, spaceBody);
        }

        public SpaceBody GetFullObject(SpaceClasses spaceClass)
        {
            SpaceBody spaceBody = GenerateSpaceBody;
            spaceBody.SpaceClass = spaceClass;
            Upgrade(spaceClass, spaceBody);
            return spaceBody;
        }
    }
}
