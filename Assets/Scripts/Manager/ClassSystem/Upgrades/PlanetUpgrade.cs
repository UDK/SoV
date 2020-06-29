using Assets.Scripts.Gameplay;
using Assets.Scripts.Helpers;
using Assets.Scripts.Physics.Sattellite;
using GeneratePlanet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Manager.ClassSystem.Upgrades
{
    public class PlanetUpgrade : UpgraderBase
    {
        [SerializeField]
        public PlanetSettings planetSettings;
        protected override void UpgradeClass(
            ITemplateManager templateManager,
            SpaceBody spaceBody)
        {
            int colourSettingsRandom = UnityEngine.Random.Range(0,planetSettings.colourSettings.Length);
            int shapeSettingsRandom = UnityEngine.Random.Range(0,planetSettings.shapeSettings.Length);
            Planet planet = spaceBody.gameObject.AddComponent<Planet>();
            planet.Init(planetSettings.shapeSettings[shapeSettingsRandom], planetSettings.colourSettings[colourSettingsRandom]);
            var graviatation = templateManager.SetUpGravitation(spaceBody);
            spaceBody.Mass += 10;
            var satelliteManager = spaceBody.GetComponent<SatelliteManager>();
            satelliteManager.MaxCountSattelites = 2;

            spaceBody.gameObject.layer = LayerHelper.Planet;
        }
    }
    [Serializable]
    public class PlanetSettings
    {
        [SerializeField]
        public ShapeSettings[] shapeSettings;
        [SerializeField]
        public ColourSettings[] colourSettings;

    }
}
