using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

namespace Assets.DOTSScripts.GlobalControllers.InputControl
{
    public static class InputDecider
    {
        public static float2 GetMovingVector(Actions currentAction)
        {
            float2 currentMove = 0;
            if ((currentAction.Move & Move.Up) != 0)
            {
                currentMove.y = 1;
            }
            if ((currentAction.Move & Move.Down) != 0)
            {
                currentMove.y = -1;
            }
            if ((currentAction.Move & Move.Rigth) != 0)
            {
                currentMove.x = 1;
            }
            if ((currentAction.Move & Move.Left) != 0)
            {
                currentMove.x = -1;
            }
            return currentMove;
        }
    }
}
