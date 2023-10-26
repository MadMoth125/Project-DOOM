using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[SelectionBase]
public class DoomCamera : MonoBehaviour, ICameraController
{
	public ICharacterController OwningController { get; }
	
	public Camera CameraComponent => _camera;
	private Camera _camera;
	
	public Transform CameraTransform => transform;
	public GameObject CameraGameObject => gameObject;
	
	[Header("Camera Settings")]
	public float minCameraPitch = -89f;
	public float maxCameraPitch = 89f;
	[Space]
	public float rotationSpeed = 10f;
	
	public Vector3 PlanarDirection => _planarDirection;
	private Vector3 _planarDirection = Vector3.forward;
	
	public Transform TargetTransform => _targetTransform;
	private Transform _targetTransform;
	
	private Vector3 _lookInputVector = Vector3.zero;
	private float _targetVerticalAngle = 0f;

	private DoOnce _initialRotationDoOnce = new DoOnce();
	
	private void OnValidate()
	{
		// Calling in editor to ensure that the min/max values are within the range of -89 to 89 degrees.
		KeepMinMaxWithinRange();
	}

	private void Awake()
	{
		_camera = gameObject.SearchForComponent<Camera>();
		_camera ??= Camera.main; // fallback to main camera if no camera is found on this object or its children.

		// initializing the camera's rotation to face the
		// same direction that it was instantiated in.
		_planarDirection = transform.rotation * Vector3.forward;
	}

	public void RotateCamera(Vector3 deltaRotation)
	{
		_lookInputVector = deltaRotation;

		// Process rotation input.
		// Most of this is lifted from the ExamplePlayerController in KinematicCharacterController
		Quaternion rotationFromInput = Quaternion.Euler(_targetTransform.up * (_lookInputVector.x * rotationSpeed));
		_planarDirection = rotationFromInput * _planarDirection;
		_planarDirection = Vector3.Cross(_targetTransform.up, Vector3.Cross(_planarDirection, _targetTransform.up));
		
		HandlePosition();
		ApplyCameraRotation();

		void ApplyCameraRotation()
		{
			KeepMinMaxWithinRange();
			
			// clamping the camera's pitch to the min/max values.
			_targetVerticalAngle -= _lookInputVector.y * rotationSpeed;
			_targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle, minCameraPitch, maxCameraPitch);
			
			Quaternion verticalRot = Quaternion.Euler(_targetVerticalAngle, 0, 0);
			
			Quaternion planarRot = Quaternion.LookRotation(_planarDirection, _targetTransform.up);
			Quaternion targetRotation = planarRot * verticalRot;
			
			// Apply rotation
			transform.rotation = targetRotation;
		}
	}
	
	public void SetFollowTransform(Transform target)
	{
		_targetTransform = target;
	}
	
	/// <summary>
	/// Updates the camera's position to match the target Transform's position.
	/// </summary>
	private void HandlePosition()
	{
		transform.position = _targetTransform.position;
	}
	
	/// <summary>
	/// Ensures that the min/max camera pitch values are within the range of -89 to 89 degrees.
	/// This only affects the limit values, not the actual camera pitch.
	/// </summary>
	private void KeepMinMaxWithinRange()
	{
		minCameraPitch = Mathf.Clamp(minCameraPitch, -89.99f, 0f);
		maxCameraPitch = Mathf.Clamp(maxCameraPitch, 0, 89.99f);
	}
}