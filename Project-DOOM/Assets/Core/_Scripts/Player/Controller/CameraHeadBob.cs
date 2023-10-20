using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// test comment
public class CameraHeadBob
{
	private Vector2 _movementDirection;

	public void SetMovementDirection(Vector2 direction)
	{
		_movementDirection = Vector2.Lerp(_movementDirection, direction.normalized, Time.deltaTime * 10f);
	}
	
	public void HandleCameraBobbing(Transform target, float frequency = 5f, float strength = 1f)
	{
		Vector3 xSway = Vector3.right * (Mathf.Sin(Time.time * frequency) * strength);
		Vector3 ySway = Vector3.up * (Mathf.Sin(Time.time * 2f * frequency) * strength);

		target.localPosition = Vector3.Lerp(Vector3.zero, xSway + ySway, _movementDirection.magnitude);
	}
}