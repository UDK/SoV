using Assets.Scripts.Gameplay;
using Assets.Scripts.GlobalControllers;
using Assets.Scripts.Manager.ClassSystem;
using Assets.Scripts.Manager.Galaxy;
using Assets.Scripts.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public partial class GameManager : ITemplateManager
    {
        public GameObject SpaceBodyTemplate;

        public GameObject GravitationTemplate;

        public SpaceBody GenerateSpaceBody =>
            Instantiate(SpaceBodyTemplate).GetComponent<SpaceBody>();

        public GravitationBehaviour SetUpGravitation(SpaceBody spaceBody)
        {
            var gravitation = FindChild<GravitationBehaviour>(spaceBody.transform);
            if(gravitation == null)
            {
                gravitation =
                    Instantiate(GravitationTemplate, spaceBody.transform)
                        .GetComponent<GravitationBehaviour>();
            }

            return gravitation;
        }

        private T FindChild<T>(Transform transform)
            where T : class
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent<T>(out T component))
                {
                    return component;
                }
            }

            return null;
        }
    }
}
