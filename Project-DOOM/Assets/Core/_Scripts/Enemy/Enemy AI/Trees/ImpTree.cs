using System;
using System.Collections;
using System.Collections.Generic;
using MinaPechuex.BehaviorTree;
using UnityEngine;

[RequireComponent(typeof(EnemyMovementReporter))]
public class ImpTree : NodeTree
{
	private EnemyMovementReporter _movementReporter;
	private Vector3 _startingPosition;
	
	private TaskWander _wanderTask;
	
	private void Awake()
	{
		_movementReporter = gameObject.SearchForComponent<EnemyMovementReporter>();
		_startingPosition = transform.position;
		
		_wanderTask = new TaskWander(_movementReporter.navMeshAgentComponent, _startingPosition, 4f, 4f, 8f);
	}

	protected override Node SetupTree()
	{
		_wanderTask.UpdateParameters(_movementReporter.navMeshAgentComponent, _startingPosition, 4f, 4f, 8f);
		
		Node root = _wanderTask;

		return root;
	}
}