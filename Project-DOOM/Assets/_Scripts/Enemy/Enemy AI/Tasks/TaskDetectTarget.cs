using System;
using System.Collections;
using System.Collections.Generic;
using MinaPechuex.BehaviorTree;
using UnityEngine;
using Utilities;

public class TaskDetectTarget : Node
{
	// enemy
	public Transform currentTransform;
	
	// how close the player must be
	public float detectionRadius;
	
	public TaskDetectTarget(Transform currentTransform, float detectionRadius)
	{
		this.currentTransform = currentTransform;
		this.detectionRadius = detectionRadius;
	}
	
	public override NodeState Evaluate()
	{
		object cachedResult = GetData("target");
		
		if (cachedResult == null) // No previous target reference
		{
			// overlap sphere
			Collider[] colliders = Physics.OverlapSphere(currentTransform.position, detectionRadius);

			// loop through colliders
			foreach (Collider collider in colliders)
			{
				// if player is detected
				if (collider.CompareTag(PropertyRefs.PlayerTag))
				{
					// set target
					parent.parent.SetData("target", collider.transform);

					state = NodeState.SUCCESS;
					return state;
				}
			}

			// No target detected in the loop
			Debug.Log("Target not detected");
			state = NodeState.FAILURE;
			return state;
		}
    
		// Target already detected
		Debug.Log("Target already detected");
		state = NodeState.SUCCESS;
		return state;
	}
}