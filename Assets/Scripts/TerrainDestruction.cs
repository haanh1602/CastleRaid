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
		private Vector3 preMousePosition = new Vector3();

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
						/*GameObject gPre = Instantiate(bombPrefab, ConvertTo3D(preTouches[i].position), Quaternion.identity) as GameObject;
						//g.transform.position = touchPos;
						gPre.transform.parent = transform;*/
						if (Vector3.Distance(ConvertTo3D(preTouches[i].position), ConvertTo3D(touches[i].position)) >= 5.5f)
                        {
							List<Vector3> points = LinearCutPos(ConvertTo3D(preTouches[i].position), ConvertTo3D(touches[i].position), 2.75f);
							LinearCut linearCutLine = LinearCut.Create(new Pair2(points[0], points[1]), 5.5f);
							Slicing.LinearCutSliceAll(linearCutLine, Layer.Create());
						}
						GameObject g = Instantiate(bombPrefab, ConvertTo3D(touches[i].position), Quaternion.identity) as GameObject;
						g.transform.parent = transform;
						//g.transform.position = touchPos;

					}
				}
			}
			else
			{
				// single-touch (for PC)
				if (UnityEngine.Input.GetMouseButtonDown(0))
                {
					preMousePosition = ConvertTo3D(UnityEngine.Input.mousePosition);
                }
				if (UnityEngine.Input.GetMouseButton(0))
				{
					if (Vector3.Distance(preMousePosition, GetMousePosition3D()) >= 5.5f)
					{
						List<Vector3> points = LinearCutPos(preMousePosition, GetMousePosition3D(), 2.75f);
						LinearCut linearCutLine = LinearCut.Create(new Pair2(points[0], points[1]), 5.5f);
						Slicing.LinearCutSliceAll(linearCutLine, Layer.Create());

					}
						/*GameObject gPre = Instantiate(bombPrefab, preMousePosition, Quaternion.identity) as GameObject;
						//g.transform.position = touchPos;
						gPre.transform.parent = transform;*/
						GameObject g = Instantiate(bombPrefab, GetMousePosition3D(), Quaternion.identity) as GameObject;
						//g.transform.position = touchPos;
						g.transform.parent = transform;
					/*Vector3 mousePos = GetMousePosition3D();
					List<Vector3> points = GetPointBetweenInclude(preMousePosition, mousePos, 6.0f);
					foreach (Vector3 point in points)
					{
						GameObject g = Instantiate(bombPrefab, point, Quaternion.identity) as GameObject;
						//g.transform.position = touchPos;
						g.transform.parent = transform;
					}*/
					preMousePosition = GetMousePosition3D();
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

		public List<Vector3> LinearCutPos(Vector3 start, Vector3 end, float explodeRadius)
        {
			List<Vector3> res = new List<Vector3>();
			Vector3 direct = end - start;
			Vector3 cutStartPos = start + direct * explodeRadius / Vector3.Distance(start, end);
			Vector3 cutEndPos = end + (-direct) * explodeRadius / Vector3.Distance(start, end);
			res.Add(cutStartPos);
			res.Add(cutEndPos);
			return res;
        }

		void Explode()
		{
			Vector2D pos = new Vector2D(transform.position);

			Polygon2D.defaultCircleVerticesCount = 15;

			Polygon2D slicePolygon = Polygon2D.Create(Polygon2D.PolygonType.Circle, 4f);
			Polygon2D slicePolygonDestroy = Polygon2D.Create(Polygon2D.PolygonType.Circle, 6f);

			slicePolygon = slicePolygon.ToOffset(pos);
			slicePolygonDestroy = slicePolygonDestroy.ToOffset(pos);

			foreach (Sliceable2D id in Sliceable2D.GetListCopy())
			{
				Slice2D result = Slicer2D.API.PolygonSlice(id.shape.GetLocal().ToWorldSpace(id.transform), slicePolygon);
				if (result.GetPolygons().Count > 0)
				{
					foreach (Polygon2D p in new List<Polygon2D>(result.GetPolygons()))
					{
						if (slicePolygonDestroy.PolyInPoly(p) == true)
						{
							result.GetPolygons().Remove(p);
						}
					}

					if (result.GetPolygons().Count > 0)
					{
						id.PerformResult(result.GetPolygons(), new Slice2D());
					}
					else
					{
						// Polygon is Destroyed!!!
						Destroy(id.gameObject);
					}
				}
			}

			//Destroy(gameObject);

			Polygon2D.defaultCircleVerticesCount = 25;
		}
	}
}