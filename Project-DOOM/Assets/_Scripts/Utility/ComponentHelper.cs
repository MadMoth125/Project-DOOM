using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
	public static class ComponentHelper
	{
		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			T component = gameObject.GetComponent<T>();

			return component == null ? gameObject.AddComponent<T>() : component;
		}
	
		public static T SearchForComponent<T>(this GameObject gameObject) where T : Component
		{
			T component = gameObject.GetComponent<T>();

			return component == null ? gameObject.GetComponentInChildren<T>() : component;
		}
	}
}