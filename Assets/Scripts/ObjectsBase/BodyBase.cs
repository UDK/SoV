﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyBase : MonoBehaviour
{
    public float TargetVelocity;
    public ControlBase ControlBase { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        ControlBase = new ControlBase(this, TargetVelocity);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ControlBase?.ReleaseDrive();
    }

    // Update is called once per frame
    void Update()
    {
        ControlBase?.RegisterDrive();
    }
}
