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
        public int Scale;

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
            var tileMap = _tileGenerator.GenerateTileMap(Height, Width, Scale);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    TileType tileType = tileMap[y, x];

                    if(tileType != Tile.Nothing)
                    {
                        var gameObject = Instantiate(
                            tileType,
                            new Vector3(
                                Random.Range(x - 2, x + 2) * 5,
                                Random.Range(y - 2, y + 2) * 5,
                                0f),
                            Quaternion.identity,
                            transform) as GameObject;
                    }
                }
            }
        }

        public MonoBehaviour InitPlayer()
        {
            BodyBehaviourBase player;
            while (!transform.GetChild(
                Random.Range(0, transform.childCount))
                    .TryGetComponent<BodyBehaviourBase>(out player));

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

        private void SetControlForGameObject<TControl>(
            GameObject player)
            where TControl : ControlBehaviourBase
        {
            var control = player.AddComponent<TControl>();
            control.TargetVelocity = MaxSpeed;
        }
    }
}
