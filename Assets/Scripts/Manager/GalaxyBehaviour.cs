using Assets.Scripts.GlobalControllers;
using Assets.Scripts.GlobalControllers.Control;
using Assets.Scripts.Manager.Background;
using Assets.Scripts.Manager.Generator;
using Assets.Scripts.Maths.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Manager
{
    public class GalaxyBehaviour : MonoBehaviour
    {
        [SerializeField]
        public int MaxSpeed;

        [SerializeField]
        public int Height;

        [SerializeField]
        public int Width;

        [SerializeField]
        public int PointsOnOnePlanet;

        [SerializeField]
        public int PlanetNoiseScale;

        [SerializeField]
        public TileType[] TileTypes;

        [SerializeField]
        public BackgroundQuad[] BackgroundQuads;

        private ITileGenerator _tileGenerator;

        private void Awake()
        {
            _tileGenerator =
                   new TileGenerator<
                       PerlinNoiseAdapter<TileType>>(
                           TileTypes);
        }
        public void Init()
        {
            var height = (int)(Height / PointsOnOnePlanet);
            var width = (int)(Width / PointsOnOnePlanet);

            var tileMap = _tileGenerator.GenerateTileMap(height, width, PlanetNoiseScale);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    TileType tileType = tileMap[y, x];

                    if(tileType != Tile.Nothing)
                    {
                        var gameObject = Instantiate(
                            tileType,
                            new Vector3(
                                x * width,
                                y * height,
                                0f),
                            Quaternion.identity,
                            transform) as GameObject;
                    }
                }
            }
        }

        public MonoBehaviour InitPlayer()
        {
            // may be we should use else statement
            BodyBehaviourBase player = null;
            foreach (var tileType in TileTypes)
            {
                if (tileType == Tile.Planet)
                {
                    var body = AddBody(tileType, Random.Range(0, Width), Random.Range(0, Height));
                    player = body.GetComponent<BodyBehaviourBase>();
                    break;
                }
            }

            if (player == null)
            {
                throw new InvalidOperationException("Tile type for player wasn't set");
            }

            SetControlForGameObject<PlayerControlBehaviour>(
                player.gameObject);

            return player;
        }

        public void InitBackground(
            MonoBehaviour camera)
        {
            foreach(var back in BackgroundQuads)
            {
                var quad = 
                    Instantiate(
                        back.Back,
                        new Vector3(
                            camera.transform.position.x,
                            camera.transform.position.y,
                            camera.transform.position.z * -1),
                        Quaternion.identity) as GameObject;
                var scroll = quad.AddComponent<ScrollUVBehaviour>();
                scroll.Parralax = back.Parallax;
                quad.transform.SetParent(camera.transform);
            }
        }

        private GameObject AddBody(GameObject gameObject, float x, float y)
        {
            var body = Instantiate(
                gameObject,
                new Vector3(
                    x,
                    y,
                    0f),
                Quaternion.identity,
                transform) as GameObject;
            return body;
        }
        private void SetControlForGameObject<TControl>(
            GameObject player)
            where TControl : ControlBehaviourBase
        {
            var control = player.AddComponent<TControl>();
            control.TargetVelocity = MaxSpeed;
        }
    }
}
