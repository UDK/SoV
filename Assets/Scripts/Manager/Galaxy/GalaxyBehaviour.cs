using Assets.Scripts.Gameplay;
using Assets.Scripts.GlobalControllers;
using Assets.Scripts.Manager.Background;
using Assets.Scripts.Manager.ClassSystem;
using Assets.Scripts.Manager.Galaxy.Generator;
using Assets.Scripts.Maths.NoiseFabrics;
using Assets.Scripts.Physics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Manager.Galaxy
{
    public class GalaxyBehaviour : MonoBehaviour
    {
        [SerializeField]
        public int MaxSpeedForObjects;

        [SerializeField]
        public int Height;

        [SerializeField]
        public int Width;

        [SerializeField]
        public int PointsOnOnePlanet;

        [SerializeField]
        public int PlanetNoiseScale;

        [SerializeField]
        public GalaxyObject[] SpaceObjects;

        [SerializeField]
        public BackgroundQuad[] BackgroundQuads;

        private IGalaxyGenerator _tileGenerator;

        private void Awake()
        {
            _tileGenerator =
                   new GalaxyGenerator<
                       PerlinNoise<GalaxyObject>>(
                           SpaceObjects);
        }
        public void Init(IUpgradeManager upgradeManager)
        {
            var height = (int)(Height / PointsOnOnePlanet);
            var width = (int)(Width / PointsOnOnePlanet);

            var tileMap = _tileGenerator.GenerateTileMap(height, width, PlanetNoiseScale);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    GalaxyObject tileType = tileMap[y, x];

                    if(tileType.SpaceClass != SpaceClasses.Nothing)
                    {
                        var spaceBody = upgradeManager.GetFullObject(tileType.SpaceClass);
                        spaceBody.transform.parent = transform;
                        spaceBody.transform.position =
                            new Vector3(
                                x * PointsOnOnePlanet,
                                y * PointsOnOnePlanet,
                                0f);
                        var movement = spaceBody.GetComponent<MovementBehaviour>();
                        movement.MaxVelocity = MaxSpeedForObjects;
                        movement.SetVelocity(
                            new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f)));
                    }
                }
            }
        }

        public void PlaceGameObject(
            SpaceBody gameObject,
            Vector3? placePosition = null)
        {
            var movement = gameObject.GetComponent<MovementBehaviour>();
            movement.MaxVelocity = MaxSpeedForObjects;

            gameObject.transform.parent = transform;
            gameObject.transform.position = placePosition ??
                new Vector3(Random.Range(0f, Width), Random.Range(0f, Height), 0f);
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
    }
}
