using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterEnemy : MonoBehaviour
{
	private BetterEnemyManager enemyManager;

	private void OnEnable()
	{
		// Ensure that the EnemyManager exists in the scene
		enemyManager = FindObjectOfType<BetterEnemyManager>();
		if (enemyManager != null)
		{
			// Add this enemy to the manager
			enemyManager.AddEnemy(this);
		}
		else
		{
			Debug.LogError("EnemyManager not found in the scene!");
		}
	}

	private void OnDisable()
	{
		// Remove this enemy from the manager when disabled
		if (enemyManager != null)
		{
			enemyManager.RemoveEnemy(this);
		}
	}
}