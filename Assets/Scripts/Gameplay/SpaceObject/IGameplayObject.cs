using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Gameplay.SpaceObject
{
    public interface IGameplayObject
    {
        Guid AllianceGuid { get; set; }

        void MakeDamage(float damage);
    }
}
