using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Color))]
public class InvisibleMenu : MonoBehaviour
{
    Text text;
    void Start()
    {
        text = GetComponent<Text>();
        PauseGame.Notify += InvisibleMenu_Notify;
    }
    private void InvisibleMenu_Notify(float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
}