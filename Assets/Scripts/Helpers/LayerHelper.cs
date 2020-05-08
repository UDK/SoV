using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    /// <summary>
    /// Класс enum со всеми тегами, которые нам нужны(не enum т.к. enum не может в string(((()
    /// </summary>
    public static class LayerHelper
    {
        static public int Asteroid = LayerMask.NameToLayer("asteroid");
        static public int Planet = LayerMask.NameToLayer("planet");
        static public int Sun = LayerMask.NameToLayer("sun");
        static public int Magnet = LayerMask.NameToLayer("planetMagnet");
        static public int Satellite = LayerMask.NameToLayer("satellite");

        /*static public int[] SpaceLayers =
        {
            Asteroid,
            Planet,
            Sun
        };*/

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public bool IsSatellite(int layer) =>
            layer == Satellite;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public bool IsLower(int layer1, int layer2)
        {
            return layer1 < layer2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public bool IsBigger(int layer1, int layer2)
        {
            return layer1 > layer2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public bool IsBody(int layer)
        {
            return (Asteroid <= layer && layer <= Sun)
                || layer == Satellite;
        }
    }
}
