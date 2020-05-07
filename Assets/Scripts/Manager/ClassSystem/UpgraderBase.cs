using Assets.Scripts.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Manager.ClassSystem
{
    public abstract class UpgraderBase : MonoBehaviour
    {
        public virtual void Upgrade(ITemplateManager templateManager, SpaceBody spaceBody)
        {
        }
    }
}
