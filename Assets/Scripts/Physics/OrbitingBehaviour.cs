using Assets.Scripts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    public class OrbitingBehaviour : MonoBehaviour
    {
        public Transform Target;
        public float OrbitDistance = 3.0f;
        public float OrbitDegreesPerSec = 30.0f;
        public Action StopCallback = null;

        private Vector3 _relativeDistance = Vector3.zero;
        private bool _once = true;

        public void Begin()
        {

            if (Target != null)
            {
                tag = EnumTags.Satellite;
                _relativeDistance = transform.position - Target.position;
            }
        }
        public void Stop()
        {
            tag = EnumTags.FreeSpaceBody;
            Target = null;
            _relativeDistance = Vector3.zero;
            _once = true;
            StopCallback = null;
        }

        void Orbit()
        {
            if (Target != null)
            {
                // Keep us at the last known relative position
                transform.position = (Target.position + _relativeDistance);
                transform.RotateAround(Target.position, Vector3.forward, OrbitDegreesPerSec * Time.fixedDeltaTime);
                // Reset relative position after rotate
                if (_once)
                {
                    // transform.position *= orbitDistance;
                    var newPos = (transform.position - Target.position).normalized * OrbitDistance;
                    newPos += Target.position;
                    transform.position = newPos;
                    _once = false;
                }
                _relativeDistance = transform.position - Target.position;
            }
        }

        void FixedUpdate()
        {
            Orbit();
        }
    }
}
