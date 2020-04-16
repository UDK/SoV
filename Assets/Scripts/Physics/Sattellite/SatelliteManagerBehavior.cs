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

        [SerializeField]
        List<MonoBehaviour> qqq = new List<MonoBehaviour>();

        List<SatellitesContainer> satelliteObservers = new List<SatellitesContainer>();

        public float LastRadius =>
            _deltaOrbitsDistance * (satelliteObservers.Count + 1);

        public void AttacheSattelite(MonoBehaviour sattelite, MonoBehaviour parent)
        {
            Satellite satelliteBehaviour = new Satellite(sattelite.transform, parent.transform);
            satelliteObservers.Add(
                new SatellitesContainer
                {
                    satelliteObserver = satelliteBehaviour,
                    satelliteBody = parent.GetComponent<BodyBehaviourBase>(),
                });
            satelliteBehaviour.DeltaDistanceModify(_deltaOrbitsDistance, satelliteObservers.Count);
            sattelite.tag = EnumTags.Satellite;
            sattelite.gameObject.layer = LayerMask.NameToLayer("sattelite");
            //Чисто для дебага делаем спутник красным
            sattelite.GetComponent<MeshRenderer>().material.color = Color.red;
            qqq.Add(sattelite);
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
            foreach (var satellit in satelliteObservers)
            {
                satellit.satelliteObserver.Update();
            }
        }
    }
}
