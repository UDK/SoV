using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScaleToSizeCamera))]
public class ScaleToSizeCameraEditor : Editor
{
    ScaleToSizeCamera scaleToSizeCamera;

    public override void OnInspectorGUI()
    {

        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
            }
        }

        if (GUILayout.Button("ChangeScale"))
        {
            
        }

    }
    private void OnEnable()
    {
        if (scaleToSizeCamera == null)
            scaleToSizeCamera = (ScaleToSizeCamera)target;
    }
}
