using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Assets.Scripts.Physics.Fabric.ForceFabrics;
using System.Collections;
using Assets.Scripts.Helpers;
using System;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Physics.Fabric.GravitationFabrics
{
    //Добавлять небесное тело в родительская иерахию в rigidBody.setParent и проверять через GetParent есть ли родитель, если есть, то не притягивать02-
    public class GravitationAdapter : IGravitationAdapter
    {
        private readonly IForce _forcePhysics;
        private readonly Dictionary<GameObject, Body> _registeredBodies;
        private readonly Body _parent;
        private MovementBehaviour MovementBehaviour { get; set; }



        //2 - очень сильно зависти от скорости, надо будет её в конце или вывести или получить эмпирическим путем
        private readonly Vector2 _inaccuracy = new Vector2(2, 2);

        const int _iterateCheckEntryOfOrbit = 30;
        private const double _relativeMass = 0.75;

        public GravitationAdapter(GameObject parent)
        {
            _forcePhysics = new GravityForce();
            _registeredBodies = new Dictionary<GameObject, Body>();
            _parent = parent;
            MovementBehaviour = parent.GetComponent<MovementBehaviour>();
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
        public void IterateFactoryMethod(float gravityForce)
        {
            foreach (var celestialBody in _registeredBodies)
            {
                if (celestialBody.Value.BeginCheckIntoOrbit)
                {
                    Pull(celestialBody.Value, _parent, MovementBehaviour, gravityForce);
                    continue;
                }
                else if (celestialBody.Value.MovementBehaviour.tag != EnumTags.Satellite)
                {
                    celestialBody.Value.BeginCheckIntoOrbit = true;
                    //CheckEntryIntoOrbit(celestialBody.Value, _parent);
                    Pull(celestialBody.Value, _parent, MovementBehaviour, gravityForce);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="possibleSatellite">Объект который вошел в зону влияния(притяжения)</param>
        /// <param name="parentalObject"></param>
        /// <returns></returns>
        private void CheckEntryIntoOrbit(Body possibleSatellite, Body parentalObject)
        {
            Vector3 speed = possibleSatellite.MovementBehaviour.Velocity;
            int trueIterateCheckEntry = 0;
            for (int iterate = 0; iterate < _iterateCheckEntryOfOrbit; iterate++)
            {
                if (Mathf.Abs((parentalObject.MovementBehaviour.Velocity - speed).x) < _inaccuracy.x &&
                    Mathf.Abs((parentalObject.MovementBehaviour.Velocity - speed).y) < _inaccuracy.y)
                {
                    trueIterateCheckEntry++;
                }
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

        private void Pull(Body satellite, Body rigidBody, MovementBehaviour moveThisobject, float gravityForce)
        {
            var force = _forcePhysics.PullForceFabricMethod(
                rigidBody,
                satellite.MovementBehaviour.transform.position,
                gravityForce);
            moveThisobject.SmoothlySetVelocity(force * -1);

        }
    }
}