using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraController
{
	public Camera CameraComponent { get; }
	public ICharacterController OwningController { get; }
	
	public Transform CameraTransform { get; }
	public GameObject CameraGameObject { get; }
	
	public void RotateCamera(Vector3 deltaRotation);
	public void SetFollowTransform(Transform target);
}