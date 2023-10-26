using System;
using System.Collections;
using System.Collections.Generic;
using ProjectDOOM.deprecated.V1;
using UnityEngine;

namespace ProjectDOOM.deprecated.V1
{
	public class Enemy : MonoBehaviour
	{
		private readonly int _rotationPropertyHash = Animator.StringToHash("Rotation Index");
	
		public SpriteRenderer SpriteRendererComponent { get; private set; }
		public Animator SpriteAnimatorComponent { get; private set; }

		private void Awake()
		{
			SpriteRendererComponent = GetComponentInChildren<SpriteRenderer>();
			SpriteAnimatorComponent = GetComponentInChildren<Animator>();
		}

		private void Start()
		{
			// calling in start to ensure that the EnemyManager is initialized
			EnemyManager.Instance.AddEnemy(this);
		}

		/*
	private void Update()
	{
		// rotate the sprite transform to face the camera
		SpriteRendererComponent.transform.rotation = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f);
		
		SpriteAnimatorComponent.SetFloat(
			_rotationPropertyHash, // "Rotation Index" hash
			GetRotationIndex( // get the rotation index based on the angle between the enemy and the camera
				BetterDetermineAngle( // determine the angle between the enemy and the camera
					transform.position,
					Camera.main.transform.position,
					transform.forward)
				)
			);
	}
	*/
	
		private void OnDestroy()
		{
			EnemyManager.Instance.RemoveEnemy(this);
		}
	
		#region Sprite Selection
	
		// pre-calculated angle ranges for sprite selection
		private readonly List<(float, float, int)> _angleRanges = new List<(float, float, int)>
		{
			(-22.5f, 22.5f, 0),
			(22.5f, 67.5f, 7),
			(67.5f, 112.5f, 6),
			(112.5f, 157.5f, 5),
			(-157.5f, -112.5f, 3),
			(-112.5f, -67.5f, 2),
			(-67.5f, -22.5f, 1),
			(-180f, -157.5f, 4),
		};
	
		/*
	// last returned rotation index
	private int _lastRotationIndex = 0;
	
	public int GetRotationIndex(float currentAngle)
	{
		foreach (var (min, max, index) in _angleRanges)
		{
			if (currentAngle >= min && currentAngle < max)
			{
				_lastRotationIndex = index;
				return _lastRotationIndex;
			}
		}

		return _lastRotationIndex;
	}
	
	public float BetterDetermineAngle(Vector3 a, Vector3 b, Vector3 forwardDirection)
	{
		Vector3 targetPosition = new Vector3(b.x, a.y, b.z);
		Vector3 targetDirection = targetPosition - a;
		
		float angle = Vector3.SignedAngle(targetDirection, forwardDirection, Vector3.up);

		return angle;
	}
	*/
	
		#endregion
	}
}

public struct EnemyData
{
	public int RotationIndex { get; private set; }
	public Quaternion NewRotation { get; private set; }
		
	private Vector3 _cameraPosition;
	private Vector3 _cameraRotation;
	private Vector3 _enemyPosition;
	private Vector3 _enemyForwardDirection;

	private int _lastRotationIndex; // last returned rotation index
		
	// pre-calculated angle ranges for sprite selection
	private readonly List<(float, float, int)> _angleRanges;
		
	public EnemyData(Enemy enemy, Transform cameraTransform)
	{
		_enemyPosition = enemy.transform.position;
		_enemyForwardDirection = enemy.transform.forward;
		_cameraPosition = cameraTransform.position;
		_cameraRotation = cameraTransform.eulerAngles;
			
		RotationIndex = 0;
		NewRotation = Quaternion.identity;
			
		_lastRotationIndex = 0;
			
		_angleRanges = new(){
			(-22.5f, 22.5f, 0),
			(22.5f, 67.5f, 7),
			(67.5f, 112.5f, 6),
			(112.5f, 157.5f, 5),
			(-157.5f, -112.5f, 3),
			(-112.5f, -67.5f, 2),
			(-67.5f, -22.5f, 1),
			(-180f, -157.5f, 4),
		};
	}
		
	public void Update()
	{
		// rotate the sprite transform to face the camera
		NewRotation = Quaternion.Euler(0f, _cameraRotation.y, 0f);
		
		RotationIndex = GetRotationIndex( // get the rotation index based on the angle between the enemy and the camera
			BetterDetermineAngle( // determine the angle between the enemy and the camera
				_enemyPosition,
				_cameraPosition,
				_enemyForwardDirection)
		);
	}
		
	public int GetRotationIndex(float currentAngle)
	{
		foreach (var (min, max, index) in _angleRanges)
		{
			if (currentAngle >= min && currentAngle < max)
			{
				_lastRotationIndex = index;
				return _lastRotationIndex;
			}
		}

		return _lastRotationIndex;
	}
	
	public float BetterDetermineAngle(Vector3 a, Vector3 b, Vector3 forwardDirection)
	{
		Vector3 targetPosition = new Vector3(b.x, a.y, b.z);
		Vector3 targetDirection = targetPosition - a;

		return Vector3.SignedAngle(targetDirection, forwardDirection, Vector3.up);
	}
}