using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class NewEnemyManager : MonoBehaviour
{
	private List<NewEnemy> _enemies = new(); // Assuming Enemy is your script for individual enemies.
    private List<(float, float, int)> _angleRanges;

    public void AddEnemy(NewEnemy enemy)
    {
        _enemies.Add(enemy);
    }
    
    public void RemoveEnemy(NewEnemy enemy)
    {
        _enemies.Remove(enemy);
    }
    
    private void Start()
    {
        // Initialize _angleRanges with your values...
        // Initialize _enemies with references to your enemy instances...

        // Assuming Camera.main is not null.
        Vector3 cameraPosition = Camera.main.transform.position;

        // Create NativeArrays for positions and results.
        NativeArray<Vector3> positionsA = new NativeArray<Vector3>(_enemies.Count, Allocator.TempJob);
        NativeArray<Vector3> positionsB = new NativeArray<Vector3>(_enemies.Count, Allocator.TempJob);
        NativeArray<Quaternion> rotationsArray = new NativeArray<Quaternion>(_enemies.Count, Allocator.TempJob);
        NativeArray<int> indicesArray = new NativeArray<int>(_enemies.Count, Allocator.TempJob);

        // Populate positionsA and positionsB with the actual positions and forward directions of your enemies.
        for (int i = 0; i < _enemies.Count; i++)
        {
            positionsA[i] = _enemies[i].transform.position;
            positionsB[i] = _enemies[i].transform.position + _enemies[i].transform.forward;
        }

        // Create the job.
        RotationJob rotationJob = new RotationJob(_angleRanges, cameraPosition)
        {
            PositionsA = positionsA,
            PositionsB = positionsB,
            ForwardDirection = Camera.main.transform.forward, // You might need to customize this based on your game logic.
            ResultRotations = rotationsArray,
            ResultIndices = indicesArray
        };

        // Schedule the job.
        JobHandle jobHandle = rotationJob.Schedule(_enemies.Count, 64);

        // Wait for the job to complete.
        jobHandle.Complete();

        // Apply the results to the enemies.
        for (int i = 0; i < _enemies.Count; i++)
        {
            _enemies[i].ApplyRotationAndIndex(rotationsArray[i], indicesArray[i]);
        }

        // Dispose of NativeArrays.
        positionsA.Dispose();
        positionsB.Dispose();
        rotationsArray.Dispose();
        indicesArray.Dispose();
    }
}