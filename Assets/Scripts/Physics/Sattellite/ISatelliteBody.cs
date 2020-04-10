using Assets.Scripts.ObjectsBase.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Physics.Sattellite
{
    public interface ISatelliteBody : IBody
    {
        void Destroy();
    }
}
