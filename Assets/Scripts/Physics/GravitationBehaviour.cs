using Assets.Scripts.Physics.Adapters.GravitationAdapter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Helpers;
using Random = UnityEngine.Random;
using System.Threading.Tasks;

namespace Assets.Scripts.Physics
{
    public class GravitationBehaviour : MonoBehaviour
    {
        private GravitationAdapter _gravitationAdapter;

        [SerializeField]
        private float _gravityForce = 0;

        void Awake()
        {
            _gravitationAdapter = new GravitationAdapter(transform.parent.gameObject);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(EnumTags.FreeSpaceBody) &&
                transform.parent.GetComponent<BodyBehaviourBase>().Mass > collision.GetComponent<BodyBehaviourBase>().Mass)
            {
                _gravitationAdapter.Register(collision.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(EnumTags.FreeSpaceBody))
            {
                _gravitationAdapter.Unregister(collision.gameObject);
            }
        }

        void FixedUpdate()
        {
            _gravitationAdapter?.IterateFactoryMethod(_gravityForce);
        }
    }
}
