using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Assets.Scripts.Physics.Adapters.ForceAdapters;
using System.Collections;
using Assets.Scripts.Helpers;

namespace Assets.Scripts.Physics.Adapters.GravitationAdapter
{
    //Добавлять небесное тело в родительская иерахию в rigidBody.setParent и проверять через GetParent есть ли родитель, если есть, то не притягивать02-
    public class GravitationAdapter : IGravitationAdapter
    {
        private readonly IForceAdapter _forcePhysics;
        private readonly List<Body> _registeredBodies;

        //2 - очень сильно зависти от скорости, надо будет её в конце или вывести или получить эмпирическим путем
       private readonly Vector2 _inaccuracy = new Vector2(2, 2);

        const int _iterateCheckEntryOfOrbit = 30;
        private const double _relativeMass = 0.75;

        public GravitationAdapter()
        {
            _forcePhysics = new GravityForceAdapter();
            _registeredBodies = new List<Body>();
        }
        
        public void Register(GameObject gameObject)
        {
            this._registeredBodies.Add(gameObject);
        }

        public void UnRegister(GameObject collision)
        {
            this._registeredBodies.Remove(collision.gameObject);
        }

        /// <summary>
        /// Uses pointed gravity force
        /// </summary>
        /// <param name="gameObject"></param>
        public void Iterate(GameObject gameObject, float gravityForce)
        {
            Rigidbody2D rigidbodyOfGameObject = gameObject.GetComponentInParent<Rigidbody2D>();
            MonoBehaviour monoBehaviour = gameObject.GetComponent<MonoBehaviour>();
            foreach (var celestialBodies in _registeredBodies)
            {
                if (!celestialBodies.BeginCheckIntoOrbit)
                {
                    celestialBodies.BeginCheckIntoOrbit = true;
                    monoBehaviour.StartCoroutine(CheckEntryIntoOrbit(celestialBodies, rigidbodyOfGameObject));
                }
                if (celestialBodies.TypeCelestialBody == TypeBody.Satellite)
                {
                    MovingCircle(celestialBodies.Rigidbody2D, rigidbodyOfGameObject);
                }
                else if(celestialBodies.Rigidbody2D.tag != EnumTags.Satellite)
                {
                    Pull(celestialBodies, rigidbodyOfGameObject, gravityForce);
                }
            }
        }

        
        private void MovingCircle(Rigidbody2D gameObject, Rigidbody2D relatively )
        {
            gameObject.velocity = relatively.velocity;
            gameObject.transform.RotateAround(relatively.transform.localPosition, Vector3.back, Time.deltaTime * 10);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject">Объект который вошел в зону влияния(притяжения)</param>
        /// <param name="rigidBody"></param>
        /// <returns></returns>
        private IEnumerator CheckEntryIntoOrbit(Body gameObject, Rigidbody2D rigidBody)
        {
            //Second cosmo-speed
            //2*(G*M)/R


            Vector2 speed = gameObject.Rigidbody2D.velocity;
            int trueIterateCheckEntry = 0;
            for (int iterate = 0; iterate < _iterateCheckEntryOfOrbit; iterate++)
            {
                if (Mathf.Abs((rigidBody.velocity - speed).x) < _inaccuracy.x && Mathf.Abs((rigidBody.velocity - speed).y) < _inaccuracy.y)
                {
                    trueIterateCheckEntry++;
                }
                yield return new WaitForSeconds(0.025f);
            }
            if (_iterateCheckEntryOfOrbit * _relativeMass <= trueIterateCheckEntry)
            {
                gameObject.TypeCelestialBody = TypeBody.Satellite;
                gameObject.Rigidbody2D.gameObject.tag = EnumTags.Satellite;
                gameObject.Rigidbody2D.transform.SetParent(rigidBody.transform);
            }
            gameObject.BeginCheckIntoOrbit = false;

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