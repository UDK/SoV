using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.AI.Shells
{
    public interface IShell
    {
        float ReloadTime { get; set; }

        GameObject Target { get; set; }

        void Initiate();
    }
}
