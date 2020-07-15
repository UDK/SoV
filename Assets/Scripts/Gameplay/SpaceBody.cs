using Assets.Scripts.Gameplay.SpaceObject;
using Assets.Scripts.Helpers;
using Assets.Scripts.Manager.ClassSystem;
using Assets.Scripts.Physics;
using Assets.Scripts.Physics.Sattellite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;
using static Assets.Scripts.Manager.GameManager;
using System;

namespace Assets.Scripts.Gameplay
{
    /// <summary>
    /// Игровой объект
    /// </summary>
    public class SpaceBody : MonoBehaviour, ISatelliteBody, IGameplayObject
    {
        public delegate void ChangeMass(int mass);
        /// <summary>
        /// Евент изменения массы
        /// </summary>
        public event ChangeMass NotifyChangeMass;

        private float mass;

        public IUpgradeManager upgradeManager;

        public Mapping mappingUpgradeSpaceObject;

        public SpaceClasses SpaceClass { get; set; }

        public Guid AllianceGuid { get; set; }

        public VisualEffect DestroyEffect;

        public bool JustEat = false;

        [SerializeField]
        private float _DamagePercent = 0.5f;

        private SatelliteManager _satelliteManager;

        private MovementBehaviour _movementBehaviour;

        [SerializeField]
        public float Mass
        {
            get
            {
                return mass;
            }
            set
            {
                mass = value;
                upgradeManager.Upgrade(this, mappingUpgradeSpaceObject);
                NotifyChangeMass?.Invoke(Convert.ToInt32(value));
            }
        }

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

        public void MakeDamage(float healtDamage)
        {
            Mass -= healtDamage;
            if (Mass <= 0)
            {
                Destroy();
            }
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