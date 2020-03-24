using UnityEngine;
using UnityEditor;

public class PlayerControl : ControlBase
{

    public PlayerControl(MonoBehaviour entity, float targetVelocity)
        : base(entity, targetVelocity)
    {
    }

    protected override Actions Drive()
    {
        Actions actions = Actions.EmptyInstance;
        if (Input.GetKey(KeyCode.W))
        {
            actions.Move |= Move.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            actions.Move |= Move.Down;
        }

        if (Input.GetKey(KeyCode.D))
        {
            actions.Move |= Move.Rigth;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            actions.Move |= Move.Left;
        }

        return actions;
    }
}