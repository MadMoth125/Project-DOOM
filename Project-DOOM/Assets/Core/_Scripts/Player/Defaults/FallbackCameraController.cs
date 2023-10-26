using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallbackCameraController : MonoBehaviour, ICameraController
{
	public Camera CameraComponent => _camera;
	private Camera _camera;
	
	public ICharacterController OwningController { get; }
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