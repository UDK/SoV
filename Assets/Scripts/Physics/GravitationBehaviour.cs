using Assets.Scripts.Physics.Adapters.GravitationAdapter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Helpers;


namespace Assets.Scripts.Physics
{
    public class GravitationBehaviour : MonoBehaviour
    {

        private GravitationAdapter _gravitationAdapter;

        [SerializeField]
        private float _gravityForce = 0;

        [SerializeField]
        private float _mass;

        void Awake()
        {
            _gravitationAdapter = new GravitationAdapter(transform.parent.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(EnumTags.FreeSpaceBody))
            {
                _gravitationAdapter.Register(collision.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(EnumTags.FreeSpaceBody))
            {
                _gravitationAdapter.UnRegister(collision.gameObject);
            }
        }

        void FixedUpdate()
        {
            _gravitationAdapter.Iterate(_gravityForce);
        }


    }
}
