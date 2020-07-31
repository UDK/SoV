using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.AI.Shells
{
    public static class ShellCollection
    {
        /// <summary>
        /// key is original
        /// value is stack of created children
        /// </summary>
        private readonly static Dictionary<GameObject, Stack<GameObject>> _shells =
            new Dictionary<GameObject, Stack<GameObject>>();

        public static GameObject Get(
            GameObject original,
            Vector3 position,
            Quaternion rotation)
        {
            /////
            ///ПЕРЕПИСАТЬ на ComparableClass
            /////
            Stack<GameObject> stackShell = GetStack(original);

            if (stackShell.Count == 0)
            {
                return UnityEngine.Object.Instantiate(original, position, rotation);
            }
            else
            {
                var gameObject = stackShell.Pop();
                gameObject.SetActive(true);
                gameObject.transform.position = position;
                gameObject.transform.rotation = rotation;
                return gameObject;
            }
        }

        private static Stack<GameObject> GetStack(GameObject original)
        {
            Stack<GameObject> stackShell;
            if (!_shells.ContainsKey(original))
            {
                stackShell = new Stack<GameObject>();
                _shells.Add(original, stackShell);
            }
            else
            {
                stackShell = _shells[original];
            }

            return stackShell;
        }

        public static void Destroy(
            GameObject gameObject)
        {
            //Stack<GameObject> stackShell = GetStack(original);
            //gameObject.SetActive(false);
            //stackShell.Push(gameObject);
        }
    }
}
