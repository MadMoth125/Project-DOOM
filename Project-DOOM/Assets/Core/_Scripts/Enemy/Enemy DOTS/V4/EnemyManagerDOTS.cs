using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class EnemyManagerDOTS : MonoBehaviour
{
	public static EnemyManagerDOTS Instance { get; private set; }
	
	private readonly List<EnemyDOTS> _enemyList = new List<EnemyDOTS>();

	public bool useUnityJobs = true;
	
	// saving the angles for changing sprites
	private NativeArray<float3> _newNativeAngleArray;
	
	// should only be called once
	private void InitNativeArray()
	{
		_newNativeAngleArray = new NativeArray<float3>(_angleRanges.Length, Allocator.Persistent);
		
		for (int j = 0; j < _angleRanges.Length; j++)
		{
			_newNativeAngleArray[j] = new float3(_angleRanges[j].min, _angleRanges[j].max, _angleRanges[j].index);
		}
	}
	
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		
		InitNativeArray();
	}

	private void InitTransformArrays()
	{
		
	}

	private NativeArray<float3> _enemyPositions;
	private NativeArray<float3> _enemyForwardDirections;
	
	private NativeArray<int> _returnedRotationIndex;
	private NativeArray<Quaternion> _returnedRotation;
	
	private void Update()
	{
		Vector3 cameraPosition = Camera.main.transform.position;
		Vector3 cameraRotation = Camera.main.transform.rotation.eulerAngles;
		
		if (useUnityJobs)
		{
			_enemyPositions = new NativeArray<float3>(_enemyList.Count, Allocator.TempJob);
			_enemyForwardDirections = new NativeArray<float3>(_enemyList.Count, Allocator.TempJob);
			
			_returnedRotationIndex = new NativeArray<int>(_enemyList.Count, Allocator.TempJob);
			_returnedRotation = new NativeArray<Quaternion>(_enemyList.Count, Allocator.TempJob);
			
			for (int i = 0; i < _enemyList.Count; i++)
			{
				_enemyPositions[i] = new float3(
					_enemyList[i].transform.position.x,
					_enemyList[i].transform.position.y,
					_enemyList[i].transform.position.z
					);
				_enemyForwardDirections[i] = new float3(
					_enemyList[i].transform.forward.x,
					_enemyList[i].transform.forward.y,
					_enemyList[i].transform.forward.z
					);
			}

			EnemyJobDOTS job = new EnemyJobDOTS
			{
				cameraPosition = cameraPosition,
				cameraRotationY = cameraRotation.y,
				enemyPositions = _enemyPositions,
				enemyForwardDirections = _enemyForwardDirections,
				returnedRotationIndex = _returnedRotationIndex,
				returnedRotation = _returnedRotation,
				anglesArray = _newNativeAngleArray,
			};

			var scheduledJob = job.Schedule(_enemyList.Count, 100);
			scheduledJob.Complete();

			for (int i = 0; i < _enemyList.Count; i++)
			{
				_enemyList[i].UpdateRotationIndex(job.returnedRotationIndex[i]);
				_enemyList[i].RotateSprite(job.returnedRotation[i]);
			}
			
			_enemyPositions.Dispose();
			_enemyForwardDirections.Dispose();
			
			_returnedRotationIndex.Dispose();
			_returnedRotation.Dispose();
		}
		else
		{
			foreach (var enemy in _enemyList)
			{
				enemy.UpdateRotationIndex(
					GetRotationIndex(
						BetterDetermineAngle(
							enemy.transform.position,
							cameraPosition,
							enemy.transform.forward),
						_angleRanges)
				);
			
				enemy.spriteRendererComponent.transform.rotation = Quaternion.Euler(0f, cameraRotation.y, 0f);
			}
		}
	}
	
	public void AddEnemyToList(EnemyDOTS enemy)
	{
		_enemyList.Add(enemy);
	}
	
	public void RemoveEnemyFromList(EnemyDOTS enemy)
	{
		_enemyList.Remove(enemy);
	}

	#region Calculating angle for sprite animation selection
	// private readonly List<AngleThresholds> _angleRanges = new List<AngleThresholds>
	private readonly AngleThresholds[] _angleRanges = new[]
	{
		new AngleThresholds(-22.5f, 22.5f, 0),
		// new AngleThresholds(-22.5f, 22.6f, 0),
		new AngleThresholds(22.5f, 67.5f, 7),
		// new AngleThresholds(22.5f, 67.5f, 7),
		new AngleThresholds(67.5f, 112.5f, 6),
		// new AngleThresholds(67.5f, 112.5f, 6),
		new AngleThresholds(112.5f, 157.5f, 5),
		// new AngleThresholds(112.5f, 157.5f, 5),
		
		new AngleThresholds(-157.5f, 157.5f, 4),
		// new AngleThresholds(-157.5f, 157.5f, 4),
		new AngleThresholds(-157.5f, -112.5f, 3),
		// new AngleThresholds(-157.5f, -112.5f, 3),
		new AngleThresholds(-112.5f, -67.5f, 2),
		// new AngleThresholds(-112.5f, -67.5f, 2),
		new AngleThresholds(-67.5f, -22.5f, 1),
		// new AngleThresholds(-67.5f, -22.5f, 1),
	};
	
	private int GetRotationIndex(float currentAngle, AngleThresholds[] thresholds)
	{
		for (int i = 0; i < thresholds.Length; i++)
		{
			// x = min, y = max, z = index
			if (thresholds[i].index == 0f) // special case for front
			{
				if (currentAngle > thresholds[i].min && currentAngle < thresholds[i].max)
				{
					return thresholds[i].index;
				}
			}
			else if (thresholds[i].index == 4f) // special case for back
			{
				if (currentAngle <= thresholds[i].min || currentAngle >= thresholds[i].max)
				{
					return thresholds[i].index;
				}
			}
			else // everything else
			if (currentAngle >= thresholds[i].min && currentAngle < thresholds[i].max)
			{
				return thresholds[i].index;
			}
		}

		return 0;
	}

	private float BetterDetermineAngle(Vector3 a, Vector3 b, Vector3 aDir)
	{
		Vector3 targetPosition = new Vector3(b.x, a.y, b.z);
		Vector3 targetDirection = targetPosition - a;

		return Vector3.SignedAngle(targetDirection, aDir, Vector3.up);
	}
	#endregion

	

	private void OnDestroy()
	{
		// Dispose the NativeArray of angles and indices.
		_newNativeAngleArray.Dispose();
	}
}

[BurstCompile]
public struct AngleThresholds
{
	public AngleThresholds(float min, float max, int index)
	{
		this.min = min;
		this.max = max;
		this.index = index;
	}

	public float min;
	public float max;
	public int index;
}