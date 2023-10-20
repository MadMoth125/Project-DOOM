using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class DoomCameraController : MonoBehaviour
{
	public float rotationSpeed = 10f;
	
	// public float rotationSharpness = 10f;
	
	public Camera CameraReference => _camera;
	private Camera _camera;
	
	private Vector3 _lookInputVector = Vector3.zero;
	
	public Vector3 PlanarDirection => _planarDirection;
	private Vector3 _planarDirection = Vector3.forward;
	
	public Transform TargetTransform => _targetTransform;
	private Transform _targetTransform;
	
	private float _targetVerticalAngle = 0f;
	
	private void Awake()
	{
		_camera ??= GetComponentInChildren<Camera>();
		_camera ??= Camera.main;
	}

	public void HandleCameraInput(Vector2 direction, Transform target)
	{
		_targetTransform = target;
		_lookInputVector = direction;
		
		// Process rotation input
		Quaternion rotationFromInput = Quaternion.Euler(_targetTransform.up * (_lookInputVector.x * rotationSpeed));
		_planarDirection = rotationFromInput * _planarDirection;
		_planarDirection = Vector3.Cross(_targetTransform.up, Vector3.Cross(_planarDirection, _targetTransform.up));
		
		HandlePosition();
		HandleRotation();
	}

	private void HandleRotation()
	{
		Quaternion planarRot = Quaternion.LookRotation(_planarDirection, _targetTransform.up);
		
		_targetVerticalAngle -= _lookInputVector.y * rotationSpeed;
		_targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle, -89, 89);
		
		Quaternion verticalRot = Quaternion.Euler(_targetVerticalAngle, 0, 0);
		// Quaternion targetRotation = Quaternion.Slerp(transform.rotation, planarRot * verticalRot, 1f - Mathf.Exp(-rotationSharpness * Time.deltaTime));
		Quaternion targetRotation = planarRot * verticalRot;

		// Apply transforms
		transform.rotation = targetRotation;
	}
	
	private void HandlePosition()
	{
		transform.position = _targetTransform.position;
	}
}