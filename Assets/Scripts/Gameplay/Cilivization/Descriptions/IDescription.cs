using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.Descriptions
{
    public interface IDescription: ICost
    {
        string Text { get; set; }

        Texture2D Image { get; set; }
    }
}
