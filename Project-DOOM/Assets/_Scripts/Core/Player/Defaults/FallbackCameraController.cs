using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class FallbackCameraController : MonoBehaviour, ICameraController
{
	public ICharacterController OwningController { get; }
	
	public Camera CameraComponent => _camera;
	private Camera _camera;
	
	public Transform CameraTransform => transform;
	public GameObject CameraGameObject => gameObject;
	
	private void Awake()
	{
		_camera = gameObject.SearchForComponent<Camera>();
	}
	
	public void RotateCamera(Vector3 deltaRotation)
	{
		
	}

	public void SetFollowTransform(Transform target)
	{
		
	}
}