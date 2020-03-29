using Assets.Scripts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Physics.Adapters.GravitationAdapter
{
    public interface IGravitationAdapter : IGenericRegistrator<GameObject>
    {
        void Iterate(GameObject gameObject, float gravityForce);
    }
}
