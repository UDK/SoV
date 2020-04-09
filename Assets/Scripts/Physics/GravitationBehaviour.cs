using Assets.Scripts.Physics.Fabric.GravitationFabrics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Helpers;
using Random = UnityEngine.Random;
using System.Threading.Tasks;
using Assets.Scripts.Physics.Fabric.ForceFabrics;
using System.Linq;

namespace Assets.Scripts.Physics
{
    public class GravitationBehaviour : MonoBehaviour
    {
        private IForce _forcePhysics;
        private List<IntSatData> _registeredBodies;
        private IntSatData _parent;

        //2 - очень сильно зависти от скорости, надо будет её в конце или вывести или получить эмпирическим путем
        private readonly Vector2 _inaccuracy = new Vector2(2, 2);
        private const int _iterateCheckEntryOfOrbit = 30;
        private const double _boundPossibility = 0.75;


        [SerializeField]
        private float _gravityForce = 0;

        void Awake()
        {
            _forcePhysics = new GravityForce();
            _registeredBodies = new List<IntSatData>();
            _parent = transform.parent.gameObject;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(EnumTags.FreeSpaceBody) &&
                transform.parent.GetComponent<BodyBehaviourBase>().Mass > collision.GetComponent<BodyBehaviourBase>().Mass)
            {
                _registeredBodies.Add(collision.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(EnumTags.FreeSpaceBody))
            {
                if (!_registeredBodies.Any(x => x.RawMonoBehaviour.gameObject == collision.gameObject))
                    return;

                collision.tag = EnumTags.FreeSpaceBody;
                _registeredBodies.RemoveAll(x => x.RawMonoBehaviour.gameObject == collision.gameObject);
            }
        }

        void FixedUpdate()
        {
            for (int i = 0; i < _registeredBodies.Count; i++)
            {
                if (_registeredBodies[i].RawMonoBehaviour.tag == EnumTags.Satellite
                    || CheckEntryIntoOrbit(_registeredBodies[i], _parent))
                {
                    _registeredBodies.RemoveAt(i);
                    i--;
                    continue;
                }
                Pull(_registeredBodies[i], _parent, _gravityForce);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="possibleSatellite">Объект который вошел в зону влияния(притяжения)</param>
        /// <param name="parentalObject"></param>
        /// <returns></returns>
        private bool CheckEntryIntoOrbit(
            IntSatData possibleSatellite,
            IntSatData parentalObject)
        {
            Vector3 speed = possibleSatellite.MovementBehaviour.Velocity;
            if (Mathf.Abs((parentalObject.MovementBehaviour.Velocity - speed).x) < _inaccuracy.x &&
                Mathf.Abs((parentalObject.MovementBehaviour.Velocity - speed).y) < _inaccuracy.y)
            {
                possibleSatellite.HitsBeforeOrbit++;
            }

            possibleSatellite.TempI++;
            if (possibleSatellite.TempI < _iterateCheckEntryOfOrbit)
            {
                return false;
            }

            if (_iterateCheckEntryOfOrbit * _boundPossibility <= possibleSatellite.HitsBeforeOrbit)
            {
                //OnTriggerExit2D(possibleSatellite);
                var parentalBody = parentalObject.RawMonoBehaviour.GetComponent<BodyBehaviourBase>();
                var satelliteOrbiting = possibleSatellite.RawMonoBehaviour.GetComponent<OrbitingBehaviour>();

                // if it was someone's satellite
                satelliteOrbiting.StopCallback?.Invoke();
                satelliteOrbiting.Stop();

                // we should register this satellite inside of bodyBehaviourForgamePlay
                // _registeredBodies.Remove(possibleSatellite.RawMonoBehaviour.gameObject);
                parentalBody.Satellites++;
                satelliteOrbiting.StopCallback = () =>
                {
                    // instead of this one we should unregister planet in bodyBehaviour
                    //Unregister(possibleSatellite.RawMonoBehaviour.gameObject);
                };
                satelliteOrbiting.OrbitDegreesPerSec = 60f / parentalBody.Satellites;
                satelliteOrbiting.OrbitDistance =
                    parentalBody.Satellites * 3f;
                satelliteOrbiting.Target =
                    parentalObject.RawMonoBehaviour.transform;
                satelliteOrbiting.Begin();
                possibleSatellite.TempI = 0;
                possibleSatellite.HitsBeforeOrbit = 0;
                return true;
            }

            possibleSatellite.TempI = 0;
            possibleSatellite.HitsBeforeOrbit = 0;
            return false;

        }

        private void Pull(IntSatData satellite, IntSatData parent, float gravityForce)
        {
            var force = _forcePhysics.PullForceFabricMethod(
                parent,
                satellite.MovementBehaviour.transform.position,
                gravityForce);
            satellite.MovementBehaviour.SmoothlySetVelocity(force);

        }
    }
}
