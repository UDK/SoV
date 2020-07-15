using Assets.Scripts.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Assets.Scripts.Manager.GameManager;

namespace Assets.Scripts.Manager.ClassSystem
{
    public interface IUpgradeManager
    {

        void Upgrade(SpaceBody spaceBody, Mapping spaceClass);

        SpaceBody GetFullObject(SpaceClasses spaceClass);
    }
}
