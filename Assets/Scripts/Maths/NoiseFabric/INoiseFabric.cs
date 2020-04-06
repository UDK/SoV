using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Maths.NoiseFabrics
{
    public interface INoiseFabric<TTile>
    {
        TTile[,] GenerateNoise(
            int heigth,
            int width,
            float scale,
            Func<float, TTile> transformator);
    }
}
