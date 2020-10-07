using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Gameplay.Cilivization.Descriptions
{
    public interface ICost
    {
        int Money { get; set; }

        int Merilium { get; set; }

        int Titan { get; set; }

        int Uranus { get; set; }

        int Time { get; set; }
    }
}
