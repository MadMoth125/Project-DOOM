using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentTest : MonoBehaviour
{
	private NavMeshAgent _navAgentComponent;

	public Transform target;
	
	private void Awake()
	{
		_navAgentComponent = gameObject.SearchForComponent<NavMeshAgent>();
	}

	private void Start()
	{
		_navAgentComponent.SetDestination(target.position);
	}

	private void Update()
	{
		
	}
}