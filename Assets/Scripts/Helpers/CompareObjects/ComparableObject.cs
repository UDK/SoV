using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using UnityEngine;
using Assets.Scripts.Gameplay.Cilivization.AI.Shells;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Scripts.Helpers.CompareObjects
{
#if UNITY_EDITOR
    public class SelectAllOfComparableObject : MonoBehaviour
    {
        [MenuItem("Game helpers/Update all comparable objects")]
        static void GetAllComparable()
        {
            var guids = AssetDatabase.FindAssets("t:Object", new[] { "Assets/Prefabs" });
            //var selected = new List<UnityEngine.Object>();
            var count = 0;

            foreach (string guid in guids)
            {
                string myObjectPath = AssetDatabase.GUIDToAssetPath(guid);
                UnityEngine.Object[] myObjs = AssetDatabase.LoadAllAssetsAtPath(myObjectPath);

                foreach (UnityEngine.Object thisObject in myObjs)
                {
                    if (thisObject is Comparable c)
                    {
                        c.ObjectUniqueId = Guid.NewGuid().ToString();
                        count++;
                    }
                }
            }
            Debug.Log("Updated comparable objects: " + count);
        }
    }

    /*[CanEditMultipleObjects]
    [CustomEditor(typeof(Comparable))]
    public class ComparableObject : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Generate ID"))
            {
                foreach (var obj in targets)
                {
                    Debug.Log(targets.Length);
                    Comparable go = obj as Comparable;
                    go.ObjectUniqueId = Guid.NewGuid().ToString();
                }
            }
            DrawDefaultInspector();
        }
    }*/

#endif
}
