using Assets.Scripts.Physics.Sattellite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Игровой объект
/// </summary>
public class BodyBehaviourBase : MonoBehaviour, ISatelliteBody
{
    [SerializeField]
    private float _mass;

    private float _incarancyEating;

    private float _incarancyTotalDestraction;

    SatelliteManagerBehavior satelliteManager;

    public float Mass => _mass;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("planetMagnet"))
            RegisterDamage(collision);
    }

    private void RegisterDamage(Collider2D collision)
    {
        var gameObject = collision.GetComponent<BodyBehaviourBase>();
        var mass = Mass - gameObject.Mass;
        if (mass > 0)
        {
            var damage = _mass * _incarancyEating;
            if (gameObject.MakeDamage(damage))
            {
                _mass += damage;
            }
            else
            {
                MakeDamage(gameObject.Mass * 0.3f);
            }
            //if(gameObject.Mass * _incarancyEating < _mass)
            //{
            //    _mass += gameObject.Mass;
            //    gameObject.Destroy();
            //}
            //else if(gameObject.Mass * _incarancyTotalDestraction < _mass)
            //{
            //    _mass -= gameObject.Mass;
            //    gameObject.Destroy();
            //}
            //else
            //{

            //    _mass -= gameObject.Mass;
            //}
        }
    }

    public bool MakeDamage(float healtDamage)
    {
        _mass -= healtDamage;
        if (_mass <= 0)
        {
            Destroy();
            return true;
        }
        return false;
    }

    private void Awake()
    {
        satelliteManager = GetComponent<SatelliteManagerBehavior>();
        _mass = Random.Range(1f, 500f);
    }

    public void Destroy()
    {
        this.Destroy();
    }
}
