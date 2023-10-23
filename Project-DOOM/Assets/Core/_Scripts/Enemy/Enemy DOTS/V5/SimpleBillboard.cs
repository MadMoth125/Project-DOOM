using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SimpleBillboard : MonoBehaviour
{
	private static Transform _targetInstance;

	private Animator _animatorComponent;
	private SpriteRenderer _spriteRendererComponent;

	private void Awake()
	{
		if (!_targetInstance) _targetInstance = Camera.main.transform;
		
		_animatorComponent = GetComponent<Animator>();
		if (!_animatorComponent) _animatorComponent = GetComponentInChildren<Animator>();
		
		_spriteRendererComponent = GetComponent<SpriteRenderer>();
		if (!_spriteRendererComponent) _spriteRendererComponent = GetComponentInChildren<SpriteRenderer>();
	}

	private void FixedUpdate()
	{
		_spriteRendererComponent.transform.rotation = Quaternion.Euler(0f, _targetInstance.rotation.eulerAngles.y, 0f);
	}

	private void OnBecameVisible()
	{
		Debug.Log($"{gameObject} Visible");
	}

	private void OnBecameInvisible()
	{
		Debug.Log($"{gameObject} Invisible");
	}
}