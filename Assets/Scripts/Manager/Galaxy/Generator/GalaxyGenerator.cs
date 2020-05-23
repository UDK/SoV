using Assets.Scripts.Maths.NoiseFabrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Manager.Galaxy.Generator
{
    /// <summary>
    /// Избавиться от интерфейса
    /// </summary>
    /// <typeparam name="TNoiseAdapter"></typeparam>
    public class GalaxyGenerator<TNoiseAdapter> : IGalaxyGenerator
        where TNoiseAdapter: INoiseFabric<GalaxyObject>, new()
    {
        private readonly TNoiseAdapter _noiseAdapter;
        private readonly GalaxyObject[] _tileTypes;

        public GalaxyGenerator(GalaxyObject[] tileTypes)
        {
            _noiseAdapter = new TNoiseAdapter();
            _tileTypes = tileTypes;
        }

        public GalaxyObject[,] GenerateTileMap(int height, int width, int scale) =>
            _noiseAdapter.GenerateNoise(height, width, scale, Map);

        private GalaxyObject Map(float height)
        {
            // for each terrain type, check if the height is lower than the one for the terrain type
            foreach (GalaxyObject terrainType in _tileTypes)
            {
                // return the first terrain type whose height is higher than the generated one
                if (height < terrainType.UpperProbabilityGround)
                {
                    return terrainType;
                }
            }
            return _tileTypes[0];
        }
    }
}
