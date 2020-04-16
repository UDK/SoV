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
        /// <summary>
        /// Переменная разброса проверки выхода на орбиту, очень сильно зависти от скорости
        /// </summary>
        public float Inaccuracy = 0.3f;
        /// <summary>
        /// Переменная разброса проверки выхода на орбиту, очень сильно зависти от скорости
        /// </summary>
        public float DeltaTimeTillOrbit = 0.5f;

        private IForce _forcePhysics;
        private List<IntSatData> _registeredBodies;
        private MovementBehaviour _parent;
        private SatelliteManagerBehavior _satelliteManagerBehavior;
        /// <summary>
        /// Сколько итераций проверки выхода на орбиту произойдет
        /// </summary>
        private const int _iterateCheckEntryOfOrbit = 3;

        private float _minR = 0;
        private float _maxR = 0;


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
            CalcInflRadius();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(EnumTags.FreeSpaceBody) &&
                (transform.parent.GetComponent<BodyBehaviourBase>().Mass > collision.GetComponent<BodyBehaviourBase>().Mass))
            {
                IntSatData intSatData = collision.gameObject;
                _registeredBodies.Add(intSatData);
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
                bool satelliteReady = false;
                if (_registeredBodies[i].Collider2D.tag == EnumTags.Satellite ||
                    (satelliteReady = CheckEntryIntoOrbit(_registeredBodies[i], _parent)))
                {
                    _registeredBodies.RemoveAt(i);
                    i--;
                    if (satelliteReady)
                    {
                        RefreshPossibleSatellites();
                    }
                    continue;
                }

                Pull(_registeredBodies[i], _parent, _gravityForce);
            }
        }

        private void RefreshPossibleSatellites()
        {
            for (int j = 0; j < _registeredBodies.Count; j++)
            {
                _registeredBodies[j].TimeLeftTillNextCheck = 0;
                _registeredBodies[j].HitsBeforeOrbit = 0;
                _registeredBodies[j].TempI = 0;
            }
        }

        private void CalcInflRadius()
        {
            var r = _satelliteManagerBehavior.LastRadius;
            _minR = r - r * Inaccuracy;
            _maxR = r + r * Inaccuracy;
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
            possibleSatellite.TimeLeftTillNextCheck -= Time.fixedDeltaTime;
            if (possibleSatellite.TimeLeftTillNextCheck > 0)
            {
                return false;
            }
            possibleSatellite.TimeLeftTillNextCheck = DeltaTimeTillOrbit;

            float distanceBeetwenPoints = Vector3.Distance(
                parentalObject.transform.position,
                possibleSatellite.MovementBehaviour.transform.position);
            //Debug.Log($"{_minR} < {distanceBeetwenPoints} < {_maxR}");

            if (!(_minR < distanceBeetwenPoints &&
                _maxR > distanceBeetwenPoints))
            {
                possibleSatellite.TimeLeftTillNextCheck = 0;
                possibleSatellite.HitsBeforeOrbit = 0;
                possibleSatellite.TempI = 0;
                return false;
            }
            possibleSatellite.TempI++;
            //Debug.Log("Hit: " + possibleSatellite.TempI);
            //Ждем пока объект попадет под влияние положеннное количество раз
            if (possibleSatellite.TempI < _iterateCheckEntryOfOrbit)
            {
                return false;
            }

            //если опрделенное количество проверок пройдено, то делаем его спутником
            _satelliteManagerBehavior.AttacheSattelite(possibleSatellite, parentalObject);
            CalcInflRadius();
            return true;

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
