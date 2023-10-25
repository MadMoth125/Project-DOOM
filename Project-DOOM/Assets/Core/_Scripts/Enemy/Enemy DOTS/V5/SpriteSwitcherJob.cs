using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace ProjectDOOM.deprecated.DOTS
{
	public struct SpriteSwitcherJob : IJob
	{
		public NativeArray<int> rotationIndex;
		
		[ReadOnly]
		public float3 cameraPosition;
		[ReadOnly]
		public float3 transformPosition;
		[ReadOnly]
		public float3 transformForward;
		[ReadOnly]
		public NativeArray<float3> angles;
		
		/*public SpriteSwitcherJob(
			float3 transformPosition,
			float3 transformForward,
			float3 cameraPosition,
			NativeArray<float3> angles)
		{
			_cameraPosition = cameraPosition;
			_transformPosition = transformPosition;
			_transformForward = transformForward;
			_angles = angles;
			
			rotationIndex = new NativeArray<int>(1, Allocator.Temp);
		}*/
		
		public void Execute()
		{
			rotationIndex[0] = GetRotationIndex(
				BetterDetermineAngle(
					transformPosition,
					transformForward,
					cameraPosition
					), angles);
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
        private static float Angle(float3 from, float3 to)
        {
        	float num = math.sqrt(math.lengthsq(from) * math.lengthsq(to));
        	return num < 1.0000000036274937E-15 ? 0.0f : math.acos(math.clamp(math.dot(from, to) / num, -1f, 1f)) * 57.29578f;
        }
        
        [BurstCompile]
        private static float SignedAngle(float3 from, float3 to, float3 axis)
        {
        	float num1 = Angle(from, to);
        	float num2 = (from.y * to.z - from.z * to.y);
        	float num3 = (from.z * to.x - from.x * to.z);
        	float num4 = (from.x * to.y - from.y * to.x);
        	float num5 = Sign(axis.x * num2 + axis.y * num3 + axis.z * num4);
        	return num1 * num5;
        }
        
        [BurstCompile]
        private static float Sign(float f)
        {
        	return f >= 0.0 ? 1f : -1f;
        }
    
        #endregion
	}
}