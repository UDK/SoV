using Assets.Scripts.Gameplay;
using Assets.Scripts.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UpdateMass : MonoBehaviour
{
    Text textUpdateMass { get; set; }
    void Start()
    {
        textUpdateMass = GetComponent<Text>();
        GameObject playerPlanet = GameObject.FindGameObjectWithTag(EnumTags.Player);
        SpaceBody bodyPlanet = playerPlanet.GetComponent<SpaceBody>();
        bodyPlanet.NotifyChangeMass += BodyPlanet_NotifyChangeMass;
    }

    private void BodyPlanet_NotifyChangeMass(int mass)
    {
        textUpdateMass.text = mass.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
