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
    class SatelliteManagerBehavior : MonoBehaviour
    {
        //Удалить и передавать при иницализации
        float _deltaOrbitsDistance = 3f;

        List<ISatelliteObserver> satelliteObservers = new List<ISatelliteObserver>();

        public void AttacheSattelite(MonoBehaviour sattelite, MonoBehaviour parent)
        {
            Satellite satelliteBehaviour = new Satellite(sattelite.transform, parent.transform);
            satelliteObservers.Add(satelliteBehaviour);
            sattelite.tag = EnumTags.Satellite;
            sattelite.gameObject.layer = LayerMask.NameToLayer("sattelite");
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
                satelliteObservers[iter].DeltaDistanceModify(newDeltaDistance,iter);
            }
        }

        private void FixedUpdate()
        {
            foreach(var satellit in satelliteObservers)
            {
                satellit.Update();
            }
        }
    }
}
