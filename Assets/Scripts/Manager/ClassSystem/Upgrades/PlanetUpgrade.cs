using Assets.Scripts.Gameplay;
using Assets.Scripts.Helpers;
using Assets.Scripts.Physics.Sattellite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Manager.ClassSystem.Upgrades
{
    public class PlanetUpgrade : UpgraderBase
    {
        public override void Upgrade(
            ITemplateManager templateManager,
            SpaceBody spaceBody)
        {
            var graviatation = templateManager.SetUpGravitation(spaceBody);
            spaceBody.Mass += 10;
            var satelliteManager = spaceBody.GetComponent<SatelliteManager>();
            satelliteManager.MaxCountSattelites = 2;

            spaceBody.gameObject.layer = LayerHelper.Planet;
        }
    }
}
