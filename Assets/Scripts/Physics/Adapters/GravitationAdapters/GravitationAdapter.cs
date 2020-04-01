using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Assets.Scripts.Physics.Adapters.ForceAdapters;
using System.Collections;
using Assets.Scripts.Helpers;
using System;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Physics.Adapters.GravitationAdapter
{
    //Добавлять небесное тело в родительская иерахию в rigidBody.setParent и проверять через GetParent есть ли родитель, если есть, то не притягивать02-
    public class GravitationAdapter : IGravitationAdapter
    {
        private readonly IForceAdapter _forcePhysics;
        private readonly Dictionary<GameObject, Body> _registeredBodies;
        private readonly Body _parent;

        //2 - очень сильно зависти от скорости, надо будет её в конце или вывести или получить эмпирическим путем
        private readonly Vector2 _inaccuracy = new Vector2(2, 2);

        const int _iterateCheckEntryOfOrbit = 30;
        private const double _relativeMass = 0.75;

        public GravitationAdapter(GameObject parent)
        {
            _forcePhysics = new GravityForceAdapter();
            _registeredBodies = new Dictionary<GameObject, Body>();
            _parent = parent;
        }

        public void Register(GameObject gameObject)
        {
            _registeredBodies.Add(gameObject, gameObject);
        }

        public void Unregister(GameObject gameObject)
        {
            if (!_registeredBodies.ContainsKey(gameObject))
                return;
            try
            {
                gameObject.tag = EnumTags.FreeSpaceBody;
                if (_registeredBodies[gameObject].CoroutineCheckIntoOrbit != null)
                {
                    _parent.RawMonoBehaviour.StopCoroutine(_registeredBodies[gameObject].CoroutineCheckIntoOrbit);
                }
                _registeredBodies.Remove(gameObject.gameObject);
            }
            catch (Exception ex)
            {
                Debug.LogError("Блядские корутины");
            }
        }

        /// <summary>
        /// Uses pointed gravity force
        /// </summary>
        /// <param name="gravityForce"></param>
        public void Iterate(float gravityForce)
        {
            foreach (var celestialBody in _registeredBodies)
            {
                if (celestialBody.Value.BeginCheckIntoOrbit)
                {
                    Pull(celestialBody.Value, _parent, gravityForce);
                    continue;
                }
                else if (celestialBody.Value.Rigidbody2D.tag != EnumTags.Satellite)
                {
                    celestialBody.Value.BeginCheckIntoOrbit = true;
                    celestialBody.Value.CoroutineCheckIntoOrbit =
                        _parent.RawMonoBehaviour.StartCoroutine(CheckEntryIntoOrbit(celestialBody.Value, _parent));
                    Pull(celestialBody.Value, _parent, gravityForce);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="possibleSatellite">Объект который вошел в зону влияния(притяжения)</param>
        /// <param name="parentalObject"></param>
        /// <returns></returns>
        private IEnumerator CheckEntryIntoOrbit(Body possibleSatellite, Body parentalObject)
        {
            Vector2 speed = possibleSatellite.Rigidbody2D.velocity;
            int trueIterateCheckEntry = 0;
            for (int iterate = 0; iterate < _iterateCheckEntryOfOrbit; iterate++)
            {
                if (Mathf.Abs((parentalObject.Rigidbody2D.velocity - speed).x) < _inaccuracy.x &&
                    Mathf.Abs((parentalObject.Rigidbody2D.velocity - speed).y) < _inaccuracy.y)
                {
                    trueIterateCheckEntry++;
                }
                yield return new WaitForSeconds(0.025f);
            }
            if (_iterateCheckEntryOfOrbit * _relativeMass <= trueIterateCheckEntry)
            {
                var parentalBody = parentalObject.RawMonoBehaviour.GetComponent<BodyBehaviourBase>();
                var satelliteOrbiting = possibleSatellite.RawMonoBehaviour.GetComponent<OrbitingBehaviour>();

                // if it was someone's satellite
                satelliteOrbiting.StopCallback?.Invoke();
                satelliteOrbiting.Stop();

                // we should register this satellite inside of bodyBehaviourForgamePlay
                // _registeredBodies.Remove(possibleSatellite.RawMonoBehaviour.gameObject);
                parentalBody.Satellites++;
                satelliteOrbiting.StopCallback = () =>
                {
                    // instead of this one we should unregister planet in bodyBehaviour
                    Unregister(possibleSatellite.RawMonoBehaviour.gameObject);
                };
                satelliteOrbiting.OrbitDegreesPerSec = 60f / parentalBody.Satellites;
                satelliteOrbiting.OrbitDistance =
                    parentalBody.Satellites * 3f;
                satelliteOrbiting.Target =
                    parentalObject.RawMonoBehaviour.transform;
                satelliteOrbiting.Begin();
            }
            possibleSatellite.BeginCheckIntoOrbit = false;

        }
        private void Pull(Body satellite, Rigidbody2D rigidBody, float gravityForce)
        {
            var force = _forcePhysics.PullForce(
                rigidBody,
                satellite.Rigidbody2D.transform.position,
                gravityForce);
            satellite.Rigidbody2D.AddForce(force, ForceMode2D.Force);
        }
    }
}