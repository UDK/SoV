﻿using Assets.Scripts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Physics.Fabric.GravitationFabrics
{
    public interface IGravitationAdapter : IGenericRegistrator<GameObject>
    {
        void IterateFactoryMethod(float gravityForce);
    }
}