using Assets.Scripts.Gameplay.Cilivization.AI.Shells;
using Assets.Scripts.Gameplay.SpaceObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.AI.Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        public GameObject Shell;

        protected ShellBase _shellBase;

        private float _timeLeft;

        private void Start()
        {
            _shellBase = Shell.GetComponent<ShellBase>();
        }

        public virtual void Attack(
            GameObject target,
            Guid allianceGuid)
        {
            _timeLeft -= Time.fixedDeltaTime;
            if (_timeLeft > 0)
            {
                return;
            }
            CoreAttack(target, allianceGuid);
            _timeLeft = _shellBase.ReloadTime;
        }

        protected virtual void CoreAttack(
            GameObject target,
            Guid allianceGuid)
        {
        }

        protected void InitShell(
            GameObject target,
            Guid allianceGuid)
        {
            var shell = ShellCollection.Get(
                Shell,
                transform.position,
                Shell.transform.rotation);
            var iShell = shell.GetComponent<ShellBase>();
            var iGameplayObject = shell.GetComponent<IGameplayObject>();
            iGameplayObject.AllianceGuid = allianceGuid;
            iShell.Target = target;
            iShell.Initiate();
        }
    }
}
