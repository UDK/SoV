using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlBase : MonoBehaviour
{
    public float TargetVelocity;

    private Rigidbody2D _entityRigidBody { get; set; }
    private Actions _currentAction { get; set; } = Actions.EmptyInstance;

    public ControlBase(MonoBehaviour entity, float targetVelocity)
    {
    }

    public void ReleaseDrive()
    {
        ProcessMove();
    }

    public void RegisterDrive()
    {
        this._currentAction = Drive();
    }

    protected virtual Actions Drive()
    {
        return Actions.EmptyInstance;
    }

    private void ProcessMove()
    {
        try
        {
            Vector2 currentMove = Vector2.zero;
            if((_currentAction.Move & Move.Up) != 0)
            {
                currentMove.y = 1;
            }
            if((_currentAction.Move & Move.Down) != 0)
            {
                currentMove.y = -1;
            }
            if((_currentAction.Move & Move.Rigth) != 0)
            {
                currentMove.x = 1;
            }
            if((_currentAction.Move & Move.Left) != 0)
            {
                currentMove.x = -1;
            }



            Vector2 requiredAcc = currentMove.normalized * PhysicHelpers.GetAcceleraton(TargetVelocity, _entityRigidBody.velocity.magnitude);
            _entityRigidBody.AddForce(requiredAcc * _entityRigidBody.mass, ForceMode2D.Force);
        }
        catch
        {
            Debug.Log("Fuck this control. This is again null.");
        }
    }

    // Update is called once per frame
    void Start()
    {
        _currentAction = Actions.EmptyInstance;
        _entityRigidBody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ReleaseDrive();
    }

    // Update is called once per frame
    void Update()
    {
        RegisterDrive();
    }
}
