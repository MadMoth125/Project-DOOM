using System;
using System.Collections;
using System.Collections.Generic;
using MinaPechuex.BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

[RequireComponent(typeof(EnemyMovementReporter))]
public class ImpTree : NodeTree
{
	private NavMeshAgent _navAgentComponent;
	private Vector3 _startingPosition;
	
	private TaskDetectTarget _taskDetectTarget;
	private TaskChaseTarget _taskChaseTarget;
	private TaskWander _taskWander;
	
	private void Awake()
	{
		_navAgentComponent = gameObject.SearchForComponent<NavMeshAgent>();
		
		_startingPosition = transform.position;
		
		// initialize tasks so save on performance (slightly)
		_taskWander = new TaskWander(_navAgentComponent, _startingPosition, 8f, 3f, 6f);
		_taskDetectTarget = new TaskDetectTarget(transform, 50f);
		_taskChaseTarget = new TaskChaseTarget(_navAgentComponent, transform, 80f);
	}

	protected override Node SetupTree()
	{
		Node root = new Selector(new List<Node>
		{
			new Sequence(new List<Node>
			{
				_taskDetectTarget,
				_taskChaseTarget,
			}),
			
			_taskWander
		});
		
		return root;
	}
}