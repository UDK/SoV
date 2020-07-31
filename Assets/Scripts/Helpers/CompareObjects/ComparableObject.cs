using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Assets.Scripts.Gameplay.Cilivization.AI.Shells;

namespace Assets.Scripts.Helpers.CompareObjects
{
#if UNITY_EDITOR

    class SelectAllOfComparableObject : ScriptableWizard
    {
        [MenuItem("Example/Select All of Tag...")]
        static void SelectAllOfTagWizard()
        {
            ScriptableWizard.DisplayWizard(
                "Select All of ComparableObject...",
                typeof(SelectAllOfComparableObject),
                "Make Selection");
        }

        void OnWizardCreate()
        {
            Debug.Log(Resources.LoadAll<Comparable>("Prefabs").Length);
            var guids = AssetDatabase.FindAssets("t:Object", new[] { "Assets/Prefabs" });
            Debug.Log(guids.Length);
            /*Selection.objects = guids.Select(g =>
                AssetDatabase.LoadAssetAtPath<Comparable>(
                    AssetDatabase.GUIDToAssetPath(g))).ToArray();*/
        }
    }

    [CanEditMultipleObjects]
    [CustomEditor(typeof(Comparable))]
    public class ComparableObject : Editor
    {
        SerializedProperty _objectUniqueId;

        void OnEnable()
        {
            _objectUniqueId = serializedObject.FindProperty("ObjectUniqueId");
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Generate ID"))
            {
                Debug.Log(targets.Length);
                serializedObject.Update();
                _objectUniqueId.stringValue = Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
                /*foreach (var obj in targets)
                {
                    Debug.Log(targets.Length);
                    Comparable go = obj as Comparable;
                    go.ObjectUniqueId = Guid.NewGuid();
                }*/
            }
            Comparable comparable = target as Comparable;
            EditorGUILayout.TextField("ObjectUniqueId:", comparable.ObjectUniqueId.ToString());
            DrawDefaultInspector();
        }
    }

#endif
}
