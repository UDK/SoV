using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.GlobalControllers
{
    class ScrollUVBehaviour : MonoBehaviour
	{
		public float Parralax = 2f;

		/*private float _height = 0f;
		private float _width = 0f;*/

		void Start()
		{
			/*_height = mat.mainTexture.height / mat.mainTextureScale.y;
			_width = mat.mainTexture.width / mat.mainTextureScale.x;*/
			SetOffset(
				transform.position.x + Random.Range(-50, 50),
				transform.position.y + Random.Range(-50, 50));


		}
		void Update()
		{
			SetOffset(transform.position.x, transform.position.y);
		}

		private void SetOffset(float x, float y)
		{
			MeshRenderer mr = GetComponent<MeshRenderer>();

			Material mat = mr.material;

			Vector2 offset = mat.mainTextureOffset;

			var lx = x / transform.localScale.x / Parralax;
			var ly = y / transform.localScale.y / Parralax;

			offset.x = lx;
			offset.y = ly;
			mat.mainTextureOffset = offset;
		}
	}
}
