using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doom.DeadScripts
{
	public class DoomCamera : MonoBehaviour
	{
		private Camera _camera;
		public Camera CameraComponent => _camera;
	
		private void Awake()
		{
			// lazy load the camera reference
			_camera ??= GetComponent<Camera>();
			_camera ??= GetComponentInChildren<Camera>();
			_camera ??= Camera.main;
		}

		private void OnEnable()
		{
		
		}

		private void OnDisable()
		{
		
		}

		private void Start()
		{
		
		}

		private void Update()
		{
		
		}
	}
}
