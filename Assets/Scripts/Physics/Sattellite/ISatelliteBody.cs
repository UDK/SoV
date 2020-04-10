using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Physics.Sattellite
{
    public interface ISatelliteBody
    {
        float Mass { get; }
        void Destroy();
    }
}
