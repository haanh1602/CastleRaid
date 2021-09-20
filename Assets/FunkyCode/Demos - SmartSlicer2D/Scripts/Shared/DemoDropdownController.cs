﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Slicer2D.Demo {
	public class DemoDropdownController : MonoBehaviour {
		public enum DropDownTypes {SlicerType, LayerType};
		public DropDownTypes type;
		
		PolygonCutter controller;
		Dropdown dropdown;
		
		void Start () {
			dropdown = GetComponent<Dropdown>();
			controller = PolygonCutter.Get();
		}
		
		void Update () {
			switch (type) {
				case DropDownTypes.LayerType:
					controller.SetLayerType(dropdown.value);
					controller.SetSlicerColor(dropdown.value);
					break;

				case DropDownTypes.SlicerType:
					controller.SetSliceType(dropdown.value);
					break;
			}
		}
	}
}