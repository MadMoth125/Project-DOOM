using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

public class EnemyMovementReporter : MonoBehaviour
{
	[HideInInspector]
	public NavMeshAgent navMeshAgentComponent;
	[HideInInspector]
	public Animator animatorComponent;

	private bool IsMoving => navMeshAgentComponent.velocity != Vector3.zero;
	private bool _previousIsMoving = false;
	
	private void Awake()
	{
		navMeshAgentComponent = gameObject.SearchForComponent<NavMeshAgent>();
		animatorComponent = gameObject.SearchForComponent<Animator>();
	}

	private void Update()
	{
		if (navMeshAgentComponent == null || animatorComponent == null) return;

		if (IsMoving != _previousIsMoving)
		{
			animatorComponent.SetBool(PropertyReferences.IsMovingPropertyHash, IsMoving);
		}
		
		_previousIsMoving = IsMoving;
	}
}