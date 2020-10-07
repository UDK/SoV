using Assets.Scripts.Gameplay.Cilivization.AI;
using Assets.Scripts.Gameplay.Cilivization.Workshop;
using Assets.Scripts.Gameplay.Cilivization.Workshop.UI;
using Assets.Scripts.Gameplay.SpaceObject;
using Assets.Scripts.Helpers;
using Assets.Scripts.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Cilivization.Base
{
    public class CivilBase : MonoBehaviour
    {
        public float TimeLeftTillBuild;

        public Database ShipsDatabase;

        public WorkshopUIManager WorkshopUIManager;

        public Guid AllianceGuid { get; set; }

        public ShipBuildTemplateDB ShipBuildTemplateDB;

        private IShipTemplate[] _templates;

        private List<GameObject> controledShips =
            new List<GameObject>();
        private int maxShips = 1;

        private void Start()
        {
            var spaceBody = GetComponent<SpaceBody>();
            AllianceGuid = spaceBody.AllianceGuid;
            var workshop = WorkshopLibrary.AddWorkshop(
                 AllianceGuid,
                 ShipsDatabase,
                 WorkshopUIManager);
            ShipBuildTemplateDB.Templates.ForEach(t =>
            {
                workshop.AddTemplate(t);
            });
            _templates = workshop.BuildTemplates();
            /*
            workshop.EditTemplates(() =>
            {
                _templates = workshop.BuildTemplates();
            });*/
        }

        private void Update()
        {
            if (GameManager.Pause)
            {
                return;
            }

            Build();
        }

        private void Build()
        {
            controledShips.RemoveAll(s => s.ToString() == "null");
            if (controledShips.Count >= maxShips)
            {
                return;
            }
            if (TimeLeftTillBuild > 0)
            {
                TimeLeftTillBuild -= Time.deltaTime;
                return;
            }
            var ship = Instantiate(
                    _templates[
                        UnityEngine.Random.Range(0, _templates.Count())].BuiltTemplate,
                    new Vector2(
                        transform.position.x + UnityEngine.Random.Range(-5, 5),
                        transform.position.y + UnityEngine.Random.Range(-5, 5)),
                    Quaternion.identity);
            ship.SetActive(true);
            var ss = ship.GetComponent<SpaceShipAI>();
            ss.Homing = gameObject;
            ss.AllianceGuid = AllianceGuid;
            controledShips.Add(ship);
        }
    }
}
