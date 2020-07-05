using Assets.Scripts.Helpers;
using Assets.Scripts.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.AI
{
    public class ShipAIBase : MonoBehaviour
    {
        // for debug
        public float heightMultiplier;
        public float visionAngle = 45;
        public int rayCount = 3;

        public float sightDist;

        public GameObject _target;
        public GameObject[] _targets;

        private MovementBehaviour _movementBehaviour;
        private bool turnBack = false;

        private void Start()
        {
            _movementBehaviour = GetComponent<MovementBehaviour>();

            heightMultiplier = 1.36f;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            //RayCasts();
            if(_target == null)
            {
                return;
            }

            if (turnBack)
            {

                return;
            }

            var heading = _target.transform.position - transform.position;
            var dot = Vector2.Dot(heading, transform.forward);
            _movementBehaviour.SmoothlySetVelocity(heading);
            float angle = Mathf.Atan2(_movementBehaviour.Velocity.y,
                _movementBehaviour.Velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            /*if(dot < 0)
            {
                turnBack = true;
            }
            else
            {
                
            }*/
        }

        private void RayCasts()
        {

            Ray enemyRay = new Ray(transform.position, transform.forward * sightDist);
            //Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward * sightDist, rayColor);
            //Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right).normalized * sightDist, rayColor);
            //Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right).normalized * sightDist, rayColor);

            float halfAngle = visionAngle / rayCount * (rayCount * 0.5f);
            for (int i = 0; i < rayCount; i++)
            {
                float angle = i * (visionAngle / (rayCount - 1));
                Vector3 direction = Quaternion.AngleAxis(transform.localRotation.eulerAngles.z + angle - halfAngle, transform.forward) * Vector3.right;

                Debug.DrawRay(transform.position, direction * sightDist, Color.red, 0.1f);
                var filter = new ContactFilter2D();
                filter.NoFilter();
                RaycastHit2D[] raycastHits = new RaycastHit2D[10];
                var rayHit = Physics2D.Raycast(transform.position, direction, filter, raycastHits, sightDist);
                //_target = rayHit.collider.gameObject;
                _targets = raycastHits.Select(x => x.collider?.gameObject).ToArray();
                //if (rayHit.collider.gameObject.tag == "Player")
                //{
                //    agent.SetDestination(him.transform.position);
                //    _target = rayHit.collider.gameObject;
                //}
            }
        }
    }
}
