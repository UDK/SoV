using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Manager.Generator
{
    [Serializable]
    public class TileType
    {
        [SerializeField]
        public Tile Tile;

        public GameObject Template;

        public float Height;

        public static implicit operator Tile(TileType tile)
        {
            return tile.Tile;
        }

        public static implicit operator GameObject(TileType tile)
        {
            return tile.Template;
        }
    }
}
