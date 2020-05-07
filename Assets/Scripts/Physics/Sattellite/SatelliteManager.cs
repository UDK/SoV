using Assets.Scripts.Gameplay;
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
    class SatelliteManager : MonoBehaviour
    {
        public float DeltaOrbitsDistance = 3f;

        /// <summary>
        /// Максимальное количество возможных спутников, изменять в Upgrade/DownGrade, общего класса SpaceObject
        /// </summary>
        public int MaxCountSattelites = 0;

        /// <summary>
        /// Это для тестов, надо будет удалить
        /// </summary>
        [SerializeField]
        List<MonoBehaviour> qqq = new List<MonoBehaviour>();

        List<SatellitesContainer> satelliteObservers = new List<SatellitesContainer>();

        public float LastRadius =>
            DeltaOrbitsDistance * (satelliteObservers.Count + 1);

        public void AttachSatellite(MovementBehaviour sattelite, MovementBehaviour parent)
        {
            Satellite satelliteBehaviour = sattelite.GetComponent<Satellite>();
            satelliteBehaviour.StartOrbiting(parent);
            satelliteObservers.Add(
                new SatellitesContainer
                {
                    satelliteObserver = satelliteBehaviour,
                    satelliteBody = parent.GetComponent<SpaceBody>(),
                });
            satelliteBehaviour.DeltaDistanceModify(DeltaOrbitsDistance, satelliteObservers.Count);
            sattelite.gameObject.layer = LayerHelper.Satellite;

            //Чисто для дебага делаем спутник красным
            sattelite.GetComponent<MeshRenderer>().material.color = Color.red;
            qqq.Add(sattelite);
            //
        }

        public bool IsMaxSatCountReached()
        {
            if (satelliteObservers.Count < MaxCountSattelites)
                return false;
            return true;
        }

        void DetachSattelite()
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
