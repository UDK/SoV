using Assets.Scripts.Gameplay.Cilivization.Workshop.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Gameplay.Cilivization.Workshop
{
    public static class WorkshopLibrary
    {
        private static Dictionary<Guid, WorkshopManager> _workshops =
            new Dictionary<Guid, WorkshopManager>();

        public static WorkshopManager AddWorkshop(
            Guid allianceGuid,
            Database database,
            WorkshopUIManager workshopUIManager)
        {
            var workshopManager = new WorkshopManager
            {
                Database = database,
                WorkshopUIManager = workshopUIManager,
            };
            _workshops.Add(allianceGuid, workshopManager);

            return workshopManager;
        }

        public static WorkshopManager GetWorkshop(
            Guid allianceGuid)
        {
            return _workshops[allianceGuid];
        }
    }
}
