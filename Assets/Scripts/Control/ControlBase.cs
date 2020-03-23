using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBase
{
    private float _targetVelocity { get; set; }
    private MonoBehaviour _entity { get; set; }
    private Rigidbody2D _entityRigidBody { get; set; }
    private Vector2 _currentMove { get; set; }

    public ControlBase(MonoBehaviour entity, float targetVelocity)
    {
        _targetVelocity = targetVelocity;
        _entity = entity;
        _entityRigidBody = entity.GetComponent<Rigidbody2D>();
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
        return aVelocityChange / ((aFinalVelocity + aVelocityChange) * Time.deltaTime);
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

    public void RegisterDrive()
    {
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
        _currentMove = acc;
    }

    public void ReleaseDrive()
    {
        Vector2 requiredAcc = _currentMove.normalized * GetRequiredAcceleraton(_targetVelocity, _entityRigidBody.drag);
        _entityRigidBody.AddForce(requiredAcc * _entityRigidBody.mass, ForceMode2D.Force);
        Debug.Log("Velocity: " + _entityRigidBody.velocity.magnitude);
    }
}
