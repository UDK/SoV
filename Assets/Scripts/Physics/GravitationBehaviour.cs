using Assets.Scripts.Physics.Adapters.GravitationAdapter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Physics
{
    public class GravitationBehaviour : MonoBehaviour
    {

        private GravitationAdapter _gravitationAdapter =
            new GravitationAdapter();

        [SerializeField]
        private float _gravityForce = 0;

        [SerializeField]
        private float _mass { get; set; }

        void Start()
        {
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _gravitationAdapter.Register(collision.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _gravitationAdapter.UnRegister(collision.gameObject);
        }

        void FixedUpdate()
        {
            _gravitationAdapter.Iterate(this.gameObject, _gravityForce);
        }


    }
}
