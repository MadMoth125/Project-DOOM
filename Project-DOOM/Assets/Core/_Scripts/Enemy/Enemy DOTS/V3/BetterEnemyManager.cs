using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class BetterEnemyManager : MonoBehaviour
{
    private List<BetterEnemy> enemies = new List<BetterEnemy>();
    private NativeArray<Vector3> enemyPositions;
    private NativeArray<Vector3> enemyForwards;
    private NativeArray<Quaternion> resultRotations;
    private NativeArray<int> resultIndices;
    private JobHandle jobHandle;

    private void OnEnable()
    {
        // Initialize NativeArrays
        enemyPositions = new NativeArray<Vector3>(enemies.Count, Allocator.Persistent);
        enemyForwards = new NativeArray<Vector3>(enemies.Count, Allocator.Persistent);
        resultRotations = new NativeArray<Quaternion>(enemies.Count, Allocator.Persistent);
        resultIndices = new NativeArray<int>(enemies.Count, Allocator.Persistent);
    }

    private void OnDisable()
    {
        // Release NativeArrays
        enemyPositions.Dispose();
        enemyForwards.Dispose();
        resultRotations.Dispose();
        resultIndices.Dispose();
    }

    private void Update()
    {
        // Update NativeArrays with current enemy data
        UpdateNativeArrays();

        // Create and schedule the job
        BetterEnemyJob enemyJob = new BetterEnemyJob
        {
            enemyPositions = enemyPositions,
            enemyForwards = enemyForwards,
            playerCameraPosition = Camera.main.transform.position,
            resultRotations = resultRotations,
            resultIndices = resultIndices
        };

        //jobHandle = enemyJob.Schedule(enemies.Count, 32);

        // Complete the job
        jobHandle.Complete();

        // Use the results (e.g., apply rotations and indices)
        ApplyResults();
    }

    private void UpdateNativeArrays()
    {
        //enemyPositions.ResizeUninitialized(enemies.Count);
        //enemyForwards.ResizeUninitialized(enemies.Count);

        for (int i = 0; i < enemies.Count; i++)
        {
            enemyPositions[i] = enemies[i].transform.position;
            enemyForwards[i] = enemies[i].transform.forward;
        }
    }

    private void ApplyResults()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].transform.rotation = resultRotations[i];
            int rotationIndex = resultIndices[i];
            // Do something with the rotation index...
        }
    }

    // Methods to add/remove instances and make each Enemy instance add itself to the EnemyManager
    public void AddEnemy(BetterEnemy enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void RemoveEnemy(BetterEnemy enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }
}