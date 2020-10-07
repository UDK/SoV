using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public partial class GameManager
    {
        public static bool Pause
        {
            get
            {
                return Time.timeScale == 0;
            }
            set
            {
                if (value)
                {
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = 1;
                }
            }
        }
    }
}
