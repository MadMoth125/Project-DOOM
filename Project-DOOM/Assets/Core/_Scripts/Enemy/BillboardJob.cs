using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;

public struct BillboardJob : IJob
{
	// private Quaternion _targetRotation;
	private Quaternion _rotateTo; // This is the transform that will be referenced for rotation
	private float _angleOffset; // This is the angle offset that will be added to the rotation

	private NativeArray<Quaternion> _rotationResult; // This is the rotation result that will be applied to the rotator
	
	public BillboardJob(Quaternion rotateTo, float angleOffset, NativeArray<Quaternion> rotationResult)
	{
		_rotateTo = rotateTo;
		_angleOffset = angleOffset;
		_rotationResult = rotationResult;
	}
	
	public void Execute()
	{
		RotateToView();
	}
	
	private void RotateToView()
	{
		_rotationResult[0] = Quaternion.Euler(0f, _rotateTo.eulerAngles.y + _angleOffset, 0f);
	}
}