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

        public static void AddWorkshop(
            Guid allianceGuid,
            Database database,
            WorkshopUIManager workshopUIManager)
        {
            _workshops.Add(allianceGuid, new WorkshopManager
            {
                Database = database,
                WorkshopUIManager = workshopUIManager,
            });
        }

        public static WorkshopManager GetWorkshop(
            Guid allianceGuid)
        {
            return _workshops[allianceGuid];
        }
    }
}
