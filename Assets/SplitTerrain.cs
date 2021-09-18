using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Slicer2D
{
    public class SplitTerrain : MonoBehaviour
    {
        PolygonCollider2D polygonCollider2D;

        public GameObject checkPoint;

        public enum SliceType { Linear };
        //public static Color[] slicerColors = { Color.black, Color.green, Color.yellow, Color.red, new Color(1f, 0.25f, 0.125f) };

        public SliceType sliceType = SliceType.Linear;

        // Slicer2DController.Get()
        private static SplitTerrain instance;

        // Slicer Layer
        public Layer sliceLayer = Layer.Create();

        // Slicer Visuals
        public Visuals visuals = new Visuals();

        // Input
        public InputController input = new InputController();

        // Input Events Handler
        public ControllerEventHandling eventHandler = new ControllerEventHandling();

        // Different Slicer Type Managers
        public Controller.Linear.Controller linearControllerObject = new Controller.Linear.Controller();

        public bool UIBlocking = true;

        public void AddResultEvent(ControllerEventHandling.ResultEvent e)
        {
            eventHandler.sliceResultEvent += e;
        }

        public void Awake()
        {
            instance = this;
            //polygonCollider2D = GetComponent<PolygonCollider2D>();
        }

        public void Start()
        {
            visuals.Initialize(gameObject);

            linearControllerObject.SetController(gameObject, input, visuals, sliceLayer, eventHandler);
            linearControllerObject.Initialize();

            Split();
        }

        public bool BlockedByUI()
        {
            if (UIBlocking == false)
            {
                return (false);
            }

            if (EventSystem.current == null)
            {
                return (false);
            }

            if (EventSystem.current.IsPointerOverGameObject(0))
            {
                return (true);
            }

            if (EventSystem.current.IsPointerOverGameObject(-1))
            {
                return (true);
            }

            if (UnityEngine.Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(UnityEngine.Input.GetTouch(0).fingerId))
            {
                return (true);
            }

            return (false);
        }

        public void LateUpdate()
        {
            if (BlockedByUI() == false)
            {
                InputController.zPosition = visuals.zPosition;
                input.Update();
            }
            Vector2 pos = input.GetInputPosition();
            linearControllerObject.Update();
            Draw();
        }

        public void Draw()
        {
            if (visuals.drawSlicer == false)
            {
                return;
            }
            linearControllerObject.Draw(transform);

        }

        public void Split()
        {
            List<GameObject> childs = new List<GameObject>(0);
            int childCount = gameObject.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                GameObject child = gameObject.transform.GetChild(i).gameObject;
                if (child.name.Contains("Terrain"))
                {
                    childs.Add(child);
                }
            }
            Debug.Log(childs.Count.ToString());
            foreach (GameObject child in childs)
            {
                PolygonCollider2D polygonCollider2D = child.GetComponent<PolygonCollider2D>();
                if (!polygonCollider2D || polygonCollider2D.points.Length <= 0)
                {
                    continue;
                }
                Vector2[] vertices = polygonCollider2D.points;
                List<Vector2> worldVertices = new List<Vector2>();
                /*                for (int i = 0; i < vertices.Length; i++)
                                {
                                    vertices[i] = 
                                    //worldVertices.Add(Camera.main.WorldToScreenPoint(vertex));
                                }*/
                Vector2 minXVertex = vertices[0];
                Vector2 maxXVertex = vertices[0];
                Vector2 minYVertex = vertices[0];
                Vector2 maxYVertex = vertices[0];

                // Get minX, maxX, minY, maxY 
                foreach (Vector2 vertex in vertices)
                {
                    if (vertex.x < minXVertex.x)
                    {
                        minXVertex = vertex;
                    }
                    if (vertex.x > maxXVertex.x)
                    {
                        maxXVertex = vertex;
                    }
                    if (vertex.y < minYVertex.y)
                    {
                        minYVertex = vertex;
                    }
                    if (vertex.y > maxYVertex.y)
                    {
                        maxYVertex = vertex;
                    }
                }
                if (Get2DPoint(child, maxYVertex).y - Get2DPoint(child, minYVertex).y < 8.0f)
                {
                    Debug.Log((maxYVertex.y - minYVertex.y).ToString());
                    continue;
                }
                else
                {
                    linearControllerObject.SplitBigTerrain(Get2DPoint(child, minXVertex), Get2DPoint(child, maxXVertex),
                        Get2DPoint(child, minYVertex), Get2DPoint(child, maxYVertex));
                }
            }
            this.enabled = false;
        }

        public void SetSliceType(int type)
        {
            sliceType = (SliceType)type;
        }

        public void SetLayerType(int type)
        {
            if (type == 0)
            {
                sliceLayer.SetLayerType((LayerType)0);
            }
            else
            {
                sliceLayer.SetLayerType((LayerType)1);
                sliceLayer.DisableLayers();
                sliceLayer.SetLayer(type - 1, true);
            }
        }

        public static Vector2 Get2DPoint(GameObject gameObject, Vector2 localPoint)
        {
            Vector3 offset = gameObject.transform.position;
            return new Vector2(localPoint.x * gameObject.transform.localScale.x + offset.x, localPoint.y * gameObject.transform.localScale.y + offset.y);
        }

        public static Vector3 Get2DPointAsVector3(GameObject gameObject, Vector2 localPoint)
        {
            Vector3 offset = gameObject.transform.position;
            return new Vector3(localPoint.x * gameObject.transform.localScale.x + offset.x, localPoint.y * gameObject.transform.localScale.y + offset.y, offset.z);
        }

        /*        public void SetSlicerColor(int colorInt)
                {
                    visuals.slicerColor = slicerColors[colorInt];
                }*/

        public static SplitTerrain Get()
        {
            return (instance);
        }
    }
}
