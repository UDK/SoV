using Assets.Scripts.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Scripts.Manager.GameManager;

namespace Assets.Scripts.Manager.ClassSystem
{
    public abstract class UpgraderBase : MonoBehaviour
    {
        public void Upgrade(ITemplateManager templateManager, SpaceBody spaceBody, Mapping upgradeMapping)
        {
            ClearMesh(spaceBody);
            UpgradeClass(templateManager, spaceBody, upgradeMapping);
        }

        protected abstract void UpgradeClass(ITemplateManager templateManager, SpaceBody spaceBody, Mapping upgradeMapping);

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
