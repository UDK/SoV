using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Helpers
{

    [RequireComponent(typeof(SpriteRenderer))]
    class GenerateAsteroidsMesh : MonoBehaviour
    {
        [SerializeField]
        Texture2D texture;

        [SerializeField]
        int countBulges;

        [SerializeField]
        int countPoligons;

        public int CountPoligons
        {
            get => countPoligons;
            set
            {
                if (countBulges >= CountPoligons)
                {
                    CountPoligons += CountPoligons;
                }
            }
        }

        private void Start()
        {
            var sprites = GetComponent<SpriteRenderer>();
            var geometryRandomSprite = CreateShape();
            var newSprite = UpdateMesh(geometryRandomSprite, texture, sprites.sprite);
            sprites.sprite = newSprite;
        }

        PropertySpriteGeometry CreateShape()
        {
            UnityEngine.Random.Range();
            //Смотрит ввехр и постоянно идет вправо, пока не сделает фулл оборот
            Vector2 startVector = new Vector2(0f,90f);
            for (int iter = 0; iter < CountPoligons; iter++)
            {
                
            }
            return new PropertySpriteGeometry
            {
                vertices = new Vector2[]
                {
                    new Vector2(0,0),
                    new Vector2(0,128),
                    new Vector2(18,34),
                    new Vector2(12,0)
                },
                triangles = new ushort[]
                {
                    0,1,2,
                    1,2,3,
                    2,3,0
                }
            };
        }


        Sprite UpdateMesh(PropertySpriteGeometry propertySprite, Texture2D texture, Sprite sprite)
        {
            Texture2D textures = new Texture2D(1024, 1024);

            for (int y = 0; y < textures.height; y++)
            {
                for (int x = 0; x < textures.width; x++)
                {
                    Color color = ((x & y) != 0 ? Color.red : Color.green);
                    textures.SetPixel(x, y, color);
                }
            }
            textures.Apply();

            var mySprite = Sprite.Create(textures, new Rect(0.0f, 0.0f, textures.width, textures.height), new Vector2(0.5f, 0.5f));
            mySprite.OverrideGeometry(propertySprite.vertices, propertySprite.triangles);

            return mySprite;
        }

        struct PropertySpriteGeometry
        {
            public Vector2[] vertices;
            public ushort[] triangles;
        }
    }
}
