using Assets.Scripts.GlobalControllers;
using Assets.Scripts.GlobalControllers.Control;
using Assets.Scripts.Manager.Background;
using Assets.Scripts.Manager.Galaxy;
using Assets.Scripts.Manager.Generator;
using Assets.Scripts.Maths.NoiseFabrics;
using Assets.Scripts.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Manager.Galaxy
{
    public class GalaxyBehaviour : MonoBehaviour
    {
        [SerializeField]
        public int TargetSpeedForPlayers;

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
                       PerlinNoise<TileType>>(
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
                                x * PointsOnOnePlanet,
                                y * PointsOnOnePlanet,
                                0f),
                            Quaternion.identity,
                            transform) as GameObject;
                        gameObject.GetComponent<MovementBehaviour>().SetVelocity(
                            new Vector3(Random.Range(-1, 1), Random.Range(-1, 1)) * Random.Range(0.2f, 0.8f));
                    }
                }
            }
        }

        public MonoBehaviour InitPlayer()
        {
            BodyBehaviourBase player = null;
            foreach (var tileType in TileTypes)
            {
                if (tileType == Tile.Planet)
                {
                    var body = AddBody(tileType, Random.Range(0f, Width), Random.Range(0f, Height));
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

        public void InitBorders()
        {
            CreateBorder(
                "BorderLeft",
                new Vector2(5f, Height),
                new Vector2(-2.5f, Height / 2),
                Vector2.right);
            CreateBorder(
                "BorderTop",
                new Vector2(Width, 5f),
                new Vector2(Width / 2, Height + 2.5f),
                Vector2.down);
            CreateBorder(
                "BorderRight",
                new Vector2(5f, Height),
                new Vector2(Width + 2.5f, Height / 2),
                Vector2.left);
            CreateBorder(
                "BorderDown",
                new Vector2(Width, 5f),
                new Vector2(Width / 2, -2.5f),
                Vector2.up);
        }

        private void CreateBorder(
            string name,
            Vector2 size,
            Vector2 position,
            Vector2 pushDirection)
        {
            GameObject gameObject = new GameObject(name);
            var collider = gameObject.AddComponent<BoxCollider2D>();
            collider.size = size;
            collider.isTrigger = true;
            var border = gameObject.AddComponent<BorderBehaviour>();
            border.PushDirection = pushDirection;
            gameObject.transform.SetParent(transform);
            gameObject.transform.localPosition = position;
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
            control.TargetVelocity = TargetSpeedForPlayers;
        }
    }
}
