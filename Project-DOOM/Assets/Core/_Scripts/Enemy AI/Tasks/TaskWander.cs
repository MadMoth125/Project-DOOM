using System;
using System.Collections;
using System.Collections.Generic;
using MinaPechuex.BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class TaskWander : Node
{
	private NavMeshAgent _navAgentComponent;
	
	private Vector3 _wanderPivot;
	private float _wanderRadius;
	
	private float _minWaitTime;
	private float _maxWaitTime;

	private bool _wating = true;
	
	public TaskWander(NavMeshAgent navReference, Vector3 wanderPivot, float wanderRadius)
	{
		_navAgentComponent = navReference;
		
		_wanderPivot = wanderPivot;
		_wanderRadius = wanderRadius;
		
		_minWaitTime = UnityEngine.Random.Range(1f, 4f);
		_maxWaitTime = UnityEngine.Random.Range(6f, 10f);
	}
	
	public TaskWander(NavMeshAgent navReference, Vector3 wanderPivot, float wanderRadius, float minWaitTime, float maxWaitTime)
	{
		_navAgentComponent = navReference;
		
		_wanderPivot = wanderPivot;
		_wanderRadius = wanderRadius;
		
		_minWaitTime = minWaitTime;
		_maxWaitTime = maxWaitTime;
	}

	public override NodeState Evaluate()
	{
		_navAgentComponent.SetDestination(_wanderPivot);
		if (_navAgentComponent.velocity != Vector3.zero)
		{
			
		}
		else
		{
			
		}

		return NodeState.RUNNING;
	}
}