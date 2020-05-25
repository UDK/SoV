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
    public class Planet : MonoBehaviour
    {

        [Range(2, 256)]
        public int resolution = 10;
        public bool autoUpdate = true;
        public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
        public FaceRenderMask faceRenderMask;

        public ShapeSettings shapeSettings;
        public ColourSettings colourSettings;

        [HideInInspector]
        public bool shapeSettingsFoldout;
        [HideInInspector]
        public bool colourSettingsFoldout;

        ShapeGenerator shapeGenerator = new ShapeGenerator();
        ColourGenerator colourGenerator = new ColourGenerator();

        [SerializeField, HideInInspector]
        MeshFilter[] meshFilters;
        TerrainFace[] terrainFaces;

        public void Init(ShapeSettings shapeSettingss, ColourSettings colourSettingss, int resolution)
        {
            this.resolution = resolution;
            shapeSettings = shapeSettingss;
            colourSettings = colourSettingss;
            GeneratePlanet();
        }

        void Initialize()
        {
            shapeGenerator.UpdateSettings(shapeSettings);
            colourGenerator.UpdateSettings(colourSettings);

            if (meshFilters == null || meshFilters.Length == 0)
            {
                meshFilters = new MeshFilter[6];
            }
            terrainFaces = new TerrainFace[6];

            Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

            for (int i = 0; i < 6; i++)
            {
                if (meshFilters[i] == null)
                {
                    GameObject meshObj = new GameObject("mesh");
                    meshObj.transform.parent = transform;

                    meshObj.AddComponent<MeshRenderer>();
                    meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                    meshFilters[i].sharedMesh = new Mesh();
                }
                meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;

                terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
                bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
                meshFilters[i].gameObject.SetActive(renderFace);
            }

        }


        public void GeneratePlanet()
        {
            Initialize();
            GenerateMesh();
            GenerateColours();
        }

        public void OnShapeSettingsUpdated()
        {
            if (autoUpdate)
            {
                Initialize();
                GenerateMesh();
            }
        }


        public void OnColourSettingsUpdated()
        {
            if (autoUpdate)
            {
                Initialize();
                GenerateColours();
            }
        }

        void GenerateMesh()
        {
            for (int i = 0; i < 6; i++)
            {
                if (meshFilters[i].gameObject.activeSelf)
                {
                    terrainFaces[i].ConstructMesh();
                }
            }

            colourGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
        }

        void GenerateColours()
        {
            colourGenerator.UpdateColours();
            for (int i = 0; i < 6; i++)
            {
                if (meshFilters[i].gameObject.activeSelf)
                {
                    terrainFaces[i].UpdateUVs(colourGenerator);
                }
            }
        }

        public void SavePrefab()
        {
            if (shapeGenerator.SaveJsonPrefab() &&
            colourGenerator.SaveJsonPrefab()
            )
            {

            }
        }

    }
}