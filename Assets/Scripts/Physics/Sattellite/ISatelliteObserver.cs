using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Physics.Sattellite
{
    interface ISatelliteObserver
    {        
        void DeltaDistanceModify(float deltaDistance, int orbitsNumber);

        void Detach();

        //void Update();
    }
}
