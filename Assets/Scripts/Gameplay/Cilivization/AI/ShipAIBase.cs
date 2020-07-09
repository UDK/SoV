using Assets.Scripts.Gameplay.Cilivization.AI.States;
using Assets.Scripts.Gameplay.Cilivization.AI.Weapons;
using Assets.Scripts.Gameplay.SpaceObject;
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
    public class ShipAIBase : MonoBehaviour, IGameplayObject
    {
        // used for visual detection of enemies
        public float visionAngle = 45;
        public int rayCount = 3;
        public float sightDist = 10f;

        public Guid AllianceGuid { get; set; }

        // determines distance for attack
        public float attackDist = 3f;

        public GameObject _target;

        // for debug
        public GameObject[] _targets;

        public GameObject[] Weapons;

        public IWeapon[] _weapons;

        private MovementBehaviour _movementBehaviour;

        // state machine
        private AStateMachine<ShipStates> _sm;

        public ShipAIBase()
        {
        }

        private void Start()
        {
            _weapons = Weapons.Select(x =>
                x.GetComponent<MonoBehaviour>() as IWeapon)
                    .ToArray();
            AllianceGuid = Guid.NewGuid();
            _sm = new AStateMachine<ShipStates>();
            _sm.Publish(ShipStates.Moving, Move)
                .Publish(ShipStates.SearchingOfTarget, SearchTarget)
                .Publish(ShipStates.Attacking, Attack);
            _sm.Push(ShipStates.Moving);
            _movementBehaviour = GetComponent<MovementBehaviour>();
        }

        private void FixedUpdate()
        {
            RotateBySpeed();
            _sm.Update();
        }

        private void RotateBySpeed()
        {
            float angle = Mathf.Atan2(_movementBehaviour.Velocity.y,
                _movementBehaviour.Velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void Move()
        {
            var distance = Vector2.Distance(_target.transform.position, transform.position);
            var heading = _target.transform.position - transform.position;

            if(distance < attackDist)
            {
                _sm.Push(
                    ShipStates.Attacking);
            }
            else
            {
                _movementBehaviour.SmoothlySetVelocity(heading.normalized * _movementBehaviour.MaxVelocity);
            }
        }

        private void Attack()
        {
            foreach(var weapon in _weapons)
            {
                weapon.Attack(_target, AllianceGuid);
            }

            var distance = Vector2.Distance(_target.transform.position, transform.position);
            if (distance > attackDist)
            {
                _sm.Push(
                    ShipStates.Moving);
                return;
            }

            // get current magnitude
            var magnitude = _movementBehaviour.Magnitude;

            // get vector center <- obj
            var gravityVector = _target.transform.position - transform.position;

            // check whether left or right of target
            var left = Vector2.SignedAngle(_movementBehaviour.Velocity, gravityVector) > 0;

            // get new vector which is 90° on gravityDirection 
            // and world Z (since 2D game)
            // normalize so it has magnitude = 1
            var newDirection = Vector3.Cross(gravityVector, Vector3.forward).normalized;

            // invert the newDirection in case user is touching right of movement direction
            if (!left) newDirection *= -1;

            // set new direction but keep speed(previously stored magnitude)
            _movementBehaviour.SmoothlySetVelocity(newDirection * magnitude);
        }

        private void SearchTarget()
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
