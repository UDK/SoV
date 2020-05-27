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
        public override void Upgrade(
            ITemplateManager templateManager,
            SpaceBody spaceBody)
        {
            ClearMesh(spaceBody);
            int colourSettingsRandom = UnityEngine.Random.Range(0,planetSettings.colourSettings.Length);
            int shapeSettingsRandom = UnityEngine.Random.Range(0,planetSettings.shapeSettings.Length);
            Planet planet = spaceBody.gameObject.AddComponent<Planet>();
            planet.Init(planetSettings.shapeSettings[shapeSettingsRandom], planetSettings.colourSettings[colourSettingsRandom], planetSettings.resolution);
            var graviatation = templateManager.SetUpGravitation(spaceBody);
            spaceBody.Mass += 10;
            var satelliteManager = spaceBody.GetComponent<SatelliteManager>();
            satelliteManager.MaxCountSattelites = 2;
            spaceBody.gameObject.layer = LayerHelper.Planet;
            spaceBody = SpinPlanet(spaceBody);
        }
        /// <summary>
        /// Крутим планету при инициализации(ебашу чистые функции чтобы попасть в рай
        /// </summary>
        /// <param name="spaceBody">Объект планеты</param>
        /// <returns></returns>
        private SpaceBody SpinPlanet(SpaceBody spaceBody)
        {
            spaceBody.gameObject.transform.Rotate(new Vector3(0, UnityEngine.Random.Range(0f, 359f), 0));
            return spaceBody;
        }
    }
    [Serializable]
    public class PlanetSettings
    {
        [SerializeField]
        public ShapeSettings[] shapeSettings;
        [SerializeField]
        public ColourSettings[] colourSettings;
        [SerializeField]
        public int resolution;
    }
}
