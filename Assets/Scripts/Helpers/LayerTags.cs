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

        static private int[] _layersLevel =
        {
            Asteroid, Planet
        };

        static public bool IsFreeSpaceBody(int layer) =>
            _freeSpaceBodies.Contains(layer);

        static public bool IsBody(int layer) =>
            _body.Contains(layer);
        static public bool Is1LowerOrEqualLevel(int layer1, int layer2)
        {
            int index1 = -1;
            int index2 = -1;
            for(int i = 0; i < _layersLevel.Length; i++)
            {
                if(_layersLevel[i] == layer1)
                {
                    if (index2 != -1)
                    {
                        return index1 <= i;
                    }
                    index1 = i;
                }
                if(_layersLevel[i] == layer2)
                {
                    if (index1 != -1)
                    {
                        return index1 <= i;
                    }
                    index2 = i;
                }
            }

            return index1 <= index2;
        }
    }
}
