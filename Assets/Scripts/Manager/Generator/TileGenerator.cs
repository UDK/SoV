using Assets.Scripts.Maths.NoiseFabrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Manager.Generator
{
    /// <summary>
    /// Избавиться от интерфейса
    /// </summary>
    /// <typeparam name="TNoiseAdapter"></typeparam>
    public class TileGenerator<TNoiseAdapter> : ITileGenerator
        where TNoiseAdapter: INoiseFabric<TileType>, new()
    {
        private readonly TNoiseAdapter _noiseAdapter;
        private readonly TileType[] _tileTypes;

        public TileGenerator(TileType[] tileTypes)
        {
            _noiseAdapter = new TNoiseAdapter();
            _tileTypes = tileTypes;
        }

        public TileType[,] GenerateTileMap(int height, int width, int scale) =>
            _noiseAdapter.GenerateNoise(height, width, scale, Map);

        private TileType Map(float height)
        {
            // for each terrain type, check if the height is lower than the one for the terrain type
            foreach (TileType terrainType in _tileTypes)
            {
                // return the first terrain type whose height is higher than the generated one
                if (height < terrainType.Probability)
                {
                    return terrainType;
                }
            }
            return _tileTypes[0];
        }
    }
}
