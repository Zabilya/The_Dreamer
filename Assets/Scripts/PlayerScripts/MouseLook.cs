using System;
using UnityEngine;

namespace PlayerScripts
{
	public class MouseLook : MonoBehaviour
	{
		public float sensitivity = 5.0f;
		public float smoothing = 2.0f;
	
		private Vector2 _mouseLook;
		private Vector2 _smoothV;
		private GameObject _character;

		void Start()
		{
			_character = this.transform.parent.gameObject;
		}

		private void Update()
		{
			var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

			md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
			_smoothV.x = Mathf.Lerp(_smoothV.x, md.x, 1f / smoothing);
			_smoothV.y = Mathf.Lerp(_smoothV.y, md.y, 1f / smoothing);
			_mouseLook.x += _smoothV.x;
			var turnResultY = _mouseLook.y + _smoothV.y;
			_mouseLook.y = Math.Abs(turnResultY) > 90 ? _mouseLook.y : turnResultY;
			transform.localRotation = Quaternion.AngleAxis(-_mouseLook.y, Vector3.right);
			_character.transform.localRotation = Quaternion.AngleAxis(_mouseLook.x, _character.transform.up);
		}
	}
}
