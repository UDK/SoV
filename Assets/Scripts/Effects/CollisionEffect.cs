using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CollisionEffect : MonoBehaviour
{
    public VisualEffect collisionEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 normal = collision.contacts[0].normal;
        var angle = Vector3.SignedAngle(-normal, Vector3.right, -Vector3.forward);
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        var position = new Vector3(transform.position.x, transform.position.y, collisionEffect.transform.position.z);
        var collisionE = Instantiate(
            collisionEffect,
            position,
            rotation,
            transform);
        /*var collisionAfterE = Instantiate(
            collisionAfterEffect,
            position,
            rotation,
            transform.parent);*/
        Destroy(collisionE.gameObject, 5);
        /*Destroy(collisionAfterE.gameObject, 5);*/
    }
}
