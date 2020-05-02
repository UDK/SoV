using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CollisionEffect : MonoBehaviour
{
    [SerializeField]
    public VisualEffect visualEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 normal = collision.contacts[0].normal;
        var angle = Vector3.SignedAngle(-normal, Vector3.right, -Vector3.forward);
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        var ripples = Instantiate(
            visualEffect,
            new Vector3(transform.position.x, transform.position.y, visualEffect.transform.position.z),
            rotation,
            transform);

        Destroy(ripples.gameObject, 5);
    }
}
