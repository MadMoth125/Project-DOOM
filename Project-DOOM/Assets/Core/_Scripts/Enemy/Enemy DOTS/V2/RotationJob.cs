using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public struct RotationJob : IJobParallelFor
{
    public Vector3 CameraPosition;
    [ReadOnly] public NativeArray<Vector3> PositionsA;
    [ReadOnly] public NativeArray<Vector3> PositionsB;
    [ReadOnly] public Vector3 ForwardDirection;
    [ReadOnly] private readonly List<(float, float, int)> _angleRanges;
    [ReadOnly] private int _lastRotationIndex;

    [WriteOnly] public NativeArray<Quaternion> ResultRotations;
    [WriteOnly] public NativeArray<int> ResultIndices;

    public RotationJob(List<(float, float, int)> angleRanges, Vector3 cameraPosition)
    {
        _angleRanges = angleRanges;
        CameraPosition = cameraPosition;
        PositionsA = new NativeArray<Vector3>(0, Allocator.TempJob);
        PositionsB = new NativeArray<Vector3>(0, Allocator.TempJob);
        ForwardDirection = Vector3.zero;
        _lastRotationIndex = 0;
        ResultRotations = new NativeArray<Quaternion>(0, Allocator.TempJob);
        ResultIndices = new NativeArray<int>(0, Allocator.TempJob);
    }

    public void Execute(int index)
    {
        float currentAngle = BetterDetermineAngle(PositionsA[index], PositionsB[index], ForwardDirection);
        int rotationIndex = GetRotationIndex(currentAngle);

        // Assume Camera position is the target position for the enemies.
        Vector3 targetPosition = CameraPosition;

        // Calculate the rotation based on the target position.
        Quaternion rotation = Quaternion.LookRotation(targetPosition - PositionsA[index], Vector3.up);

        ResultRotations[index] = rotation;
        ResultIndices[index] = rotationIndex;
    }

    private int GetRotationIndex(float currentAngle)
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
    }
}