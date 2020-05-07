using Assets.Scripts.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Manager.ClassSystem
{
    public interface IUpgradeManager
    {
        void Upgrade(SpaceBody spaceBody);

        SpaceBody GetFullObject(SpaceClasses spaceClass);
    }
}
