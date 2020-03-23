using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBase
{
    public float _targetVelocity { get; set; }

    public ControlBase(float targetVelocity)
    {
        _targetVelocity = targetVelocity;
    }

    float GetFinalVelocity(float aVelocityChange, float aDrag)
    {
        return aVelocityChange * (1 / Mathf.Clamp01(aDrag * Time.fixedDeltaTime) - 1);
    }
    float GetFinalVelocityFromAcceleration(float aAcceleration, float aDrag)
    {
        return GetFinalVelocity(aAcceleration * Time.fixedDeltaTime, aDrag);
    }


    float GetDrag(float aVelocityChange, float aFinalVelocity)
    {
        return aVelocityChange / ((aFinalVelocity + aVelocityChange) * Time.fixedDeltaTime);
    }
    float GetDragFromAcceleration(float aAcceleration, float aFinalVelocity)
    {
        return GetDrag(aAcceleration * Time.fixedDeltaTime, aFinalVelocity);
    }


    float GetRequiredVelocityChange(float aFinalSpeed, float aDrag)
    {
        float m = Mathf.Clamp01(aDrag * Time.fixedDeltaTime);
        return aFinalSpeed * m / (1 - m);
    }
    float GetRequiredAcceleraton(float aFinalSpeed, float aDrag)
    {
        return GetRequiredVelocityChange(aFinalSpeed, aDrag) / Time.fixedDeltaTime;
    }

    public void Drive(MonoBehaviour entity)
    {
        var rigidBody = entity.GetComponent<Rigidbody2D>();
        Vector2 acc = new Vector2();
        if (Input.GetKey(KeyCode.W))
        {
            acc.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            acc.y = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            acc.x = 1;
        }
        else if(Input.GetKey(KeyCode.A))
        {
            acc.x = -1;
        }

        Vector2 requiredAcc = acc.normalized * GetRequiredAcceleraton(_targetVelocity, rigidBody.drag);
        rigidBody.AddForce(requiredAcc * rigidBody.mass, ForceMode2D.Force);
        Debug.Log("Velocity: " + Mathf.Sqrt(rigidBody.velocity.magnitude));
        Debug.Log("Mass: " + rigidBody.mass);
        Debug.Log("Force: " + requiredAcc.magnitude);
    }
}
