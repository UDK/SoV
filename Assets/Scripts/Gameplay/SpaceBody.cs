using Assets.Scripts.Helpers;
using Assets.Scripts.Manager.ClassSystem;
using Assets.Scripts.Physics;
using Assets.Scripts.Physics.Sattellite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Gameplay
{
    /// <summary>
    /// Игровой объект
    /// </summary>
    public class SpaceBody : MonoBehaviour, ISatelliteBody
    {
        public delegate void ChangeMass();
        /// <summary>
        /// Евент изменения массы
        /// </summary>
        public event ChangeMass NotifyChangeMass;

        private float mass;

        public float Mass
        {
            get
            {
                return mass;
            }
            set
            {
                mass = value;
                NotifyChangeMass?.Invoke();
            }
        }

        public SpaceClasses SpaceClass { get; set; }

        public VisualEffect DestroyEffect;

        public bool JustEat = false;

        [SerializeField]
        private float _DamagePercent = 0.5f;

        private SatelliteManager _satelliteManager;

        private MovementBehaviour _movementBehaviour;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (LayerHelper.IsBody(collision.gameObject.layer))
                RegisterDamage(
                    collision.gameObject.GetComponent<SpaceBody>());
        }

        private void RegisterDamage(SpaceBody enemy)
        {
            var mass = Mass - enemy.Mass;
            if (mass >= 0)
            {
                if (JustEat)
                {
                    Mass += enemy.EatMe();
                }
                else
                {
                    var damage = Mass * _DamagePercent;
                    enemy.MakeDamage(damage);
                    damage *= 0.95f;
                    if (gameObject.CompareTag(EnumTags.Player))
                    {
                        damage *= 0.5f;
                    }
                    MakeDamage(damage);
                }
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

        public float EatMe()
        {
            _satelliteManager.DetachSattelites();
            Destroy(gameObject);
            return Mass;
        }

        private void Awake()
        {
            _satelliteManager = GetComponent<SatelliteManager>();
            _movementBehaviour = GetComponent<MovementBehaviour>();
        }

        public void Destroy()
        {
            _satelliteManager.DetachSattelites();
            var position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            var destroyEffect = Instantiate(
                DestroyEffect,
                position,
                Quaternion.identity);
            destroyEffect.SetFloat("Radius", transform.localScale.x);
            Destroy(gameObject);
            Destroy(destroyEffect.gameObject, 5);
        }
    }
}