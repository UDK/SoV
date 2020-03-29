using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    /// <summary>
    /// Regedit and iterator for generic types
    /// </summary>
    public interface IGenericRegistrator<in T>
    {
        void UnRegister(T collision);

        void Register(T gameObject);

    }
}
