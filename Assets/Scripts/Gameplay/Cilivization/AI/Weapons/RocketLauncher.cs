using Assets.Scripts.Gameplay.Cilivization.AI.Shells;
using Assets.Scripts.Gameplay.SpaceObject;
using Assets.Scripts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.AI.Weapons
{
    public class RocketLauncher : MonoBehaviour, IWeapon
    {
        public GameObject Shell;

        public float ReloadTime;

        private float _timeLeft;

        public RocketLauncher()
        {
        }

        public void Attack(
            GameObject target,
            Guid allianceGuid)
        {
            _timeLeft -= Time.fixedDeltaTime;
            if (_timeLeft > 0)
            {
                return;
            }
            var heading = target.transform.position - transform.position;
            var right = transform.right;
            var dot = Vector3.Dot(
                    right,
                    heading);
            var angle = Vector3.Angle(right, heading);
            if (angle < 20)
            {
                Shell.CheckComponent<UsualRocket>(_ =>
                    ShellHelper.InitShell<UsualRocket>(
                            Shell,
                            target,
                            transform.position,
                            allianceGuid))
                    ?.CheckComponent<MonoBehaviour>(_ =>
                        throw new ArgumentException("Unknown shell", nameof(RocketLauncher)));
                _timeLeft = ReloadTime;
            }
        }
    }
}
