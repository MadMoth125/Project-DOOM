using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile(CompileSynchronously = true)]
public struct EnemyJobDOTS : IJobParallelFor
{
	[ReadOnly]
	public float3 cameraPosition;
	[ReadOnly]
	public float cameraRotationY;
	
	[ReadOnly]
	public NativeArray<float3> enemyPositions;
	[ReadOnly]
	public NativeArray<float3> enemyForwardDirections;
	[ReadOnly]
	public NativeArray<float3> anglesArray;
	
	[NativeDisableParallelForRestriction]
	public NativeArray<int> returnedRotationIndex;
	[NativeDisableParallelForRestriction]
	public NativeArray<Quaternion> returnedRotation;
	
	public void Execute(int index)
	{
		returnedRotationIndex[index] = GetRotationIndex(
			BetterDetermineAngle(
				enemyPositions[index],
				cameraPosition,
				enemyForwardDirections[index]
				),
			anglesArray
			);

		returnedRotation[index] = Quaternion.Euler(0f, cameraRotationY, 0f);
	}
	
	private int GetRotationIndex(float currentAngle, NativeArray<float3> angles)
	{
		for (int i = 0; i < angles.Length; i++)
		{
			// x = min, y = max, z = index
			if (angles[i].z == 0f) // special case for front
			{
				if (currentAngle > angles[i].x && currentAngle < angles[i].y)
				{
					return (int) angles[i].z;
				}
			}
			else if (angles[i].z == 4f) // special case for back
			{
				if (currentAngle <= angles[i].x || currentAngle >= angles[i].y)
				{
					return (int) angles[i].z;
				}
			}
			else // everything else
			if (currentAngle >= angles[i].x && currentAngle < angles[i].y)
			{
				return (int) angles[i].z;
			}
		}

		return 0;
	}

	private float BetterDetermineAngle(float3 a, float3 b, float3 aDir)
	{
		float3 targetPosition = new float3(b.x, a.y, b.z);
		float3 targetDirection = targetPosition - a;

		return SignedAngle(targetDirection, aDir, new float3(0f, 1f, 0f));
	}

	#region Calculating a better SignedAngle

	[BurstCompile]
	private float Angle(float3 from, float3 to)
	{
		// float num = (float) Math.Sqrt((double) from.sqrMagnitude * (double) to.sqrMagnitude);
		float num = math.sqrt(math.lengthsq(from) * math.lengthsq(to));
		// return num < 1.0000000036274937E-15 ? 0.0f : (float) Math.Acos((double) Mathf.Clamp(Vector3.Dot(from, to) / num, -1f, 1f)) * 57.29578f;
		return num < 1.0000000036274937E-15 ? 0.0f : math.acos(math.clamp(math.dot(from, to) / num, -1f, 1f)) * 57.29578f;
	}
	
	[BurstCompile]
	private float SignedAngle(float3 from, float3 to, float3 axis)
	{
		float num1 = Angle(from, to);
		float num2 = (from.y * to.z - from.z * to.y);
		float num3 = (from.z * to.x - from.x * to.z);
		float num4 = (from.x * to.y - from.y * to.x);
		float num5 = Sign(axis.x * num2 + axis.y * num3 + axis.z * num4);
		return num1 * num5;
	}
	
	[BurstCompile]
	private float Sign(float f)
	{
		return f >= 0.0 ? 1f : -1f;
	}

	#endregion
}