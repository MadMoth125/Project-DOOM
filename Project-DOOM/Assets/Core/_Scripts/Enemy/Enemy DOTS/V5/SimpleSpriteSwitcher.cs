using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace DOTS.V5
{
	public class SimpleSpriteSwitcher : MonoBehaviour
	{
		private static readonly int RotationIndexHash = Animator.StringToHash("Rotation Index");
		
		private static readonly float3[] AngleChart = new[]
		{
			// front angles
			new float3(-22.5f, 22.5f, 0f),
			new (22.5f, 67.5f, 7f),
			new (67.5f, 112.5f, 6f),
			new (112.5f, 157.5f, 5f),
		
			// back angles
			new float3(-157.5f, 157.5f, 4f),
			new (-157.5f, -112.5f, 3f),
			new (-112.5f, -67.5f, 2f),
			new (-67.5f, -22.5f, 1f),
		};
		
		private static Transform _cameraInstance;
		
		private NativeArray<float3> _anglesNativeArray;
		private NativeArray<int> _rotationIndexNativeArray;
		private JobHandle _scheduledJobHandle;
		
		private Animator _animatorComponent;
		private SpriteRenderer _spriteRendererComponent;

		public bool enableJobs = true;
		private bool _ableToRunJob = true;

		private void Awake()
		{
			// assigning static camera instance
			if (!_cameraInstance) _cameraInstance = Camera.main.transform;
			
			// getting animator component
			_animatorComponent = GetComponent<Animator>();
			_spriteRendererComponent = GetComponent<SpriteRenderer>();
			if (!_animatorComponent) _animatorComponent = GetComponentInChildren<Animator>();
			if (!_spriteRendererComponent) _spriteRendererComponent = GetComponentInChildren<SpriteRenderer>();
			
			// disposing native arrays (if they have been created)
			if (_anglesNativeArray.IsCreated) _anglesNativeArray.Dispose();
			if (_rotationIndexNativeArray.IsCreated) _rotationIndexNativeArray.Dispose();
			
			// creating new native arrays
			_anglesNativeArray = new NativeArray<float3>(AngleChart, Allocator.Persistent);
			_rotationIndexNativeArray = new NativeArray<int>(1, Allocator.Persistent);
		}

		private void Update()
		{
			_ableToRunJob &= _animatorComponent != null;
			_ableToRunJob &= _spriteRendererComponent != null;
			_ableToRunJob &= enableJobs;
			
			if (!_animatorComponent)
			{
				Debug.LogWarning($"No {_animatorComponent.GetType()} component found on {gameObject} or children of {gameObject}", this);
			}
			
			if (!_spriteRendererComponent)
			{
				Debug.LogWarning($"No {_spriteRendererComponent.GetType()} component found on {gameObject} or children of {gameObject}", this);
			}
			
			if (_ableToRunJob)
			{
				// allocating required variables for job
				float3 selfPosition = transform.position;
				float3 selfForward = transform.forward;
				float3 camPosition = _cameraInstance.position;

				// creating job
				SpriteSwitcherJob spriteSwitcherJob = new SpriteSwitcherJob
				{
					transformPosition = selfPosition,
					transformForward = selfForward,
					cameraPosition = camPosition,
					angles = _anglesNativeArray,
					rotationIndex = _rotationIndexNativeArray
				};
			
				// scheduling & completing job
				_scheduledJobHandle = spriteSwitcherJob.Schedule();
				_scheduledJobHandle.Complete();

				// setting animator rotation index
				_animatorComponent.SetFloat(RotationIndexHash, spriteSwitcherJob.rotationIndex[0]);
				
				// flipping sprite based on rotation index
				_spriteRendererComponent.flipX = spriteSwitcherJob.rotationIndex[0] is > 0 and < 4;
			}
			else
			{
				Debug.Log("Job cannot currently run!");
			}
		}

		private void OnDestroy()
		{
			// disposing native arrays (if they have been created)
			if (_anglesNativeArray.IsCreated) _anglesNativeArray.Dispose();
			if (_rotationIndexNativeArray.IsCreated) _rotationIndexNativeArray.Dispose();
		}
	}
}