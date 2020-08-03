using Assets.Scripts.Gameplay.Cilivization.AI;
using Assets.Scripts.Gameplay.Cilivization.Workshop;
using Assets.Scripts.Gameplay.Cilivization.Workshop.UI;
using Assets.Scripts.Gameplay.SpaceObject;
using Assets.Scripts.Helpers;
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
        public SpaceshipContainer[] Fighters;

        public SpaceshipContainer[] Artilleries;

        public SpaceshipContainer[] Bombers;

        public ShellContainer[] Turrets;

        public ShellContainer[] Rockets;

        public ShellContainer ChosenTurret;

        public ShellContainer ChosenRocket;

        public SpaceshipContainer ChosenFighter;

        public SpaceshipContainer ChosenArtillery;

        public SpaceshipContainer ChosenBomber;

        public SpaceshipContainer ChosenBuild;

        public SpaceshipTypes BuildNext;

        public float TimeLeftTillBuild;

        public Database ShipsDatabase;

        public WorkshopUIManager WorkshopUIManager;

        public RenderTexture RenderTexture;

        public Guid AllianceGuid { get; set; }

        private void Start()
        {
            var spaceBody = GetComponent<SpaceBody>();
            AllianceGuid = spaceBody.AllianceGuid;
            WorkshopLibrary.AddWorkshop(
                AllianceGuid,
                ShipsDatabase,
                WorkshopUIManager);
            WorkshopLibrary.GetWorkshop(AllianceGuid)
                .EditTemplates();
        }

        private void Update()
        {
            //Build();
        }

        private void Build()
        {
            TimeLeftTillBuild -= Time.deltaTime;
            if (TimeLeftTillBuild > 0)
            {
                return;
            }

            CreateNewSpaceship();
        }

        private void CreateNewSpaceship()
        {
            TimeLeftTillBuild = ChosenBuild.TimeToBuild;
            var spaceShip = Instantiate(
                ChosenBuild.SpaceShip,
                transform.position,
                Quaternion.identity);
            foreach (Transform child in spaceShip.transform)
            {
                if (child.gameObject.CompareTag(EnumTags.RocketLot))
                {
                    var rocket = Instantiate(
                        ChosenRocket.Weapon,
                        transform.position,
                        Quaternion.identity);
                    rocket.transform.parent = child;
                    TimeLeftTillBuild += ChosenRocket.TimeToBuild;
                }
                else if (child.gameObject.CompareTag(EnumTags.TurretLot))
                {
                    var turret = Instantiate(
                        ChosenTurret.Weapon,
                        transform.position,
                        Quaternion.identity);
                    turret.transform.parent = child;
                    TimeLeftTillBuild += ChosenTurret.TimeToBuild;
                }
            }
        }

        private void CalculateTime()
        {
            TimeLeftTillBuild = ChosenBuild.TimeToBuild;
            foreach (Transform child in ChosenBuild.SpaceShip.transform)
            {
                if (child.gameObject.CompareTag(EnumTags.RocketLot))
                {
                    TimeLeftTillBuild += ChosenRocket.TimeToBuild;
                }
                else if (child.gameObject.CompareTag(EnumTags.TurretLot))
                {
                    TimeLeftTillBuild += ChosenTurret.TimeToBuild;
                }
            }
        }
    }
}
