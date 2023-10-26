using System;
using System.Collections;
using System.Collections.Generic;
using MinaPechuex.BehaviorTree;
using UnityEngine;

public class TaskDetectPlayer : Node
{
	private Transform _source;
	private float _detectionRadius;
	
	public TaskDetectPlayer(Transform source, float detectionRadius)
	{
		_source = source;
		_detectionRadius = detectionRadius;
	}
	
	public override NodeState Evaluate()
	{
		object cachedResult = GetData("target");
		if (cachedResult == null)
		{
			// Vector3.Distance(_source.position)
		}
		
		return NodeState.SUCCESS;
	}
}