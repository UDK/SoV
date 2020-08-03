using Assets.Scripts.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Physics.Sattellite
{
    public class Satellite : MonoBehaviour, ISatelliteObserver
    {
        /// <summary>
        /// Кто притягивает
        /// </summary>
        private Movement _target = null;
        private Movement _subject = null;

        private Vector3 _rotationVector = Vector3.forward;
        private float _deltaDistance = 3.0f;
        private float _orbitDistance = 3.0f;
        private float _orbitDegreesPerSec = 30.0f;

        private Vector3 _relativeDistance = Vector3.zero;
        private bool _once = true;

        private bool _getToTarget = false;

        private float _minDistance;
        private float _maxDistance;

        public void StartOrbiting(Movement target)
        {
            _target = target;
            _subject = GetComponent<Movement>();
            _getToTarget = true;
        }

        public void Detach()
        {
            _target = null;
        }

        public void DeltaDistanceModify(float deltaDistance, int orbitsNumber)
        {
            _deltaDistance = deltaDistance;
            _orbitDistance = orbitsNumber * _deltaDistance;
            _orbitDegreesPerSec = 60f / orbitsNumber;

            _minDistance = _orbitDistance * 0.98f;
            _maxDistance = _orbitDistance * 1.02f;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!LayerHelper.IsBody(collision.gameObject.layer))
            {
                return;
            }

            if (_target != null)
                StartCoroutine(SlowlySetBackOrbit());
        }

        private IEnumerator SlowlySetBackOrbit()
        {
            var temp = _target;
            _target = null;
            yield return new WaitForSeconds(0.2f);
            _rotationVector *= -1;
            StartOrbiting(temp);
        }

        public void FixedUpdate()
        {
            if(_target == null)
            {
                return;
            }

            if (_getToTarget)
            {
                _subject.SetVelocity(Vector3.zero);
                var newPos = (this.transform.position - _target.transform.position).normalized * _orbitDistance;
                newPos += _target.transform.position;
                transform.position = Vector3.MoveTowards(transform.position, newPos, 0.02f + _target.Magnitude);
                var distance = Vector3.Distance(transform.position, _target.transform.position);
                if (_minDistance < distance && distance < _maxDistance)
                {
                    _relativeDistance = this.transform.position - _target.transform.position;
                    _once = true;
                    _getToTarget = false;
                }
                else
                {
                    return;
                }
            }

            // Keep us at the last known relative position
            this.transform.position = (_target.transform.position + _relativeDistance);
            this.transform.RotateAround(_target.transform.position, _rotationVector, _orbitDegreesPerSec * Time.fixedDeltaTime);
            // Reset relative position after rotate
            if (_once)
            {
                // transform.position *= orbitDistance;
                var newPos = (this.transform.position - _target.transform.position).normalized * _orbitDistance;
                newPos += _target.transform.position;
                this.transform.position = newPos;
                _once = false;
            }
            _relativeDistance = this.transform.position - _target.transform.position;
        }

    }
}
