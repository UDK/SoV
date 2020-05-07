using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Manager.ClassSystem
{
    [Serializable]
    public enum SpaceClasses
    {
        Nothing = -1,
        Asteroid = 0,
        Planet = 1,
        Sun = 2,
        BlackHole = 3,
    }
}
