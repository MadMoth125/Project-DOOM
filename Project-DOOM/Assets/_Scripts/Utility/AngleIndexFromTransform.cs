using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
	public class AngleIndexFromTransform
	{
		private int _lastRotationIndex = 0;
	
		public int GetRotationIndex(float currentAngle)
		{
			// front
			if (currentAngle is > -22.5f and < 22.5f) return _lastRotationIndex = 0;
		
			if (currentAngle is >= 22.5f and < 67.5f) return _lastRotationIndex = 7;
		
			if (currentAngle is >= 67.5f and < 112.5f) return _lastRotationIndex = 6;
		
			if (currentAngle is >= 112.5f and < 157.5f) return _lastRotationIndex = 5;
		
			// back
			if (currentAngle is <= -157.5f or >= 157.5f) return _lastRotationIndex = 4;
		
			if (currentAngle is >= -157.5f and < -112.5f) return _lastRotationIndex = 3;
		
			if (currentAngle is >= -112.5f and < -67.5f) return _lastRotationIndex = 2;
		
			if (currentAngle is >= -67.5f and < -22.5f) return _lastRotationIndex = 1;

			return _lastRotationIndex;
		}
	
		public void DetermineAngle(Transform a, Transform b, out float angle)
		{
			DetermineVectors(a, b, out Vector3 targetPosition, out Vector3 targetDirection);
		
			angle = Vector3.SignedAngle(targetDirection, a.forward, Vector3.up);
		}
	
		public void DetermineVectors(Transform a, Transform b, out Vector3 targetPosition, out Vector3 targetDirection)
		{
			targetPosition = new Vector3(b.position.x, a.position.y, b.position.z);
			targetDirection = targetPosition - a.position;
		}
	}
}