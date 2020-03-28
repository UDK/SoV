using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Maths.Adapters
{
    public interface INoiseAdapter<TTile>
    {
        TTile[,] GenerateNoise(
            int heigth,
            int width,
            float scale,
            Func<float, TTile> transformator);
    }
}
