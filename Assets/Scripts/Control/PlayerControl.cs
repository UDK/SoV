using UnityEngine;
using UnityEditor;

public class PlayerControl : ControlBase
{

    public PlayerControl(MonoBehaviour entity, float targetVelocity)
        : base(entity, targetVelocity)
    {
    }

    protected override Vector2 Drive()
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

        return acc;
    }
}