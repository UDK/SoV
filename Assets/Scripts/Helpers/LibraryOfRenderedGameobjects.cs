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

        private static readonly Dictionary<GameObject, RenderContainer> _renders =
            new Dictionary<GameObject, RenderContainer>();

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
            if (!_renders.ContainsKey(template))
            {
                container = AddTexture(template);
            }
            else
            {
                container = _renders[template];
            }

            if(getSomeData != null)
            {
                container.Storage = getSomeData.Invoke(container.Scaling);
            }
            return container;
        }

        public static Vector3? WorldToCameraTexture(Vector3 vector3) =>
            _virtualCamera?.WorldToScreenPoint(vector3);

        private static RenderContainer AddTexture(
            GameObject template)
        {
            var container = Render(template);
            _renders.Add(template, container);
            return container;
        }

        private static RenderContainer Render(
            GameObject template)
        {
            if(_virtualCamera == null)
            {
                GameObject gameObject = new GameObject();
                Camera camera = gameObject.AddComponent<Camera>();
                camera.name = $"Camera for renderings";
                camera.orthographic = true;
                camera.transform.SetPositionAndRotation(
                    Vector3.zero,
                    Quaternion.identity);
                camera.backgroundColor = new Color(0, 0, 0, 0);
                camera.cullingMask = 1 << LayerHelper.RenderedObjects;
                camera.gameObject.layer = LayerHelper.RenderedObjects;
                _virtualCamera = camera;
                RenderTexture renderTexture = new RenderTexture(512, 256, 24, RenderTextureFormat.ARGB32)
                {
                    name = "render" + _renders.Count,
                    graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm,
                    wrapMode = TextureWrapMode.Clamp,
                    filterMode = FilterMode.Bilinear,
                    dimension = UnityEngine.Rendering.TextureDimension.Tex2D
                };
                _virtualCamera.targetTexture = renderTexture;
            }
            else
            {
                _virtualCamera.gameObject.SetActive(true);
            }

            var gameobject = UnityEngine.Object.Instantiate(template);
            gameobject.transform.parent = _virtualCamera.transform;
            gameobject.transform.position = new Vector3(0, 0, 10);
            gameobject.layer = LayerHelper.RenderedObjects;
            var scale = UnityHelpers.StretchGameobjectToCamera(gameobject, _virtualCamera);
            _virtualCamera.Render();

            UnityEngine.Object.DestroyImmediate(gameobject);
            RenderTexture.active = null;
            _virtualCamera.gameObject.SetActive(false);

            Texture2D texture = new Texture2D(512, 256, TextureFormat.ARGB32, false)
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


        private static void SetComponentEnabled(Component component, bool value)
        {
            if (component == null) return;
            if (component is Collider)
            {
                (component as Collider).enabled = value;
            }
            else if (component is Collider2D)
            {
                (component as Collider2D).enabled = value;
            }
            else if (component is Animation)
            {
                (component as Animation).enabled = value;
            }
            else if (component is Animator)
            {
                (component as Animator).enabled = value;
            }
            else if (component is AudioSource)
            {
                (component as AudioSource).enabled = value;
            }
            else if (component is MonoBehaviour)
            {
                (component as MonoBehaviour).enabled = value;
            }
            else
            {
                Debug.Log("Don't know how to enable " + component.GetType().Name);
            }
        }

        public struct RenderContainer
        {
            public TStorage Storage { get; set; }

            public Texture RenderedTexture { get; set; }

            public float Scaling { get; set; }
        }
    }
}
