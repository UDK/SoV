﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Helpers
{
    /// <summary>
    /// Класс enum со всеми тегами, которые нам нужны(не enum т.к. enum не может в string(((()
    /// </summary>
    public static class EnumTags
    {
        static public string Satellite = "Satellite";
        static public string Core = "Core Gravity";
        static public string FreeSpaceBody = "Space body";
    }
}
