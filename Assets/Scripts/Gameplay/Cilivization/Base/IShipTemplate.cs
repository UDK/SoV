using Assets.Scripts.Gameplay.Cilivization.Descriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.Base
{
    public interface IShipTemplate
    {
        string Name { get; set; }

        ShipCost Cost { get; }

        GameObject BuiltTemplate { get; }
    }

    public class ShipCost : ICost
    {
        public int Money { get; set; }
        public int Merilium { get; set; }
        public int Titan { get; set; }
        public int Uranus { get; set; }
        public int Time { get; set; }
    }
}
