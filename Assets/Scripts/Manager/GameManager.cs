using Assets.Scripts.GlobalControllers;
using Assets.Scripts.GlobalControllers.Control;
using Assets.Scripts.Manager.Galaxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public partial class GameManager : MonoBehaviour
    {
        [SerializeField]
        public float PlayerAcceleration;

        [SerializeField]
        public GalaxyBehaviour[] GalaxyBehaviours = null;

        private GameManager()
        {

        }

        void Awake()
        {
            // DontDestroyOnLoad(gameObject);

            //Call the InitGame function to initialize the first level 
            InitGame();
        }

        void InitGame()
        {
            // Create player
            var player = GetFullObject(ClassSystem.SpaceClasses.Planet);
            SetControlForGameObject<PlayerControl>(player.gameObject);


            var galaxy = Instantiate(GalaxyBehaviours[0]);
            galaxy.Init(this);

            var camera =
                transform.parent.gameObject.AddComponent<CameraBehaviour>();

            camera.Player = player;
            galaxy.PlaceGameObject(player);

            galaxy.InitBackground(camera);
            galaxy.InitBorders();
        }

        //Update is called every frame.
        void Update()
        {

        }

        private void SetControlForGameObject<TControl>(
            GameObject player)
            where TControl : ControlBehaviourBase
        {
            var control = player.AddComponent<TControl>();
            control.Acceleration = PlayerAcceleration;
        }
    }
}
