using Assets.Scripts.Helpers.CompareObjects;
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
        private readonly static Dictionary<GameObject, Comparable> _comparableMapping =
            new Dictionary<GameObject, Comparable>();

        /// <summary>
        /// key is original
        /// value is stack of created children
        /// </summary>
        private readonly static Dictionary<Comparable, Stack<GameObject>> _shells =
            new Dictionary<Comparable, Stack<GameObject>>();

        public static GameObject Get(
            GameObject original,
            Vector3 position,
            Quaternion rotation)
        {
            var c = GetComparable(original);
            Stack<GameObject> stackShell = GetStack(c);

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

        private static Comparable GetComparable(GameObject original)
        {
            Comparable c;
            if (!_comparableMapping.ContainsKey(original))
            {
                c = original.c();
                _comparableMapping.Add(original, c);
            }
            else
            {
                c = _comparableMapping[original];
            }

            return c;
        }

        private static Stack<GameObject> GetStack(Comparable original)
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
            Stack<GameObject> stackShell = GetStack(gameObject.c());
            gameObject.SetActive(false);
            stackShell.Push(gameObject);
        }
    }
}
