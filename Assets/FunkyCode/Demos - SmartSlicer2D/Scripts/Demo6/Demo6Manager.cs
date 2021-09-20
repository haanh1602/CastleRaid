using System.Collections.Generic;
using UnityEngine;
using Utilities2D;

namespace Slicer2D.Demo
{
	public class Demo6Manager : MonoBehaviour
	{
		
		public GameObject bombPrefab;
		public GameObject bouncerPrefab;
		public Transform parent;
		private static Plane plane = new Plane(Vector3.back, new Vector3(0, 0, 0));

		void Update()
		{
			if(UnityEngine.Input.touchCount > 0)
            {
				// multi-touch (for Mobile)
				for (int i = 0; i < UnityEngine.Input.touchCount; i++)
                {
					Vector3 touchPos = GetTouchPosition3D(i);
					GameObject g = Instantiate(bombPrefab, touchPos, Quaternion.identity) as GameObject;
					//g.transform.position = touchPos;
					g.transform.parent = transform;
				}
            } else
            {
				// single-touch (for PC)
				if (UnityEngine.Input.GetMouseButton(0))
				{
					Vector3 mousePos = GetMousePosition3D();
					GameObject g = Instantiate(bombPrefab) as GameObject;
					g.transform.position = mousePos;
					g.transform.parent = transform;
				}
			}
        }

		public static Vector2D GetMousePosition()
		{
			return (new Vector2D(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition)));
		}

		public static Vector3 GetMousePosition3D()
		{
			Vector3 res = new Vector3();
			Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);

			float enter;
			if (plane.Raycast(ray, out enter))
			{
				res = ray.GetPoint(enter);
			}
			return res;
		}

		public static Vector3 GetTouchPosition3D (int index)
        {
			Vector3 res = new Vector3();
			Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.GetTouch(index).position);
			float enter;
			if (plane.Raycast(ray, out enter))
			{
				res = ray.GetPoint(enter);
			}
			return res;
        }
	}
}