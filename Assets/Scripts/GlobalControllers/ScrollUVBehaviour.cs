using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GlobalControllers
{
    class ScrollUVBehaviour : MonoBehaviour
	{
		public float parralax = 2f;
		private float _start_offset_x = 0f;
		private float _start_offset_y = 0f;

		void Start()
		{
			MeshRenderer mr = GetComponent<MeshRenderer>();

			Material mat = mr.material;

			Vector2 offset = mat.mainTextureOffset;

			_start_offset_x = offset.x;
			_start_offset_y = offset.y;
		}
		void Update()
		{

			MeshRenderer mr = GetComponent<MeshRenderer>();

			Material mat = mr.material;

			Vector2 offset = mat.mainTextureOffset;

			offset.x = transform.position.x / transform.localScale.x / parralax + _start_offset_x;
			offset.y = transform.position.y / transform.localScale.y / parralax + _start_offset_y;

			mat.mainTextureOffset = offset;

		}
	}
}
