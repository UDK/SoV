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
            var vertices = new Vector2[CountPoligons];
            //Смотрит ввехр и постоянно идет вправо, пока не сделает фулл оборот
            float defaultAngle = 90f;
            float defaultRadius = 128f;
            for (int iter = 1; iter <= CountPoligons; iter++)
            {
                float dispersionRadius = UnityEngine.Random.Range(-(defaultRadius / 5), defaultRadius / 5);
                float dispersionAngleCircle = UnityEngine.Random.Range(-(360 / ((float)CountPoligons * 2.5f)), 360 / ((float)CountPoligons * 2.5f));
                var angle = AngleCircle(defaultAngle - (360 * (iter / (float)CountPoligons)) + dispersionAngleCircle);
                var radius = defaultRadius + dispersionRadius;
                float x = radius * Mathf.Cos(DegressToRadian(angle));
                float y = radius * Mathf.Sin(DegressToRadian(angle));
                vertices[iter - 1] = new Vector2(x + defaultRadius * 1.5f, y + defaultRadius * 1.5f);
            }
            return new PropertySpriteGeometry
            {
                vertices = vertices,
                triangles = GenerateTriangles(CountPoligons)
            };
        }

        ushort[] GenerateTriangles(int count)
        {
            ushort[] result = new ushort[(count-2)*3];
            result[0] = 0;
            result[1] = 1;
            result[2] = 2;
            for (int iter = 1; iter < count - 2; iter++)
            {
                result[iter*3] = 0;
                result[iter*3 + 1] = (ushort)(iter + 1);
                result[iter*3 + 2] = (ushort)(iter + 2);
            }
            //for (int iter = 0; iter < count - 2; iter++)
            //{
            //    result[iter * 3] = (ushort)(iter);
            //    result[iter * 3 + 1] = (ushort)(iter + 1);
            //    result[iter * 3 + 2] = (ushort)(iter + 2);
            //}
            //result[(count - 2) * 3] = (ushort)(count - 2);
            //result[(count - 2) * 3 + 1] = (ushort)(count - 1);
            //result[(count - 2) * 3 + 2] = 0;
            //result[(count - 1) * 3] = (ushort)(count - 1);
            //result[(count - 1) * 3 + 1] = 0;
            //result[(count - 1) * 3 + 2] = 1;
            return result;
        }

        float DegressToRadian(float degress) => (degress * Mathf.PI) / 180;

        float AngleCircle(float value)
        {
            if (value < 0)
            {
                value = 360 - Mathf.Abs(value);
            }
            else if (value > 360)
            {
                value = value - 360;
            }
            else
            {
                return value;
            }
            return AngleCircle(value);
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
