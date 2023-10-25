using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace ProjectDOOM.deprecated.V3
{
	public class BetterEnemyJob : IJobParallelFor
	{
		[ReadOnly] public NativeArray<Vector3> enemyPositions;
		[ReadOnly] public NativeArray<Vector3> enemyForwards;
		[ReadOnly] public Vector3 playerCameraPosition;
		public NativeArray<Quaternion> resultRotations;
		public NativeArray<int> resultIndices;

		public void Execute(int index)
		{
			Vector3 enemyPosition = enemyPositions[index];
			Vector3 enemyForward = enemyForwards[index];

			//float angle = BetterDetermineAngle(enemyPosition, playerCameraPosition, enemyForward);

			Quaternion rotation = Quaternion.LookRotation(playerCameraPosition - enemyPosition, Vector3.up);
			resultRotations[index] = rotation;
			//resultIndices[index] = GetRotationIndex(angle);
		}

		private int _lastRotationIndex;

		/*private int GetRotationIndex(float currentAngle)
	{
		foreach (var (min, max, index) in _angleRanges)
		{
			if (currentAngle >= min && currentAngle < max)
			{
				_lastRotationIndex = index;
				return _lastRotationIndex;
			}
		}

		return _lastRotationIndex;
	}

	private float BetterDetermineAngle(Vector3 a, Vector3 b, Vector3 forwardDirection)
	{
		Vector3 targetPosition = new Vector3(b.x, a.y, b.z);
		Vector3 targetDirection = targetPosition - a;

		return Vector3.SignedAngle(targetDirection, forwardDirection, Vector3.up);
	}*/
	}
}