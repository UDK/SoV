using Assets.Scripts.Gameplay.SpaceObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.AI.Shells
{
    public static class ShellHelper
    {
        public static void InitShell<TShell>(
            GameObject originalShell,
            GameObject target,
            Vector3 startPosition,
            Guid allianceGuid)
            where TShell : MonoBehaviour
        {
            var shell = ShellCollection<TShell>.Get(
                originalShell,
                startPosition,
                originalShell.transform.rotation);
            var iShell = shell.GetComponent<IShell>();
            var iGameplayObject = shell.GetComponent<IGameplayObject>();
            iGameplayObject.AllianceGuid = allianceGuid;
            iShell.Target = target;
            iShell.Initiate();
        }
    }
}
