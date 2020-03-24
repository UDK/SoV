using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Наследуемся от этого класса, чтобы получить гравитацию(Нифига не наследуем, надо с Владяном обсудить архитектуру)
/// </summary>
public class AbstractGravity : MonoBehaviour
{
    private BodyBase _bodyBase { get; set; }
    private readonly Dictionary<GameObject, Satellite> _satellties =
        new Dictionary<GameObject, Satellite>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!this._satellties.ContainsKey(collision.gameObject))
            this._satellties.Add(collision.gameObject, collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        this._satellties.Remove(collision.gameObject);
    }

    private void Pull(Satellite satellite)
    {
        var force = PhysicHelpers.GetGravitationPullForce(_bodyBase, satellite, satellite.BodyBase.GravityForce);
        satellite.Rigidbody2D.AddForce(force, ForceMode2D.Force);
    }

    void Start()
    {
        _bodyBase = GetComponent<BodyBase>();
    }

    void Update()
    {
        if(_satellties.Count> 0)
        {
            foreach(var satellite in _satellties)
            {
                Pull(satellite.Value);
            }
        }
    }
    
    private class Satellite
    {
        public Rigidbody2D Rigidbody2D { get; set; }

        public BodyBase BodyBase { get; set; }

        public Collider2D Collider2D { get; set; }

        public static implicit operator Rigidbody2D(Satellite satellite)
        {
            return satellite.Rigidbody2D;
        }

        public static implicit operator BodyBase(Satellite satellite)
        {
            return satellite.BodyBase;
        }

        public static implicit operator Collider2D(Satellite satellite)
        {
            return satellite.Collider2D;
        }

        public static implicit operator Satellite(Collider2D collider2D)
        {
            return new Satellite
            {
                Rigidbody2D = collider2D.GetComponent<Rigidbody2D>(),
                BodyBase = collider2D.gameObject.GetComponent<BodyBase>(),
                Collider2D = collider2D,
            };
        }
    }
}
