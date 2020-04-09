using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ObjectsBase.SpaceObject
{
    abstract class AbstractSpaceObject
    {
        /// <summary>
        /// Наличие цивилзации, скорее всего переделается из булл в объект класса
        /// </summary>
        bool isCivilization { get; set; } = false;

        /// <summary>
        /// Ссылки на спутнеики, чтобы их можно было съесть
        /// </summary>
        List<BodyBehaviourBase> bodyBehaviourBasesSattelites = new List<BodyBehaviourBase>();

        /// <summary>
        /// Масса(хп) планеты
        /// </summary>
        uint Mass
        {
            get => Mass;
            set
            {
                if (value < 0)
                {
                    Mass = 0;
                }
                else
                    Mass = value;
            }
        }

        public abstract void Upgrade();

        public abstract void Downgrade();



    }
}
