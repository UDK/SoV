using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    /// <summary>
    /// Класс enum со всеми тегами, которые нам нужны(не enum т.к. enum не может в string(((()
    /// </summary>
    public static class LayerEnums
    {
        static public int Asteroid = LayerMask.NameToLayer("asteroid");
        static public int Planet = LayerMask.NameToLayer("planet");
        static public int Magnet = LayerMask.NameToLayer("planetMagnet");
        static public int Satellite = LayerMask.NameToLayer("satellite");

        static private int[] _freeSpaceBodies =
        {
            Asteroid, Planet
        };

        static private int[] _body =
        {
            Asteroid, Planet, Satellite
        };

        static public bool IsFreeSpaceBody(int layer) =>
            _freeSpaceBodies.Contains(layer);

        static public bool IsBody(int layer) =>
            _body.Contains(layer);
    }
}
