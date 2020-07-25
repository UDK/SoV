using Assets.Scripts.Gameplay.Cilivization.Workshop.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.Workshop
{
    public class WorkshopManager
    {
        public ShipsDatabase Database;

        public WorkshopUIManager WorkshopUIManager;

        /// <summary>
        /// Existed templates os spaceships
        /// </summary>
        private readonly List<ShipTemplate> Templates =
            new List<ShipTemplate>();

        public void EditTemplates()
        {
            WorkshopUIManager.Enable(
                Templates,
                Database);
        }

        public void AddTemplate(
            ShipTemplate shipTemplate)
        {
            Templates.Add(shipTemplate);
        }
    }
}
