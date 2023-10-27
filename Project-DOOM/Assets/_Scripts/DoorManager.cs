using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour, IInteractable
{
	public Vector3 openOffset;
	public float openSpeed;
	public bool closeAutomatically = true;
	
	private Vector3 _openPosition;
	private Vector3 _closedPosition;
	private bool _isOpen = false;
	
	private Coroutine _doorCoroutine;
	private Transform _instigatorReference;
	
	private void Start()
	{
		_openPosition = transform.position + openOffset;
		_closedPosition = transform.position;
	}

	private void Update()
	{
		if (closeAutomatically && _instigatorReference != null)
		{
			float dist = Mathf.Abs(Vector3.Distance(transform.position, _instigatorReference.position));

			if (dist > 20f)
			{
				StartCoroutine(CloseDoor());
				
				_instigatorReference = null;
			}
		}
	}
	
	public void Interact(Transform instigator)
	{
		if (_doorCoroutine != null) return;
		
		_instigatorReference = instigator;
		
		if (_isOpen)
		{
			_doorCoroutine = StartCoroutine(CloseDoor());
		}
		else
		{
			_doorCoroutine = StartCoroutine(OpenDoor());
		}
	}
	
	private IEnumerator OpenDoor()
	{
		while (Mathf.Abs(Vector3.Distance(transform.position, _openPosition)) > 0.01f)
		{
			transform.position = Vector3.MoveTowards(transform.position, _openPosition, openSpeed * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
		
		_doorCoroutine = null;
	}
	
	private IEnumerator CloseDoor()
	{
		while (Mathf.Abs(Vector3.Distance(transform.position, _closedPosition)) > 0.01f)
		{
			transform.position = Vector3.MoveTowards(transform.position, _closedPosition, openSpeed * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
		
		_doorCoroutine = null;
	}
}