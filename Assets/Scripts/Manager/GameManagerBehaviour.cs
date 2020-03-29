﻿using Assets.Scripts.GlobalControllers;
using Assets.Scripts.Manager.Galaxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    class GameManagerBehaviour : MonoBehaviour
    {
        [SerializeField]
        public GalaxyBehaviour[] GalaxyBehaviours = null;

        private GameManagerBehaviour()
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
            var galaxy = Instantiate(GalaxyBehaviours[0]);

            galaxy.Init();
            var player = galaxy.InitPlayer();
            var camera = gameObject.AddComponent<CameraBehaviour>();
            camera.Player = player;
            galaxy.InitBackground(camera);
            galaxy.InitBorders();
        }

        //Update is called every frame.
        void Update()
        {

        }
    }
}
