using Assets.Scripts.Manager.ClassSystem;
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
        public static int Asteroid = LayerMask.NameToLayer("asteroid");
        public static int Planet = LayerMask.NameToLayer("planet");
        public static int Sun = LayerMask.NameToLayer("sun");
        public static int Magnet = LayerMask.NameToLayer("planetMagnet");
        public static int planetSatellite = LayerMask.NameToLayer("planetSatellite");
        public static int sunSatellite = LayerMask.NameToLayer("sunSatellite");

        public static Dictionary<SpaceClasses, int> ClassMap2Layer =
            new Dictionary<SpaceClasses, int>
        {
            { SpaceClasses.Asteroid, Asteroid },
            { SpaceClasses.Planet, Planet },
            { SpaceClasses.Sun, Sun },
        };

        public static Dictionary<SpaceClasses, int> ClassSatMap2Layer =
            new Dictionary<SpaceClasses, int>
        {
            { SpaceClasses.Asteroid, planetSatellite },
            { SpaceClasses.Planet, planetSatellite },
            { SpaceClasses.Sun, sunSatellite },
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public bool IsSatellite(int layer) =>
            layer == planetSatellite || layer == sunSatellite;

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
                || layer == planetSatellite
                || layer == sunSatellite;
        }
    }
}
