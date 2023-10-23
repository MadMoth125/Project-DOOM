using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnerTest : MonoBehaviour
{
	public GameObject enemyPrefab;

	private void Start()
	{
		InvokeRepeating(nameof(SpawnEnemy), 0.1f, 0.1f);
	}

	private void SpawnEnemy()
	{
		var spawnPoint = Random.insideUnitCircle * 5;
		Instantiate(enemyPrefab, transform.position + new Vector3(spawnPoint.x, 0f, spawnPoint.y), Quaternion.identity);
	}
	
}