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

        List<SatellitesContainer> satelliteObservers = new List<SatellitesContainer>();

        public float LastRadius =>
            DeltaOrbitsDistance * (satelliteObservers.Count + 1);

        public void AttachSatellite(MovementBehaviour sattelite, MovementBehaviour parent)
        {
            Satellite satelliteBehaviour = sattelite.GetComponent<Satellite>();
            satelliteBehaviour.StartOrbiting(parent);
            var container = new SatellitesContainer
            {
                satelliteObserver = satelliteBehaviour,
                satelliteBody = satelliteBehaviour.GetComponent<SpaceBody>(),
                gameObject = satelliteBehaviour.gameObject,
            };
            satelliteObservers.Add(container);
            satelliteBehaviour.DeltaDistanceModify(DeltaOrbitsDistance, satelliteObservers.Count);
            sattelite.gameObject.layer =
                LayerHelper.ClassSatMap2Layer[container.satelliteBody.SpaceClass];
        }

        public bool IsMaxSatCountReached()
        {
            if (satelliteObservers.Count < MaxCountSattelites)
                return false;
            return true;
        }

        public void DetachSattelites()
        {
            for (int iter = 0; iter < satelliteObservers.Count; iter++)
            {
                RemoveSatellite(iter);
            }
        }

        public void DetachSattelite(GameObject gameObject)
        {
            for (int iter = 0; iter < satelliteObservers.Count; iter++)
            {
                if(satelliteObservers[iter].gameObject == gameObject)
                {
                    RemoveSatellite(iter);
                    break;
                }
            }
            NotifySetDistance(DeltaOrbitsDistance);
        }

        private void RemoveSatellite(int iter)
        {
            satelliteObservers[iter].satelliteObserver.Detach();
            satelliteObservers[iter].gameObject.layer =
                LayerHelper.ClassMap2Layer[satelliteObservers[iter].satelliteBody.SpaceClass];
            satelliteObservers.RemoveAt(iter);
        }

        /// <summary>
        /// Здесь мы в foreach вызываем у каждого Update
        /// </summary>
        public void NotifySetDistance(float newDeltaDistance)
        {
            DeltaOrbitsDistance = newDeltaDistance;
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
