using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    /// <summary>
    /// Должны реализовывать все космические объекты
    /// </summary>
    public interface IGameObjectIterator
    {

        /// <summary>
        /// Вся 
        /// </summary>
        void Iterate(GameObject gameObject);

        void UnRegisterGameObject(GameObject collision);

        void RegisterGameObject(GameObject gameObject);

    }
}
