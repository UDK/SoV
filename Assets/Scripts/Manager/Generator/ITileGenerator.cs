using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Manager.Generator
{
    public interface ITileGenerator
    {
        TileType[,] GenerateTileMap(int height, int width, int scale);
    }
}
