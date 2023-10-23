using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDOTS : MonoBehaviour
{
	private static readonly int Property = Animator.StringToHash("Rotation Index");
	
	public SpriteRenderer spriteRendererComponent;
	public Animator animatorComponent;
	
	private Vector3 _currentPosition;
	private Vector3 _currentForward;

	private int _rotationIndex = 0;

	private void Awake()
	{
		spriteRendererComponent = GetComponentInChildren<SpriteRenderer>();
		animatorComponent = GetComponentInChildren<Animator>();
	}

	private void Start()
	{
		EnemyManagerDOTS.Instance.AddEnemyToList(this);
	}

	private void OnDestroy()
	{
		EnemyManagerDOTS.Instance.RemoveEnemyFromList(this);
	}

	public void UpdateRotationIndex(int newIndex)
	{
		_rotationIndex = newIndex;
		
		spriteRendererComponent.flipX = _rotationIndex is > 0 and < 4;
		
		animatorComponent.SetFloat(Property, _rotationIndex);
	}
	
	public void RotateSprite(Quaternion rotation)
	{
		spriteRendererComponent.transform.rotation = rotation;
	}
}