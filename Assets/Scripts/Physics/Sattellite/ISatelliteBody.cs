using Assets.Scripts.Manager.ClassSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Physics.Sattellite
{
    public interface ISatelliteBody
    {
        SpaceClasses SpaceClass { get; set; }
        void Destroy();
    }
}
