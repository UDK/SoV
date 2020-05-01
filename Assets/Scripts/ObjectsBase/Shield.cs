using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ObjectsBase
{
    public class Shield : MonoBehaviour
    {
        public GameObject ripplexVFX;

        private Material mat;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var ripples = Instantiate(ripplexVFX, transform) as GameObject;
            var psr = ripples.GetComponent<ParticleSystemRenderer>();
            mat = psr.material;
            mat.SetVector("_SphereCenter", collision.contacts[0].point);

            Destroy(ripples, 2);
        }
    }
}
