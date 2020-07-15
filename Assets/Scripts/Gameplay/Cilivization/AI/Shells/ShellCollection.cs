using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.AI.Shells
{
    public static class ShellCollection<TShell>
        where TShell : MonoBehaviour
    {
        private readonly static Stack<GameObject> _shells =
            new Stack<GameObject>();

        public static GameObject Get(
            GameObject original,
            Vector3 position,
            Quaternion rotation)
        {
            if (_shells.Count == 0)
            {
                return UnityEngine.Object.Instantiate(original, position, rotation);
            }
            else
            {
                var gameObject = _shells.Pop();
                gameObject.SetActive(true);
                gameObject.transform.position = position;
                gameObject.transform.rotation = rotation;
                return gameObject;
            }
        }

        public static void Destroy(
            GameObject gameObject)
        {
            gameObject.SetActive(false);
            _shells.Push(gameObject);
        }
    }
}
