using Assets.Scripts.Physics.Adapters.GravitationAdapter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Helpers;


namespace Assets.Scripts.Physics
{
    public class GravitationBehaviour : MonoBehaviour
    {

        private GravitationAdapter _gravitationAdpter;

        [SerializeField]
        private float _gravityForce;

        [SerializeField]
        private float _mass;

        void Awake()
        {
            _gravitationAdpter = new GravitationAdapter();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (EnumTags.FreeSpaceBody == collision.tag)
            {
                _gravitationAdpter.Register(collision.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (EnumTags.FreeSpaceBody == collision.tag)
            {
                _gravitationAdpter.UnRegister(collision.gameObject);
            }
        }

        void FixedUpdate()
        {
            _gravitationAdpter.Iterate(this.gameObject, _gravityForce);
        }


    }
}
