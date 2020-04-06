using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Assets.Scripts.Physics.Fabric.ForceFabrics;
using System.Collections;
using Assets.Scripts.Helpers;
using System;
using Random = UnityEngine.Random;
using System.Linq;

namespace Assets.Scripts.Physics.Fabric.GravitationFabrics
{
    //Добавлять небесное тело в родительская иерахию в rigidBody.setParent и проверять через GetParent есть ли родитель, если есть, то не притягивать02-
    public class GravitationAdapter : IGravitationAdapter
    {
        private readonly IForce _forcePhysics;
        private readonly List<IntSatData> _registeredBodies;
        private readonly IntSatData _parent;



        //2 - очень сильно зависти от скорости, надо будет её в конце или вывести или получить эмпирическим путем
        private readonly Vector2 _inaccuracy = new Vector2(2, 2);

        private const int _iterateCheckEntryOfOrbit = 30;
        private const double _boundPossibility = 0.75;

        public GravitationAdapter(GameObject parent)
        {
            _forcePhysics = new GravityForce();
            _registeredBodies = new List<IntSatData>();
            _parent = parent;
        }

        public void Register(GameObject gameObject)
        {
            _registeredBodies.Add(gameObject);
        }

        public void Unregister(GameObject gameObject)
        {
            if (!_registeredBodies.Any(x => x.RawMonoBehaviour.gameObject == gameObject))
                return;

            gameObject.tag = EnumTags.FreeSpaceBody;
            _registeredBodies.RemoveAll(x => x.RawMonoBehaviour.gameObject == gameObject);
        }

        /// <summary>
        /// Uses pointed gravity force
        /// </summary>
        /// <param name="gravityForce"></param>
        public void IterateFactoryMethod(float gravityForce)
        {
            for (int i = 0; i < _registeredBodies.Count; i++)
            {
                if(CheckEntryIntoOrbit(_registeredBodies[i], _parent))
                {
                    i--;
                    continue;
                }
                Pull(_registeredBodies[i], _parent, gravityForce);
                /*if (celestialBody.Value.TempI == 0)
                {
                    Pull(celestialBody.Value, _parent, MovementBehaviour, gravityForce);
                    continue;
                }
                else if (celestialBody.Value.MovementBehaviour.tag != EnumTags.Satellite)
                {
                    CheckEntryIntoOrbit(celestialBody.Value, _parent);
                    Pull(celestialBody.Value, _parent, MovementBehaviour, gravityForce);
                }*/
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="possibleSatellite">Объект который вошел в зону влияния(притяжения)</param>
        /// <param name="parentalObject"></param>
        /// <returns></returns>
        private bool CheckEntryIntoOrbit(
            IntSatData possibleSatellite,
            IntSatData parentalObject)
        {
            Vector3 speed = possibleSatellite.MovementBehaviour.Velocity;
            if (Mathf.Abs((parentalObject.MovementBehaviour.Velocity - speed).x) < _inaccuracy.x &&
                Mathf.Abs((parentalObject.MovementBehaviour.Velocity - speed).y) < _inaccuracy.y)
            {
                possibleSatellite.HitsBeforeOrbit++;
            }

            possibleSatellite.TempI++;
            if (possibleSatellite.TempI < _iterateCheckEntryOfOrbit)
            {
                return false;
            }

            if (_iterateCheckEntryOfOrbit * _boundPossibility <= possibleSatellite.HitsBeforeOrbit)
            {
                Unregister(possibleSatellite.RawMonoBehaviour.gameObject);
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
                    //Unregister(possibleSatellite.RawMonoBehaviour.gameObject);
                };
                satelliteOrbiting.OrbitDegreesPerSec = 60f / parentalBody.Satellites;
                satelliteOrbiting.OrbitDistance =
                    parentalBody.Satellites * 3f;
                satelliteOrbiting.Target =
                    parentalObject.RawMonoBehaviour.transform;
                satelliteOrbiting.Begin();
                possibleSatellite.TempI = 0;
                possibleSatellite.HitsBeforeOrbit = 0;
                return true;
            }

            possibleSatellite.TempI = 0;
            possibleSatellite.HitsBeforeOrbit = 0;
            return false;

        }

        private void Pull(IntSatData satellite, IntSatData parent, float gravityForce)
        {
            var force = _forcePhysics.PullForceFabricMethod(
                parent,
                satellite.MovementBehaviour.transform.position,
                gravityForce);
            satellite.MovementBehaviour.SmoothlySetVelocity(force);

        }
    }
}