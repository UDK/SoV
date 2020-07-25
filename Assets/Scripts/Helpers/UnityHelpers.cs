using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class UnityHelpers
    {
        /// <summary>
        /// Returns gameObject if not success
        /// else returns null
        /// </summary>
        /// <typeparam name="TComponent">Desirable component</typeparam>
        /// <param name="go">Original gameobject</param>
        /// <param name="success">Action in case of success</param>
        /// <returns></returns>
        public static GameObject CheckComponent<TComponent>(
            this GameObject go,
            Action<TComponent> success)
            where TComponent : MonoBehaviour
        {
            if (go.TryGetComponent<TComponent>(out TComponent component))
            {
                success(component);
                return null;
            }
            else
            {
                return go;
            }
        }

        public static void DestroyAllChilds(
            this GameObject go)
        {
            while(go.transform.childCount != 0)
            {
                UnityEngine.Object.DestroyImmediate(go.transform.GetChild(0).gameObject);
            }
        }

        /// <summary>
        /// Get rectangle that wraps gameobject in XY space
        /// </summary>
        /// <param name="go">Gameobject itself</param>
        /// <returns>Rectangle that wraps gameobject</returns>
        public static Bounds GetRectangle2D_XY(
            GameObject go)
        {
            Vector3 cen = go.GetComponent<Renderer>().bounds.center;
            Vector3 ext = go.GetComponent<Renderer>().bounds.extents;
            Vector2[] extentPoints = new Vector2[8]
            {
                 new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z),
                 new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z),
                 new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z),
                 new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z),
                 new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z),
                 new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z),
                 new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z),
                 new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z)
            };
            Vector2 min = extentPoints[0];
            Vector2 max = extentPoints[0];
            foreach (Vector2 v in extentPoints)
            {
                min = Vector2.Min(min, v);
                max = Vector2.Max(max, v);
            }
            return new Bounds
            {
                min = min,
                max = max,
            };
        }

        public static float StretchGameobjectToCamera(
            GameObject go,
            Camera camera)
        {
            var rect = UnityHelpers.GetRectangle2D_XY(go);
            Vector3 origin = camera.WorldToScreenPoint(new Vector3(rect.min.x, rect.max.y, 0f));
            Vector3 extent = camera.WorldToScreenPoint(new Vector3(rect.max.x, rect.min.y, 0f));
            float xScale = (float)camera.pixelWidth / (extent.x - origin.x);
            float yScale = (float)camera.pixelHeight / (origin.y - extent.y);
            float commonScale = xScale > yScale ? yScale : xScale;
            go.transform.localScale = new Vector3(
                commonScale * go.transform.localScale.x,
                commonScale * go.transform.localScale.y,
                commonScale * go.transform.localScale.z);
            return commonScale;
        }
    }
}
