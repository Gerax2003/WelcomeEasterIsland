///-----------------------------------------------------------------
/// Author : Adrien Lemaire
/// Date : 11/07/2022 15:33
///-----------------------------------------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace Com.Isartdigital.EasterIsland.Common.Tools {
	public class RotateObjectAroundCenter : MonoBehaviour 
	{
		[Header("GameObjects")]
		public static RotateObjectAroundCenter Instance = default;
		[SerializeField] private Transform center = default;

		[Header("Maths")]
		[SerializeField] private Vector2 sphericalAngle = new Vector3(Mathf.PI, 0);
		[SerializeField] private Vector2 clampScrollMouse = new Vector2(7, 50);
		[SerializeField] private Vector2 clampScrollTouch = new Vector2(10, 70);
		[SerializeField] private float radius = 35f;

		[Header("Strength")]
		[SerializeField] private float scrollMouseStrength = 0.75f;
		[SerializeField] private float scrollTouchStrength = 0.1f;

		[SerializeField] private float mouseStrength = 130;
		[SerializeField] private float touchStrength = 200;

		private Vector2 clampScroll;
		private Vector2 firstCursorPosition = Vector2.zero;
		private Vector2 sphericalAngleClick = Vector2.zero;
		private Vector2 addSphericalAngle = Vector2.zero;

		private float moveStrength = default;
		private float scrollStrength = default;
		private float lastTouchDistance = default;

		private Action SupportType = default;

		#region Unity Methods
		private void Awake()
		{
			Instance = this;

#if UNITY_ANDROID
			SupportType = DoTouch;
			moveStrength = touchStrength;
			scrollStrength = scrollTouchStrength;
			clampScroll = clampScrollTouch;
#else
			SupportType = DoClick;
			moveStrength = mouseStrength;
			scrollStrength = scrollMouseStrength;
			clampScroll = clampScrollMouse;
#endif
		}

		public void Init(Vector2 sphericalAngle, float radius, Vector2 clampScroll)
		{
			this.radius = radius;
			this.clampScroll = clampScroll;

			UpdatePosition(sphericalAngle);

			UpdateRotation();
		}

		public void GameLoop()
		{
			SupportType();
		}

		private void OnValidate()
		{
			UpdatePosition(sphericalAngle);

			UpdateRotation();
		}
		#endregion

		#region State Machine
		private void DoTouch()
		{
			if (Input.touchCount != 0)
			{
				Touch[] touches = Input.touches;

				if (touches.Length == 1)
					Move(touches[0].position, touches[0].phase == TouchPhase.Began);
				else
				{
					float currentTouchDistance = Mathf.Abs((touches[1].position - touches[0].position).magnitude);

					if (touches[0].phase != TouchPhase.Began &&
						touches[1].phase != TouchPhase.Began)
					{
						float change = currentTouchDistance - lastTouchDistance;

						Scroll(change);
					}

					lastTouchDistance = currentTouchDistance;
				}
			}
		}

		private void DoClick()
		{
			if (Input.GetMouseButton(1))
				Move(Input.mousePosition, Input.GetMouseButtonDown(1));

			Scroll(Input.mouseScrollDelta.y);
		}
		#endregion

		#region Update Transform
		public void Move(Vector2 mousePosition, bool isStarting)
		{
			if (isStarting)
			{
				firstCursorPosition = mousePosition;
				sphericalAngleClick = sphericalAngle;
			}

			addSphericalAngle = (mousePosition - firstCursorPosition) / moveStrength;

			sphericalAngle = sphericalAngleClick + addSphericalAngle;
			sphericalAngle.y = Mathf.Clamp(sphericalAngle.y, -Mathf.PI / 2 + 0.1f, Mathf.PI / 2 - 0.1f);

			UpdatePosition(sphericalAngle);

			UpdateRotation();
		}

		private void Scroll(float scroll)
		{
			if (scroll == 0) return;

			radius -= scrollStrength * scroll;
			radius = Mathf.Clamp(radius, clampScroll.x, clampScroll.y);

			UpdatePosition(sphericalAngle);
		}

		private void UpdatePosition(Vector2 sphericalAngle)
		{
			transform.position = new Vector3(
				(float)(radius * Math.Cos(sphericalAngle.y) * Math.Sin(sphericalAngle.x)),
				(float)(-radius * Math.Sin(sphericalAngle.y)),
				(float)(radius * Math.Cos(sphericalAngle.y) * Math.Cos(sphericalAngle.x)));
		}

		private void UpdateRotation()
		{
			transform.rotation = Quaternion.LookRotation(center.position - transform.position, Vector3.up);
		}
		#endregion
	}
}

