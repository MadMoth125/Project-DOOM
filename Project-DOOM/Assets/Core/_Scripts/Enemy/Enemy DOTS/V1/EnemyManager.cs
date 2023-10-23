using System;
using System.Collections;
using Unity.Collections;
using Unity.Jobs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public struct SpriteRotationJob : IJobParallelForTransform
{
	// public NativeArray<EnemyData> enemyData1;

	public void Execute(int index, TransformAccess transform)
	{
		
	}
}

public class EnemyManager : MonoBehaviour
{
	public static EnemyManager Instance { get; private set; }
	
	private List<Enemy> _enemies = new List<Enemy>();
	
	private void Awake()
	{
		ManageSingleton();
	}

	private void Update()
	{
		/*var enemyData2 = new NativeArray<EnemyData>(_enemies.Count, Allocator.TempJob);
		for (int i = 0; i < _enemies.Count; i++)
		{
			enemyData2[i] = new EnemyData(_enemies[i], Camera.main.transform);
		}
		var rotJob = new SpriteRotationJob
		{
			enemyData1 = enemyData2
		};
		var rotJobHandle = rotJob.Schedule(_enemies.Count, 1);
		rotJobHandle.Complete();
		enemyData2.Dispose();*/
	}
	
	public void AddEnemy(Enemy enemy)
	{
		_enemies.Add(enemy);
	}
	
	public void RemoveEnemy(Enemy enemy)
	{
		_enemies.Remove(enemy);
	}
	
	private void ManageSingleton()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Debug.LogWarning($"Multiple instances of {GetType().Name} found. Disabling dupe instance.");
			enabled = false;
		}
	}
	
	
}