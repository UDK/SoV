using Assets.Scripts.Helpers;
using Assets.Scripts.Manager.ClassSystem;
using Assets.Scripts.Physics;
using Assets.Scripts.Physics.Sattellite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Gameplay
{
    /// <summary>
    /// Игровой объект
    /// </summary>
    public class SpaceBody : MonoBehaviour, ISatelliteBody
    {
        public float Mass;

        public SpaceClasses SpaceClass { get; set; }

        [SerializeField]
        private float _incarancyEating;

        private float _incarancyTotalDestraction;

        private SatelliteManager _satelliteManager;

        private MovementBehaviour _movementBehaviour;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            /*if (LayerEnums.IsBody(collision.gameObject.layer))
                RegisterDamage(collision);*/
        }

        private void RegisterDamage(Collider2D collision)
        {
            var enemy = collision.GetComponent<SpaceBody>();
            var mass = Mass - enemy.Mass;
            if (mass > 0)
            {
                var damage = Mass * _incarancyEating;
                if (enemy.MakeDamage(damage))
                {
                    Mass += damage;
                }
                else
                {
                    MakeDamage(enemy.Mass * 0.3f);
                }
            }
            else if (mass == 0)
            {
                var damage = Mass * 0.25f;
                enemy.MakeDamage(damage);
                if (gameObject.CompareTag(EnumTags.Player))
                {
                    damage *= 0.95f;
                }
                MakeDamage(damage);
            }
        }

        public bool MakeDamage(float healtDamage)
        {
            Mass -= healtDamage;
            if (Mass <= 0)
            {
                Destroy();
                return true;
            }
            return false;
        }

        private void Awake()
        {
            _satelliteManager = GetComponent<SatelliteManager>();
            _movementBehaviour = GetComponent<MovementBehaviour>();
            //_mass = Random.Range(1f, 500f);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}