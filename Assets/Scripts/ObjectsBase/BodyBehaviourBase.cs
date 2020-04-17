using Assets.Scripts.Helpers;
using Assets.Scripts.Physics;
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

    [SerializeField]
    private float _incarancyEating;

    private float _incarancyTotalDestraction;

    private SatelliteManagerBehavior _satelliteManager;
    private MovementBehaviour _movementBehaviour;

    public float Mass => _mass;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (LayerEnums.IsBody(collision.gameObject.layer))
            RegisterDamage(collision);*/
    }

    private void RegisterDamage(Collider2D collision)
    {
        var enemy = collision.GetComponent<BodyBehaviourBase>();
        var mass = Mass - enemy.Mass;
        if (mass > 0)
        {
            var damage = _mass * _incarancyEating;
            if (enemy.MakeDamage(damage))
            {
                _mass += damage;
            }
            else
            {
                MakeDamage(enemy.Mass * 0.3f);
            }
        }
        else if(mass == 0)
        {
            var damage = _mass * 0.25f;
            enemy.MakeDamage(damage);
            if(gameObject.CompareTag(EnumTags.Player))
            {
                damage *= 0.95f;
            }
            MakeDamage(damage);
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
        _satelliteManager = GetComponent<SatelliteManagerBehavior>();
        _movementBehaviour = GetComponent<MovementBehaviour>();
        //_mass = Random.Range(1f, 500f);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
