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
        /*private readonly static Dictionary<GameObject, Comparable> _comparableMapping =
            new Dictionary<GameObject, Comparable>();*/

        /// <summary>
        /// key is original
        /// value is stack of created children
        /// </summary>
        private readonly static List<KeyValuePair<GameObject, Stack<GameObject>>> _shells =
            new List<KeyValuePair<GameObject, Stack<GameObject>>>();

        public static GameObject Get(
            GameObject original,
            Vector3 position,
            Quaternion rotation)
        {
            /*var c = GetComparable(original);*/
            Stack<GameObject> stackShell = GetStack(original);

            if (stackShell.Count == 0)
            {
                var obj = UnityEngine.Object.Instantiate(original, position, rotation);
                obj.name = original.name;
                return obj;
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

        /*private static Comparable GetComparable(GameObject original)
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
        }*/

        private static Stack<GameObject> GetStack(GameObject original)
        {
            Stack<GameObject> stackShell;
            if (!_shells.Any(x => x.Key.name == original.name))
            {
                stackShell = new Stack<GameObject>();
                _shells.Add(new KeyValuePair<GameObject, Stack<GameObject>>(original, stackShell));
            }
            else
            {
                stackShell = _shells.First(x => x.Key.name == original.name).Value;
            }

            return stackShell;
        }

        public static void Destroy(
            GameObject gameObject)
        {
            Stack<GameObject> stackShell = GetStack(gameObject);
            gameObject.SetActive(false);
            stackShell.Push(gameObject);
        }
    }
}
