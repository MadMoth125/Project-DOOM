using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace EnemySpriteHandling
{
	[BurstCompile]
	public struct EnemySpriteSelectorJob : IJobParallelFor
	{
		[ReadOnly]
		public float3 playerPosition;
		[ReadOnly]
		public float playerRotationY;
		
		[ReadOnly]
		public NativeArray<float3> directions;
		[ReadOnly]
		public NativeArray<float3> enemyPositions;
		[ReadOnly]
		public NativeArray<float3> enemyDirections;
		
		[WriteOnly]
		public NativeArray<int> rotationIndex;
		[WriteOnly]
		public NativeArray<float> rotationY;
		
		public void Execute(int index)
		{
			rotationIndex[index] = GetRotationIndex(
				BetterDetermineAngle(
					enemyPositions[index],
					enemyDirections[index],
					playerPosition
					), directions);
			
			rotationY[index] = playerRotationY;
		}
		
		[BurstCompile]
		private int GetRotationIndex(float currentAngle, NativeArray<float3> angleChart)
		{
			for (int i = 0; i < angleChart.Length; i++)
			{
				// x = min, y = max, z = index
				if ((int) angleChart[i].z == 0) // special case for front
				{
					if (currentAngle > angleChart[i].x && currentAngle < angleChart[i].y)
					{
						return (int) angleChart[i].z;
					}
				}
				else if ((int) angleChart[i].z == 4) // special case for back
				{
					if (currentAngle <= angleChart[i].x || currentAngle >= angleChart[i].y)
					{
						return (int) angleChart[i].z;
					}
				}
				else // everything else
				if (currentAngle >= angleChart[i].x && currentAngle < angleChart[i].y)
				{
					return (int) angleChart[i].z;
				}
			}
	
			return 0;
		}
	
		[BurstCompile]
		private float BetterDetermineAngle(float3 a, float3 aDir, float3 b)
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
			// return num < 1.0000000036274937E-15 ? 0.0f : (float) Math.Acos((double) Mathf.Clamp(Vector3.Dot(from, to) / num, -1f, 1f)) * 57.29578f;
			
			float num = math.sqrt(math.lengthsq(from) * math.lengthsq(to));
			return num < 1.0000000036274937E-15 ? 0.0f : math.acos(math.clamp(math.dot(from, to) / num, -1f, 1f)) * 57.29578f;
		}
		
		[BurstCompile]
		private float SignedAngle(float3 from, float3 to, float3 axis)
		{
			float num1 = Angle(from, to);
			
			// float num2 = (from.y * to.z - from.z * to.y);
			// float num3 = (from.z * to.x - from.x * to.z);
			// float num4 = (from.x * to.y - from.y * to.x);
			// float num5 = Sign(axis.x * num2 + axis.y * num3 + axis.z * num4);
			
			float3 num234 = new float3(
				from.y * to.z - from.z * to.y,
				from.z * to.x - from.x * to.z,
				from.x * to.y - from.y * to.x);
			float num5 = Sign(axis.x * num234.x + axis.y * num234.y + axis.z * num234.z);
			
			return num1 * num5;
		}
		
		[BurstCompile]
		private float Sign(float f) => f >= 0.0 ? 1f : -1f;
	
		#endregion
	}
}