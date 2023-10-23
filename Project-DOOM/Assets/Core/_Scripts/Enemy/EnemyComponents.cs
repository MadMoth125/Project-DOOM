using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;

public class EnemyComponents : MonoBehaviour
{
	public SpriteRenderer SpriteComponent { get; private set; }

	private NativeArray<Quaternion> _rotationResult;
	
	private JobHandle _jobHandle;
	
	private void Awake()
	{
		SpriteComponent = GetComponent<SpriteRenderer>();
		SpriteComponent = GetComponentInChildren<SpriteRenderer>();
		
		_rotationResult = new NativeArray<Quaternion>(1, Allocator.Persistent);
	}

	private void Start()
	{
		EnemyBillboardManager.Instance.AddEnemyComponent(this);
	}

	private void Update()
	{
		BillboardJob job = new BillboardJob(
			Camera.main.transform.rotation,
			180f,
			_rotationResult);

		_jobHandle = job.Schedule();
		_jobHandle.Complete();
		SpriteComponent.transform.rotation = _rotationResult[0];
	}

	private void LateUpdate()
	{
		
	}

	private void OnDestroy()
	{
		EnemyBillboardManager.Instance.RemoveEnemyComponent(this);
		_rotationResult.Dispose();
	}
}