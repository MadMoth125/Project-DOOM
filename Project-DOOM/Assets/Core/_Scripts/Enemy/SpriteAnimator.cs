using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class SpriteAnimator : MonoBehaviour
{
	private readonly AngleIndexFromTransform _angleCheck = new AngleIndexFromTransform();
	private readonly int _rotationPropertyHash = Animator.StringToHash("Rotation Index");
	
	private SpriteRenderer _spriteRendererComponent;
	private Animator _animatorComponent;
	
	public Transform Target => _target;
	public float Angle => _angle;
	
	private Transform _target;
	private float _angle;

	private int _rotationIndex = 0;
	private int _lastRotationIndex = 0;
	
	private void Start()
	{
		GetComponents();
	}

	private void Update()
	{
		_angleCheck.DetermineAngle(transform, _target, out _angle);
		
		_rotationIndex = _angleCheck.GetRotationIndex(_angle);
		
		if (_rotationIndex != _lastRotationIndex)
		{
			Debug.Log("Angle updated");
			_lastRotationIndex = _rotationIndex;
			
			// flipping sprite based on rotation index
			_spriteRendererComponent.flipX = _rotationIndex is > 0 and < 4;
			
			// setting animator rotation index
			_animatorComponent.SetFloat(_rotationPropertyHash, _angleCheck.GetRotationIndex(_angle));
		}
	}
	
	private void GetComponents()
	{
		_target ??= Camera.main.transform;
		
		_spriteRendererComponent ??= GetComponent<SpriteRenderer>();
		_animatorComponent ??= GetComponent<Animator>();
	}
}