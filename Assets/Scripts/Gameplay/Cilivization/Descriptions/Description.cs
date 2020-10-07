using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.Descriptions
{
    public class Description : MonoBehaviour, IDescription
    {
        [field: SerializeField]
        public int Money { get; set; }

        [field: SerializeField]
        public int Merilium { get; set; }

        [field: SerializeField]
        public int Titan { get; set; }

        [field: SerializeField]
        public int Uranus { get; set; }

        [field: SerializeField]
        public int Time { get; set; }

        [field: SerializeField]
        public string Text { get; set; }

        [field: SerializeField]
        public Texture2D Image { get; set; }
    }
}
