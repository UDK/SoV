﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GetModelPlayer))]
public class NewBehaviourScript : Editor
{
    GetModelPlayer modelPlayer;

    public override void OnInspectorGUI()
    {
        
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                //planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("Change mass"))
        {
            modelPlayer.BodyPlanet.Mass = 50f;
        }


    }
    private void OnEnable()
    {
        if(modelPlayer == null)
            modelPlayer = (GetModelPlayer)target;
    }
}