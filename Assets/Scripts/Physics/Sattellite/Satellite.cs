using Assets.Scripts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Physics.Sattellite
{
    public class Satellite : ISatelliteObserver
    {
        /// <summary>
        /// Кто притягивает
        /// </summary>
        private readonly Transform _target;
        /// <summary>
        /// Кто притягивается
        /// </summary>
        private readonly Transform _subject;

        private float _deltaDistance = 3.0f;
        private float _orbitDistance = 3.0f;
        private float _orbitDegreesPerSec = 30.0f;

        private Vector3 _relativeDistance = Vector3.zero;
        private bool _once = true;

        public Satellite(Transform subject, Transform target)
        {
            _target = target;
            _subject = subject;
            _relativeDistance = _subject.position - _target.position;
        }

        public void DeltaDistanceModify(float deltaDistance, int orbitsNumber)
        {
            _deltaDistance = deltaDistance;
            _orbitDistance = orbitsNumber * _deltaDistance;
            _orbitDegreesPerSec = 60f / orbitsNumber;
        }

        public void Update()
        {
            // Keep us at the last known relative position
            _subject.transform.position = (_target.position + _relativeDistance);
            _subject.transform.RotateAround(_target.position, Vector3.forward, _orbitDegreesPerSec * Time.fixedDeltaTime);
            // Reset relative position after rotate
            if (_once)
            {
                // transform.position *= orbitDistance;
                var newPos = (_subject.transform.position - _target.position).normalized * _orbitDistance;
                newPos += _target.position;
                _subject.transform.position = newPos;
                _once = false;
            }
            _relativeDistance = _subject.transform.position - _target.position;
        }

    }
}
