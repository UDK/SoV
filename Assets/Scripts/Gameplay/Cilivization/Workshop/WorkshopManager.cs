using Assets.Scripts.Gameplay.Cilivization.Base;
using Assets.Scripts.Gameplay.Cilivization.Workshop.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Scripts.Gameplay.Cilivization.Workshop.UI.WorkshopUIManager;

namespace Assets.Scripts.Gameplay.Cilivization.Workshop
{
    public class WorkshopManager
    {
        public Database Database;

        public WorkshopUIManager WorkshopUIManager;

        /// <summary>
        /// Existed templates os spaceships
        /// </summary>
        private readonly List<ShipBuildTemplate> Templates =
            new List<ShipBuildTemplate>();

        public void EditTemplates(
            OnClose onClose)
        {
            WorkshopUIManager.Enable(
                Templates,
                Database,
                onClose);
        }

        public void AddTemplate(
            ShipBuildTemplate shipTemplate)
        {
            Templates.Add(shipTemplate);
        }

        public IShipTemplate[] BuildTemplates()
        {
            return Templates.Select(t => t.Build()).ToArray();
        }
    }
}
