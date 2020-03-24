using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Наследуемся от этого класса, чтобы получить гравитацию(Нифига не наследуем, надо с Владяном обсудить архитектуру)
/// </summary>
public class AbstractGravity : MonoBehaviour
{

    [SerializeField]
    private float _targetVelocity;
    private MonoBehaviour _entity { get; set; }
    private Rigidbody2D _entityRigidBody { get; set; }
    private Vector2 _currentMove { get; set; } = Vector2.zero;

    private List<Rigidbody2D> gameObjectsOfPulling { get; set; }
    /// <summary>
    /// Очень сильно похоже на _targetVelocity
    /// </summary>
    protected float ForceOfGravitation { get; set; }
    protected CircleCollider2D ColliderOfTrigger { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ggg Ebat");
        this.gameObjectsOfPulling.Add(collision.GetComponent<Rigidbody2D>());
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        this.gameObjectsOfPulling.Remove(collision.GetComponent<Rigidbody2D>());
    }

    private void Prit(Rigidbody2D fromCollider)
    {
        Vector2 distanceBetween = _entityRigidBody.position - fromCollider.position;
        Vector2 requiredAcc = distanceBetween.normalized * GetAcceleraton(_targetVelocity, fromCollider.velocity.magnitude);
        fromCollider.AddForce(requiredAcc * fromCollider.mass, ForceMode2D.Force);
    }

    private float GetRequiredVelocity(float aFinalSpeed, float currentSpeed)
    {
        float m = Mathf.Clamp01(Time.fixedDeltaTime);
        return (aFinalSpeed - currentSpeed) * m / (1 - m);
    }
    private float GetAcceleraton(float aFinalSpeed, float currentSpeed)
    {
        return GetRequiredVelocity(aFinalSpeed, currentSpeed) / Time.fixedDeltaTime;
    }

    void Start()
    {
        gameObjectsOfPulling = new List<Rigidbody2D>();
        _entityRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(this.gameObjectsOfPulling.Count> 0)
        {
            foreach(var sputnik in this.gameObjectsOfPulling)
            {
                Prit(sputnik);
            }
        }
    }

}
