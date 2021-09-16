using System.Collections.Generic;
using UnityEngine;
using Utilities2D;

namespace Slicer2D.Demo {

	/*	public class Demo6Manager : MonoBehaviour {
			public GameObject bombPrefab;
			public GameObject bouncerPrefab;
			public Transform parent;

			void Update () {
				Vector2D pos = GetMousePosition ();

				if (UnityEngine.Input.GetMouseButtonDown (0)) {
					GameObject g = Instantiate (bombPrefab) as GameObject;
					g.transform.position = new Vector3 ((float)pos.x, (float)pos.y, -4.75f);
					g.transform.parent = transform;
				}

				if (UnityEngine.Input.GetMouseButtonDown (1)) {
					GameObject g = Instantiate (bouncerPrefab) as GameObject;
					g.transform.position = new Vector3 ((float)pos.x, (float)pos.y, -4.75f);
					g.transform.parent = transform;
				}
			}

			public static Vector2D GetMousePosition() {
				return(new Vector2D (Camera.main.ScreenToWorldPoint (UnityEngine.Input.mousePosition)));
			}
		}*/

	public class Demo6Manager : MonoBehaviour
	{
		public GameObject bombPrefab;
		public GameObject bouncerPrefab;
		public Transform parent;
		private static Plane plane = new Plane(Vector3.back, new Vector3(0, 0, 0));

		void Update()
		{
			//Vector2D pos = GetMousePosition ();
			if (UnityEngine.Input.GetMouseButton(0))
			{
				//Debug.Log(UnityEngine.Input.mousePosition.x + ", " + UnityEngine.Input.mousePosition.y + ", " + UnityEngine.Input.mousePosition.z);
				//Debug.Log(pos.x + ", " + pos.y);
				Vector3 mousePos = GetMousePosition3D();
				//Debug.Log(mousePos.x + ", " + mousePos.y + ", " + mousePos.z);
				GameObject g = Instantiate(bombPrefab) as GameObject;
				g.transform.position = mousePos;
				//new Vector3 ((float)pos.x, (float)pos.y, -4.75f);
				g.transform.parent = transform;
			}

			/*			if (UnityEngine.Input.GetMouseButtonDown (1)) {
							GameObject g = Instantiate (bouncerPrefab) as GameObject;
							g.transform.position = new Vector3 ((float)pos.x, (float)pos.y, -4.75f);
							g.transform.parent = transform;
						}*/
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
	}
}