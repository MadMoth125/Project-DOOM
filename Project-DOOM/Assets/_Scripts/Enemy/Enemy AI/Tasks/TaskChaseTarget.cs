using System;
using System.Collections;
using System.Collections.Generic;
using MinaPechuex.BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

public class TaskChaseTarget : Node
{
	private NavMeshAgent _navAgentComponent;
	
	// player
	private Transform _target;
	// enemy
	private Transform _currentTransform;
	// max distance before losing the target
	private float _forgetDistance;
	
	public TaskChaseTarget(NavMeshAgent navReference, Transform currentTransform, float forgetDistance)
	{
		_navAgentComponent = navReference;
		_currentTransform = currentTransform;
		_forgetDistance = forgetDistance;
	}

	public override NodeState Evaluate()
	{
		// attempt to get the target reference
		_target = (Transform)GetData("target");
		
		if (_target != null)
		{
			// Check the distance between the current position and the target position
			float distanceToTarget = Vector3.Distance(_target.position, _currentTransform.position);

			if (distanceToTarget < _forgetDistance)
			{
				// Continue chasing by setting the destination for the navigation agent
				_navAgentComponent.SetDestination(_target.position);
			}
			else
			{
				// If the distance is greater than _forgetDistance, stop chasing
				parent.parent.SetData("target", null); // Reset the target
			
				// Stop the navigation agent
				_navAgentComponent.SetDestination(_currentTransform.position);
			
				state = NodeState.FAILURE;
				return state;
			}

			
		}
		
		state = NodeState.RUNNING;
		return state;
	}
}