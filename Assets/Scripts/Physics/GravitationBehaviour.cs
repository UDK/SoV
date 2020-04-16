using Assets.Scripts.Physics.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Helpers;
using Random = UnityEngine.Random;
using System.Threading.Tasks;
using Assets.Scripts.Physics.Sattellite;
using Assets.Scripts.Physics.Fabric.ForceFabrics;
using System.Linq;

namespace Assets.Scripts.Physics
{
    public class GravitationBehaviour : MonoBehaviour
    {
        private IForce _forcePhysics;
        private List<IntSatData> _registeredBodies;
        private MovementBehaviour _parent;
        private SatelliteManagerBehavior _satelliteManagerBehavior;

        /// <summary>
        /// Переменная разброса проверки выхода на орбиту, очень сильно зависти от скорости
        /// </summary>
        private float _inaccuracy;
        /// <summary>
        /// Сколько итераций проверки выхода на орбиту произойдет
        /// </summary>
        private const int _iterateCheckEntryOfOrbit = 200;
        private const double _boundPossibility = 0.78;


        [SerializeField]
        private float _gravityForce = 0;

        void Awake()
        {
            _forcePhysics = new GravityForce();
            _registeredBodies = new List<IntSatData>();
            _parent = transform.parent.GetComponent<MovementBehaviour>();
            _satelliteManagerBehavior = GetComponentInParent<SatelliteManagerBehavior>();
        }

        private void Start()
        {
            _inaccuracy = 0.1f;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(EnumTags.FreeSpaceBody) &&
                (transform.parent.GetComponent<BodyBehaviourBase>().Mass > collision.GetComponent<BodyBehaviourBase>().Mass))
            {
                _registeredBodies.Add(collision.gameObject);
                var spaceBody = _registeredBodies.Last();
                spaceBody.LastRadius = Mathf.Sqrt(Mathf.Pow(_parent.transform.position.x - spaceBody.MovementBehaviour.transform.position.x, 2)
                + Mathf.Pow(_parent.transform.position.y - spaceBody.MovementBehaviour.transform.position.y, 2));
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(EnumTags.FreeSpaceBody))
            {
                if (!_registeredBodies.Any(x => x.Collider2D.gameObject == collision.gameObject))
                    return;

                collision.tag = EnumTags.FreeSpaceBody;
                _registeredBodies.RemoveAll(x => x.Collider2D.gameObject == collision.gameObject);
            }
        }

        void FixedUpdate()
        {
            for (int i = 0; i < _registeredBodies.Count; i++)
            {
                if (_registeredBodies[i].Collider2D.tag == EnumTags.Satellite
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
        /// <param name="parentalObject">Объект который тянет</param>
        /// <returns></returns>
        private bool CheckEntryIntoOrbit(
            IntSatData possibleSatellite,
            MovementBehaviour parentalObject)
        {
            float distanceBeetwenPoints = Mathf.Sqrt(Mathf.Pow(parentalObject.transform.position.x - possibleSatellite.MovementBehaviour.transform.position.x, 2)
                + Mathf.Pow(parentalObject.transform.position.y - possibleSatellite.MovementBehaviour.transform.position.y, 2));
            if (Mathf.Abs(distanceBeetwenPoints - possibleSatellite.LastRadius) < _inaccuracy)
            {
                possibleSatellite.HitsBeforeOrbit++;
                Debug.Log(Mathf.Abs(distanceBeetwenPoints - possibleSatellite.LastRadius));
            }
            possibleSatellite.LastRadius = distanceBeetwenPoints;
            possibleSatellite.TempI++;
            //Ждем пока объект попадет под влияние положеннное количество раз
            if (possibleSatellite.TempI < _iterateCheckEntryOfOrbit)
            {
                return false;
            }
            //если опрделенное количество проверок пройдено, то делаем его спутником
            if (_iterateCheckEntryOfOrbit * _boundPossibility <= possibleSatellite.HitsBeforeOrbit)
            {
                _satelliteManagerBehavior.AttacheSattelite(possibleSatellite, parentalObject);
                return true;
            }

            possibleSatellite.TempI = 0;
            possibleSatellite.HitsBeforeOrbit = 0;
            return false;

        }

        private void Pull(IntSatData satellite, MovementBehaviour parent, float gravityForce)
        {
            var force = _forcePhysics.PullForceFabricMethod(
                parent,
                satellite.MovementBehaviour.transform.position,
                gravityForce);
            satellite.MovementBehaviour.SmoothlySetVelocity(force);

        }
    }
}
