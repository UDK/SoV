using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Maths.NoiseFabrics
{
    public class PerlinNoise<TTile> : INoiseFabric<TTile>
    {
        public TTile[,] GenerateNoise(
            int heigth,
            int width,
            float scale,
            Func<float, TTile> transformator)
        {
            // create an empty noise map with the mapDepth and mapWidth coordinates
            TTile[,] noiseMap = new TTile[heigth, width];

            for (int zIndex = 0; zIndex < heigth; zIndex++)
            {
                for (int xIndex = 0; xIndex < width; xIndex++)
                {
                    // calculate sample indices based on the coordinates and the scale
                    float sampleX = xIndex / scale;
                    float sampleZ = zIndex / scale;

                    // generate noise value using PerlinNoise
                    float noise = Mathf.PerlinNoise(sampleX, sampleZ);

                    // map noise out to tile
                    noiseMap[zIndex, xIndex] = transformator(noise);
                }
            }

            return noiseMap;
        }
    }
}
