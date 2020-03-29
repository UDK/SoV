using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Assets.Scripts.Physics.Adapters.ForceAdapters;

namespace Assets.Scripts.Physics.Adapters.GravitationAdapter
{
    public class GravitationAdapter : IGravitationAdapter
    {
        private readonly IForceAdapter _forcePhysics;
        private readonly Dictionary<GameObject, Body> _registeredBodies;

        public GravitationAdapter()
        {
            _forcePhysics = new GravityForceAdapter();
            _registeredBodies = new Dictionary<GameObject, Body>();
        }

        public void Register(GameObject gameObject)
        {
            if (!this._registeredBodies.ContainsKey(gameObject.gameObject))
                this._registeredBodies.Add(gameObject.gameObject, gameObject);
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
            Rigidbody2D rigidbodyOfGameObject = gameObject.GetComponent<Rigidbody2D>();
            foreach (var satellite in _registeredBodies)
            {
                Pull(satellite.Value, rigidbodyOfGameObject, gravityForce);
            }
        }
        private bool CheckEntryIntoOrbit(Vector2 speed, Rigidbody2D rigidBody)
        {
            //Second cosmo-speed
            //2*(G*M)/R

            Vector2 Inaccuracy = new Vector2(2, 2);
            //Debug.Log(this._rigidBody.velocity - speed);
            if (Mathf.Abs((rigidBody.velocity - speed).x) < Inaccuracy.x && Mathf.Abs((rigidBody.velocity - speed).y) < Inaccuracy.y)
                return true;
            return false;

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