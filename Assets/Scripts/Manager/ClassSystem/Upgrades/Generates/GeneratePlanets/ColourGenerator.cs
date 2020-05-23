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
    public class ColourGenerator
    {

        ColourSettings settings;
        Texture2D texture;
        const int textureResolution = 50;
        INoiseFilter biomeNoiseFilter;

        public void UpdateSettings(ColourSettings settings)
        {
            this.settings = settings;
            if (texture == null || texture.height != settings.biomeColourSettings.biomes.Length)
            {
                texture = new Texture2D(textureResolution * 2, settings.biomeColourSettings.biomes.Length, TextureFormat.RGBA32, false);
            }
            biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColourSettings.noise);
        }

        public void UpdateElevation(MinMax elevationMinMax)
        {
            settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
        }

        public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
        {
            float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
            heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - settings.biomeColourSettings.noiseOffset) * settings.biomeColourSettings.noiseStrength;
            float biomeIndex = 0;
            int numBiomes = settings.biomeColourSettings.biomes.Length;
            float blendRange = settings.biomeColourSettings.blendAmount / 2f + .001f;

            for (int i = 0; i < numBiomes; i++)
            {
                float dst = heightPercent - settings.biomeColourSettings.biomes[i].startHeight;
                float weight = Mathf.InverseLerp(-blendRange, blendRange, dst);
                biomeIndex *= (1 - weight);
                biomeIndex += i * weight;
            }

            return biomeIndex / Mathf.Max(1, numBiomes - 1);
        }

        public void UpdateColours()
        {
            Color[] colours = new Color[texture.width * texture.height];
            int colourIndex = 0;
            foreach (var biome in settings.biomeColourSettings.biomes)
            {
                for (int i = 0; i < textureResolution * 2; i++)
                {
                    Color gradientCol;
                    if (i < textureResolution)
                    {
                        gradientCol = settings.oceanColour.Evaluate(i / (textureResolution - 1f));
                    }
                    else
                    {
                        gradientCol = biome.gradient.Evaluate((i - textureResolution) / (textureResolution - 1f));
                    }
                    Color tintCol = biome.tint;
                    colours[colourIndex] = gradientCol * (1 - biome.tintPercent) + tintCol * biome.tintPercent;
                    colourIndex++;
                }
            }
            texture.SetPixels(colours);
            texture.Apply();
            settings.planetMaterial.SetTexture("_texture", texture);
        }
        public bool SaveJsonPrefab()
        {
            try
            {
                string json = JsonUtility.ToJson(settings);
                //здесь прибавлять ещё хэш
                File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "ColorPrefab.json", json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
