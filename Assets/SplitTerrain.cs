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
        }

        public void Start()
        {
            visuals.Initialize(gameObject);

            linearControllerObject.SetController(gameObject, input, visuals, sliceLayer, eventHandler);
            linearControllerObject.Initialize();
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
            //Draw();
        }

        public void Draw()
        {
            if (visuals.drawSlicer == false)
            {
                return;
            }
            linearControllerObject.Draw(transform);

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
