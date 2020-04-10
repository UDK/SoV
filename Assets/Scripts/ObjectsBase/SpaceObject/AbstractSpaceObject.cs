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

        /// <summary>
        /// Во что игровой объект превратится при апгрейде
        /// </summary>
        public abstract void Upgrade();

        /// <summary>
        /// Во что игровой объект превратится при довнгрейде
        /// </summary>
        public abstract void Downgrade();



    }
}
