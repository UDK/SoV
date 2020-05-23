using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*MIT License

Copyright (c) 2018 Sebastian Lague

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/
namespace GeneratePlanet
{
    public class ShapeGenerator
    {

        ShapeSettings settings;
        INoiseFilter[] noiseFilters;
        public MinMax elevationMinMax;

        public void UpdateSettings(ShapeSettings settings)
        {
            this.settings = settings;
            noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
            for (int i = 0; i < noiseFilters.Length; i++)
            {
                noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
            }
            elevationMinMax = new MinMax();
        }

        public float CalculateUnscaledElevation(Vector3 pointOnUnitSphere)
        {
            float firstLayerValue = 0;
            float elevation = 0;

            if (noiseFilters.Length > 0)
            {
                firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
                if (settings.noiseLayers[0].enabled)
                {
                    elevation = firstLayerValue;
                }
            }

            for (int i = 1; i < noiseFilters.Length; i++)
            {
                if (settings.noiseLayers[i].enabled)
                {
                    float mask = (settings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValue : 1;
                    elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
                }
            }
            elevationMinMax.AddValue(elevation);
            return elevation;
        }

        public float GetScaledElevation(float unscaledElevation)
        {
            float elevation = Mathf.Max(0, unscaledElevation);
            elevation = settings.planetRadius * (1 + elevation);
            return elevation;
        }

        public bool SaveJsonPrefab()
        {
            try
            {
                string json = JsonUtility.ToJson(settings);
                File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "ShapePrefab.json", json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}