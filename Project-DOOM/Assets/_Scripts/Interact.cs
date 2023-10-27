using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
	private Transform _cameraTransform;
	
	private void Start()
	{
		_cameraTransform = Camera.main.transform;
	}
	
	private void Update()
	{
		// Check for user input (e.g., mouse click)
		if (Input.GetKeyDown(KeyCode.F))
		{
			Debug.Log("F key pressed");
			// Perform a raycast from the camera's position forward
			Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward * 5f);

			Debug.DrawRay(_cameraTransform.position, _cameraTransform.forward * 5f, Color.red, 1f);
			if (Physics.Raycast(ray, out RaycastHit hit, 5f))
			{
				Debug.Log($"Hit {hit.collider.gameObject.name}");
				// Check if the object has the IInteractable interface
				IInteractable interactable = hit.collider.GetComponent<IInteractable>();

				if (interactable != null)
				{
					// Call the Interact method on the interactable object
					interactable.Interact(transform);
				}
			}
		}
	}
}