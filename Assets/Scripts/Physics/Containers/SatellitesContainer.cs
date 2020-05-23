using Assets.Scripts.Physics.Sattellite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Physics.Containers
{
    /// <summary>
    /// Храним интерфейс для работы с движением и геймоджект гемплейного взаимодействия
    /// </summary>
    struct SatellitesContainer
    {
        public ISatelliteBody satelliteBody;

        public ISatelliteObserver satelliteObserver;

        public GameObject gameObject;
    }
}
