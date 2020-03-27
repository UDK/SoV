using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Physics
{
    public class Gravitation : MonoBehaviour
    {

        private GravitationPhysics _gravitation = new GravitationPhysics();

        [SerializeField]
        private float GravityForce;
        public float Mass { get; set; }

        void Start()
        {
            _gravitation.gravityForce = GravityForce;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _gravitation.RegisterGameObject(collision.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _gravitation.UnRegisterGameObject(collision.gameObject);
        }

        void FixedUpdate()
        {
            _gravitation.Iterate(this.gameObject);
        }


    }
}
