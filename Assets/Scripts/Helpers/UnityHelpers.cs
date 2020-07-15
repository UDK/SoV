using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
