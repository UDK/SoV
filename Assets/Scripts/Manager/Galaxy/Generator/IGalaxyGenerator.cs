using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Manager.Galaxy.Generator
{
    public interface IGalaxyGenerator
    {
        GalaxyObject[,] GenerateTileMap(int height, int width, int scale);
    }
}
