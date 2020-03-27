using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Assets.Scripts.Physics
{
    public class GravitationPhysics : IForcePhysics, IGameObjectIterator
    {

        private readonly Dictionary<GameObject, Body> _registeredBodies =
           new Dictionary<GameObject, Body>();

        public float gravityForce { get; set; }


        /// <summary>
        /// Gives Vector2 force
        /// </summary>
        /// <param name="who">Object that pulls</param>
        /// <param name="iv">Second object that will be pulled</param>
        /// <param name="influence">Influence parameter that charges end force</param>
        /// <returns>Force for moving by sob</returns>
        public Vector2 PullForce(Rigidbody2D who, Vector2 iv, float influence)
        {
            var distance = Vector2.Distance(who.transform.position, iv);
            float force = influence / Mathf.Pow(distance, 2);
            // Find the Normal direction
            Vector2 normalDirection = ((Vector2)who.transform.position - iv).normalized;

            // calculate the force on the object from the planet
            Vector2 normalForce = normalDirection * force;

            return normalForce;
        }

        public void RegisterGameObject(GameObject gameObject)
        {

            if (!this._registeredBodies.ContainsKey(gameObject.gameObject))
                this._registeredBodies.Add(gameObject.gameObject, gameObject);
        }

        public void UnRegisterGameObject(GameObject collision)
        {
            this._registeredBodies.Remove(collision.gameObject);
        }


        public void Iterate(GameObject gameObject)
        {
            Rigidbody2D rigidbodyOfGameObject = gameObject.GetComponent<Rigidbody2D>();
            foreach (var satellite in _registeredBodies)
            {
                Pull(satellite.Value, rigidbodyOfGameObject);
            }
        }

        public bool CheckEntryIntoOrbit(Vector2 speed, Rigidbody2D rigidBody)
        {
            //Second cosmo-speed
            //2*(G*M)/R

            Vector2 Inaccuracy = new Vector2(2, 2);
            //Debug.Log(this._rigidBody.velocity - speed);
            if (Mathf.Abs((rigidBody.velocity - speed).x) < Inaccuracy.x && Mathf.Abs((rigidBody.velocity - speed).y) < Inaccuracy.y)
                return true;
            return false;

        }
        private void Pull(Body satellite, Rigidbody2D rigidBody)
        {
            var force = this.PullForce(
                rigidBody,
                satellite.Rigidbody2D.transform.position,
                gravityForce);
            satellite.Rigidbody2D.AddForce(force, ForceMode2D.Force);
        }


    }
}