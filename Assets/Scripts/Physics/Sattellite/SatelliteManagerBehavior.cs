using Assets.Scripts.Helpers;
using Assets.Scripts.Physics.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Physics.Sattellite
{
    /// <summary>
    /// Скорее всего перестанет наследоваться от MonoBehaviour????
    /// </summary>
    class SatelliteManagerBehavior : MonoBehaviour
    {
        //Удалить и передавать при иницализации
        [SerializeField]
        float _deltaOrbitsDistance = 3f;

        /// <summary>
        /// Максимальное количество возможных спутников, изменять в Upgrade/DownGrade, общего класса SpaceObject
        /// </summary>
        [SerializeField]
        private int maxCountSattelites = 2;

        /// <summary>
        /// Это для тестов, надо будет удалить
        /// </summary>
        [SerializeField]
        List<MonoBehaviour> qqq = new List<MonoBehaviour>();

        List<SatellitesContainer> satelliteObservers = new List<SatellitesContainer>();

        public float LastRadius =>
            _deltaOrbitsDistance * (satelliteObservers.Count + 1);

        public void AttachSatellite(MovementBehaviour sattelite, MovementBehaviour parent)
        {
            Satellite satelliteBehaviour = sattelite.GetComponent<Satellite>();
            satelliteBehaviour.StartOrbiting(parent);
            satelliteObservers.Add(
                new SatellitesContainer
                {
                    satelliteObserver = satelliteBehaviour,
                    satelliteBody = parent.GetComponent<BodyBehaviourBase>(),
                });
            satelliteBehaviour.DeltaDistanceModify(_deltaOrbitsDistance, satelliteObservers.Count);
            sattelite.gameObject.layer = LayerEnums.Satellite;
            //Чисто для дебага делаем спутник красным
            sattelite.GetComponent<MeshRenderer>().material.color = Color.red;
            qqq.Add(sattelite);
            //
        }

        public bool IsMaxSatCountReached()
        {
            if (satelliteObservers.Count < maxCountSattelites)
                return false;
            return true;
        }

        void DeAttacheSattelite()
        {

        }

        /// <summary>
        /// Здесь мы в foreach вызываем у каждого Update
        /// </summary>
        public void NotifySetDistance(float newDeltaDistance)
        {
            for (int iter = 0; iter < satelliteObservers.Count; iter++)
            {
                satelliteObservers[iter].satelliteObserver.DeltaDistanceModify(newDeltaDistance, iter);
            }
        }

        private void FixedUpdate()
        {
            /*foreach (var satellit in satelliteObservers)
            {
                satellit.satelliteObserver.Update();
            }*/
        }
    }
}
