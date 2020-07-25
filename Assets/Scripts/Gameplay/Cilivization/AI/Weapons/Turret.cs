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
    public class Turret : MonoBehaviour, IWeapon
    {
        public GameObject Shell;

        private float _timeLeft;

        public Turret()
        {
        }

        public void Attack(
            GameObject target,
            Guid allianceGuid)
        {
            _timeLeft -= Time.fixedDeltaTime;
            if(_timeLeft > 0)
            {
                return;
            }

            Shell.CheckComponent<UsualBullet>(usualBullet =>
                {
                    _timeLeft = usualBullet.ReloadTime;
                    ShellHelper.InitShell<UsualBullet>(
                                Shell,
                                target,
                                transform.position,
                                allianceGuid);
                    _timeLeft = usualBullet.ReloadTime;
                })
                ?.CheckComponent<MonoBehaviour>(_ =>
                    throw new ArgumentException("Unknown shell", nameof(RocketLauncher)));
        }
    }


}
