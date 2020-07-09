using Assets.Scripts.Gameplay.Cilivization.AI.Bullets;
using Assets.Scripts.Gameplay.SpaceObject;
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

        public float ReloadTime;

        private float _timeLeft;

        public Turret()
        {
        }

        public void Attack(
            GameObject gameObject,
            Guid allianceGuid)
        {
            _timeLeft -= Time.fixedDeltaTime;
            if(_timeLeft > 0)
            {
                return;
            }

            var shell = Instantiate(Shell, transform.position, Quaternion.identity);
            var iShell = shell.GetComponent<IShell>();
            var iGameplayObject = shell.GetComponent<IGameplayObject>();
            iGameplayObject.AllianceGuid = allianceGuid;
            iShell.Target = gameObject;
            iShell.Initiate();
            _timeLeft = ReloadTime;
        }
    }


}
