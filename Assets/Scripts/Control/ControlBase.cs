using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlBase
{
    private float _targetVelocity { get; set; }
    private MonoBehaviour _entity { get; set; }
    private Rigidbody2D _entityRigidBody { get; set; }
    private Vector2 _currentMove { get; set; } = Vector2.zero;

    public ControlBase(MonoBehaviour entity, float targetVelocity)
    {
        _targetVelocity = targetVelocity;
        _entity = entity;
        _entityRigidBody = entity.GetComponent<Rigidbody2D>();
    }

    public void ReleaseDrive()
    {
        Vector2 requiredAcc = _currentMove.normalized * GetRequiredAcceleraton(_targetVelocity, _entityRigidBody.drag);
        _entityRigidBody.AddForce(requiredAcc * _entityRigidBody.mass, ForceMode2D.Force);
    }

    public void RegisterDrive()
    {
        this._currentMove = Drive();
    }

    protected virtual Vector2 Drive()
    {
        return Vector2.zero;
    }

    private float GetFinalVelocity(float aVelocityChange, float aDrag)
    {
        return aVelocityChange * (1 / Mathf.Clamp01(aDrag * Time.fixedDeltaTime) - 1);
    }
    private float GetFinalVelocityFromAcceleration(float aAcceleration, float aDrag)
    {
        return GetFinalVelocity(aAcceleration * Time.fixedDeltaTime, aDrag);
    }


    private float GetDrag(float aVelocityChange, float aFinalVelocity)
    {
        return aVelocityChange / ((aFinalVelocity + aVelocityChange) * Time.deltaTime);
    }
    private float GetDragFromAcceleration(float aAcceleration, float aFinalVelocity)
    {
        return GetDrag(aAcceleration * Time.fixedDeltaTime, aFinalVelocity);
    }


    private float GetRequiredVelocityChange(float aFinalSpeed, float aDrag)
    {
        float m = Mathf.Clamp01(aDrag * Time.fixedDeltaTime);
        return aFinalSpeed * m / (1 - m);
    }
    private float GetRequiredAcceleraton(float aFinalSpeed, float aDrag)
    {
        return GetRequiredVelocityChange(aFinalSpeed, aDrag) / Time.fixedDeltaTime;
    }
}
