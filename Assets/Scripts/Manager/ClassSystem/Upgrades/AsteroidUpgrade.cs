using Assets.Scripts.Gameplay;
using Assets.Scripts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Manager.ClassSystem.Upgrades
{
    public class AsteroidUpgrade : UpgraderBase
    {
        public override void Upgrade(ITemplateManager templateManager, SpaceBody spaceBody)
        {
            spaceBody.gameObject.layer = LayerHelper.Asteroid;
            base.Upgrade(templateManager, spaceBody);
        }
    }
}
