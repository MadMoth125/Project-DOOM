using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBillboardManager : MonoBehaviour
{
	public static EnemyBillboardManager Instance { get; private set; }
	
	private List<EnemyComponents> _enemyComponents = new List<EnemyComponents>();
	
	private Coroutine _rotationCoroutine;
	
	private Transform _target;
	private Vector3 _prevTargetPosition;
	private Quaternion _prevTargetRotation;

	[SerializeField]
	[Tooltip("How many enemies to rotate per frame.\n" +
	         "Higher values will result in faster rotation, but will also cause more stuttering.")]
	private int rotationBatchCount = 10;
	
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Debug.LogWarning($"Multiple instances of {GetType().Name} found. Disabling dupe instance.");
			enabled = false;
		}
	}
	
	private void OnDisable()
	{
		// StopCoroutine(_rotationCoroutine);
		// _rotationCoroutine = null;
	}

	private void Start()
	{
		_target = Camera.main.transform;
		_prevTargetRotation = _target.rotation;
		// StartCoroutine(RotateToFacePlayer());
	}

	private void Update()
	{
		// BillboardJob job = new BillboardJob();
	}

	public void AddEnemyComponent(EnemyComponents component)
	{
		_enemyComponents.Add(component);
	}
	
	public void RemoveEnemyComponent(EnemyComponents component)
	{
		_enemyComponents.Remove(component);
	}
	
	private IEnumerator RotateToFacePlayer()
	{
		while (true)
		{
			if (_prevTargetRotation == _target.rotation)
			{
				yield return null;
			}
			else
			{
				int current = 0;
				foreach (var component in _enemyComponents)
				{
					component.SpriteComponent.transform.rotation = Quaternion.Euler(0f, _target.rotation.eulerAngles.y + 180f, 0f);
					Debug.Log("Rotating");

					if (current++ == rotationBatchCount)
					{
						current = 0;
						yield return null;
					}
				}
				
				_prevTargetPosition = _target.position;
				_prevTargetRotation = _target.rotation;
			}
			
			// loop should be infinite
		}
	}
}