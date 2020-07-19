﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.AI.Weapons
{
    public interface IWeapon
    {
        void Attack(
            GameObject target,
            Guid allianceGuid);
    }
}