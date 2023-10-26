using System;
using System.Collections;
using System.Collections.Generic;
using MinaPechuex.BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

public class TaskWander : Node
{
	private NavMeshAgent _navAgentComponent;
	
	private Vector3 _wanderPivot;
	private float _wanderRadius;
	
	private float _minWaitTime;
	private float _maxWaitTime;

	private bool _waiting = true;
	private float _waitRequirement = 0f;
	private float _waitProgress = 0f;

	public TaskWander(NavMeshAgent navReference, Vector3 wanderPivot, float wanderRadius)
	{
		_navAgentComponent = navReference;
		
		_wanderPivot = wanderPivot;
		_wanderRadius = wanderRadius;
		
		_minWaitTime = UnityEngine.Random.Range(6f, 8f);
		_maxWaitTime = UnityEngine.Random.Range(10f, 14f);
		
		_waitRequirement = UnityEngine.Random.Range(_minWaitTime, _maxWaitTime);
	}
	
	public TaskWander(NavMeshAgent navReference, Vector3 wanderPivot, float wanderRadius, float minWaitTime, float maxWaitTime)
	{
		_navAgentComponent = navReference;
		
		_wanderPivot = wanderPivot;
		_wanderRadius = wanderRadius;
		
		_minWaitTime = minWaitTime;
		_maxWaitTime = maxWaitTime;
		
		_waitRequirement = UnityEngine.Random.Range(_minWaitTime, _maxWaitTime);
	}

	public void UpdateParameters(NavMeshAgent navReference, Vector3 wanderPivot, float wanderRadius, float minWaitTime, float maxWaitTime)
	{
		_navAgentComponent = navReference;
		
		_wanderPivot = wanderPivot;
		_wanderRadius = wanderRadius;
		
		_minWaitTime = minWaitTime;
		_maxWaitTime = maxWaitTime;
		
		_waitRequirement = UnityEngine.Random.Range(_minWaitTime, _maxWaitTime);
	}
	
	public override NodeState Evaluate()
	{
		if (_waiting)
		{
			_waitProgress += Time.deltaTime;

			if (_waitProgress >= _waitRequirement)
			{
				_waiting = false;
			}
			
			state = NodeState.RUNNING;
			return state;
		}

		Vector3 newPosition = (UnityEngine.Random.insideUnitSphere * _wanderRadius) + _navAgentComponent.transform.position;
		newPosition.y = _navAgentComponent.transform.position.y;

		_navAgentComponent.SetDestination(newPosition);
			
		_waiting = true;
		_waitProgress = 0f;
		_waitRequirement = UnityEngine.Random.Range(_minWaitTime, _maxWaitTime);
		
		state = NodeState.RUNNING;
		return state;
	}
	
}