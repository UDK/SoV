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
        public void Upgrade(ITemplateManager templateManager, SpaceBody spaceBody)
        {
            ClearMesh(spaceBody);
            UpgradeClass(templateManager, spaceBody);
        }

        protected virtual void UpgradeClass(ITemplateManager templateManager, SpaceBody spaceBody)
        {
        }

        protected void ClearMesh(SpaceBody spaceBody)
        {
            DeleteComponent<MeshRenderer>(spaceBody);
            DeleteComponent<MeshFilter>(spaceBody);
        }

        private void DeleteComponent<T>(SpaceBody spaceBody)
            where T : UnityEngine.Object
        {
            if (spaceBody.TryGetComponent<T>(out var component))
            {
                Destroy(component);
            }
        }
    }
}
