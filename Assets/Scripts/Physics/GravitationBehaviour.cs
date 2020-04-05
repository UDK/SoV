using Assets.Scripts.Physics.Adapters.GravitationAdapter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Helpers;
using Random = UnityEngine.Random;
using System.Threading;
using Assets.Scripts.Manager.Galaxy;
using System.Threading.Tasks;
using System;

namespace Assets.Scripts.Physics
{
    public class GravitationBehaviour : MonoBehaviour
    {
        private GravitationAdapter _gravitationAdapter;

        private GalaxyBehaviour galaxyParent;

        [SerializeField]
        private float _gravityForce = 0;

        void Awake()
        {
            _gravitationAdapter = new GravitationAdapter(transform.parent.gameObject);
            //StartCoroutine(Iterate());
        }

        private void Start()
        {
            galaxyParent = GetComponentInParent<GalaxyBehaviour>();
        }

        private IEnumerator Iterate()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
            }
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

        void Update()
        {
            galaxyParent.task.Add(Task.Run(() =>
            {
                Action action = () =>
                {
                    _gravitationAdapter.IterateFactoryMethod(1);
                };
            }));
        }

        //private void FixedUpdate()
        //{
        //    _gravitationAdapter?.IterateFactoryMethod(1);
        //}
    }
}
