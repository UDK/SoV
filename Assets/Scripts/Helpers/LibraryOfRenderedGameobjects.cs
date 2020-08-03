using Assets.Scripts.Gameplay.SpaceObject;
using Assets.Scripts.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class LibraryOfRenderedGameobjects<TStorage>
        where TStorage : class
    {
        public delegate TStorage StoreAdditionalData(
            float scaling);

        private static int _width = 1024;

        private static int _height= 1024;

        private static readonly List<KeyValuePair<GameObject, RenderContainer>> _renders =
            new List<KeyValuePair<GameObject, RenderContainer>>();

        private static Camera _virtualCamera;

        public static RenderContainer GetTexture(
            GameObject template)
        {
            return GetTexture(template, null);
        }

        /// <summary>
        /// Get texture
        /// </summary>
        /// <param name="template">Template of gameobject</param>
        /// <param name="getSomeData">If it is not null it will give
        /// control for getting new stored item</param>
        /// <returns></returns>
        public static RenderContainer GetTexture(
            GameObject template,
            StoreAdditionalData getSomeData)
        {
            RenderContainer container;
            if (!_renders.Any(x => x.Key.name == template.name))
            {
                container = AddTexture(template);
            }
            else
            {
                container = _renders.First(x => x.Key.name == template.name).Value;
            }

            if(getSomeData != null)
            {
                container.Storage = getSomeData.Invoke(container.Scaling);
            }
            return container;
        }

        public static Vector3? WorldToCameraTexture(Vector3 vector3) =>
            _virtualCamera?.WorldToScreenPoint(vector3);

        public static Vector2 GetSizes =>
            new Vector2(_width, _height);

        private static RenderContainer AddTexture(
            GameObject template)
        {
            var container = Render(template);
            _renders.Add(new KeyValuePair<GameObject, RenderContainer>(template, container));
            return container;
        }

        private static RenderContainer Render(
            GameObject template)
        {
            if(_virtualCamera == null)
            {
                GameObject gameObject = new GameObject();
                Camera camera = gameObject.AddComponent<Camera>();
                camera.transform.position = new Vector3(0, 0, 0);
                camera.name = $"Camera for renderings";
                camera.orthographic = true;
                camera.transform.SetPositionAndRotation(
                    Vector3.zero,
                    Quaternion.identity);
                camera.backgroundColor = new Color(0, 0, 0, 0);
                camera.cullingMask = 1 << LayerHelper.RenderedObjects;
                camera.gameObject.layer = LayerHelper.RenderedObjects;
                _virtualCamera = camera;
                RenderTexture renderTexture = new RenderTexture(_width, _height, 24, RenderTextureFormat.ARGB32)
                {
                    name = "render" + _renders.Count,
                    graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm,
                    wrapMode = TextureWrapMode.Clamp,
                    filterMode = FilterMode.Bilinear,
                    dimension = UnityEngine.Rendering.TextureDimension.Tex2D
                };
                GameObject gameObjectLight = new GameObject();
                Light light = gameObjectLight.AddComponent<Light>();
                light.type = LightType.Point;
                light.transform.parent = gameObject.transform;
                light.cullingMask = 1 << LayerHelper.RenderedObjects;
                gameObjectLight.layer = LayerHelper.RenderedObjects;
                light.range = 150;
                light.intensity = 150;
                light.transform.localPosition = new Vector3(0, 0, 0);
                _virtualCamera.targetTexture = renderTexture;
            }
            else
            {
                _virtualCamera.gameObject.SetActive(true);
            }

            var gameobject = UnityEngine.Object.Instantiate(template);
            gameobject.transform.parent = _virtualCamera.transform;
            gameobject.transform.localPosition = new Vector3(0, 0, 10);
            gameobject.layer = LayerHelper.RenderedObjects;
            var scale = UnityHelpers.StretchGameobjectToCamera(gameobject, _virtualCamera);
            _virtualCamera.Render();

            UnityEngine.Object.DestroyImmediate(gameobject);
            RenderTexture.active = null;
            _virtualCamera.gameObject.SetActive(false);

            Texture2D texture = new Texture2D(_width, _height, TextureFormat.ARGB32, false)
            {
                name = "Rendered" + template.name
            };
            Graphics.CopyTexture(_virtualCamera.targetTexture, texture);
            // dispose render texture
            return new RenderContainer
            {
                RenderedTexture = texture,
                Scaling = scale,
            };
        }

        public struct RenderContainer
        {
            public TStorage Storage { get; set; }

            public Texture RenderedTexture { get; set; }

            public float Scaling { get; set; }
        }
    }
}
