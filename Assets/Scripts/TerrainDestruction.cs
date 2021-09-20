using System.Collections.Generic;
using UnityEngine;
using Utilities2D;

namespace Slicer2D.Demo
{
	public class TerrainDestruction : MonoBehaviour
	{
		public GameObject bombPrefab;
		public GameObject bouncerPrefab;
		public Transform parent;
		private static Plane plane = new Plane(Vector3.back, new Vector3(0, 0, 0));
		private List<Touch> preTouches = new List<Touch>();

		void Update()
		{
			if (UnityEngine.Input.touchCount > 0)
			{
				// multi-touch (for Mobile)
				Touch[] touches = UnityEngine.Input.touches;
				for (int i = 0; i < UnityEngine.Input.touchCount; i++)
				{
					if (i >= preTouches.Count)
                    {
						List<Vector3> points = new List<Vector3>();
						points.Add(ConvertTo3D(touches[i].position));
						foreach (Vector3 point in points)
						{
							GameObject g = Instantiate(bombPrefab, point, Quaternion.identity) as GameObject;
							//g.transform.position = touchPos;
							g.transform.parent = transform;
						}
					} else
                    {
						List<Vector3> points = GetPointBetweenInclude(ConvertTo3D(preTouches[i].position), ConvertTo3D(touches[i].position), 6.0f);
						//Vector3 touchPos = GetTouchPosition3D(i);
						foreach (Vector3 point in points)
						{
							GameObject g = Instantiate(bombPrefab, point, Quaternion.identity) as GameObject;
							//g.transform.position = touchPos;
							g.transform.parent = transform;
						}
					}
				}
			}
			else
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
			preTouches.Clear();
			preTouches.AddRange(UnityEngine.Input.touches);
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

		public static Vector3 GetTouchPosition3D(int index)
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

		public Vector3 ConvertTo3D(Vector3 origin)
        {
			Vector3 res = new Vector3();
			Ray ray = Camera.main.ScreenPointToRay(origin);
			float enter;
			if (plane.Raycast(ray, out enter))
			{
				res = ray.GetPoint(enter);
			}
			return res;
        }

		public List<Vector3> GetPointBetweenInclude(Vector3 start, Vector3 end, float space)
        {
			List<Vector3> res = new List<Vector3>();
			if (start == null)
            {
				res.Add(end);
				return res;
            }
			float distance = Vector3.Distance(start, end);
			Vector3 to = end - start;
			float count = 0.0f;
			while (count < distance)
            {
				Vector3 point = start + to * count / distance;
				res.Add(point);
				count += space;
            }
			res.Add(end);
			return res;
        }
	}
}