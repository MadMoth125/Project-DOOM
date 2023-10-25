using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace EnemySpriteHandling
{
	public class EnemySpriteManager : MonoBehaviour
	{
		public static EnemySpriteManager Instance { get; private set; }
		
		private readonly List<ISpriteComponentReferences> _componentReferences = new List<ISpriteComponentReferences>();
		
		private readonly float3[] _directions = new[]
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

		private Transform _cameraTransform;
		
		private NativeArray<float3> _directionsNativeArray;
		private NativeArray<float3> _enemyPositionsNativeArray;
		private NativeArray<float3> _enemyDirectionsNativeArray;
		
		private NativeArray<int> _outRotationIndexes;
		private NativeArray<float> _outRotationsY;
		
		private int _cachedComponentCount = 0;
		
		private void Awake()
		{
			InitializeSingleton();
			
			InitializeDirectionArray();
			
			_cameraTransform = Camera.main.transform;
			
			// local functions
			void InitializeSingleton()
			{
				if (Instance)
				{
					Debug.LogWarning($"Cannot have more than one {this.GetType()}", this);
					Destroy(gameObject);
					return;
				}
			
				Instance = this;
			}

			void InitializeDirectionArray()
			{
				// allocate once since we won't be adding or removing directions
				DisposeDirectionsArray();
				_directionsNativeArray = new NativeArray<float3>(_directions, Allocator.Persistent);
			}
		}

		private void DetectComponentChanges()
		{
			if (_cachedComponentCount != _componentReferences.Count)
			{
				_cachedComponentCount = _componentReferences.Count;
				
				RefreshArrays(_componentReferences.Count);
			}
		}

		private void RefreshArrays(int length)
		{
			DisposeActivePersistantArrays();
			
			_enemyPositionsNativeArray = new NativeArray<float3>(length, Allocator.Persistent);
			_enemyDirectionsNativeArray = new NativeArray<float3>(length, Allocator.Persistent);
			_outRotationIndexes = new NativeArray<int>(length, Allocator.Persistent);
			_outRotationsY = new NativeArray<float>(length, Allocator.Persistent);
		}
		
		private void Update()
		{
			DetectComponentChanges();
			
			PopulateNecessaryJobData();
			
			EnemySpriteSelectorJob spriteSelectorJob = new EnemySpriteSelectorJob
			{
				playerPosition = _cameraTransform.position,
				playerRotationY = _cameraTransform.rotation.eulerAngles.y,
				directions = _directionsNativeArray,
				enemyPositions = _enemyPositionsNativeArray,
				enemyDirections = _enemyDirectionsNativeArray,
				rotationIndex = _outRotationIndexes,
				rotationY = _outRotationsY
			};

			// schedule the job and "dynamically" assign the batch count
			var activeJob = spriteSelectorJob.Schedule(_componentReferences.Count, _componentReferences.Count / 6);
			
			activeJob.Complete();
			
			ApplyResultsToInstances();
			
			void PopulateNecessaryJobData()
			{
				// fill the array with the positions and directions of each sprite component instance
				for (int i = 0; i < _componentReferences.Count; i++)
				{
					_enemyPositionsNativeArray[i] = _componentReferences[i].GetTransform().position;
					_enemyDirectionsNativeArray[i] = _componentReferences[i].GetTransform().forward;
				}
			}
			
			void ApplyResultsToInstances()
			{
				// apply the results of the job to each sprite component instance
				for (int i = 0; i < _componentReferences.Count; i++)
				{
					_componentReferences[i].UpdateAnimatorIndex(spriteSelectorJob.rotationIndex[i]);
					_componentReferences[i].UpdateSpriteDirection(spriteSelectorJob.rotationY[i]);
				}
			}
		}

		private void OnDestroy()
		{
			DisposeAllPersistantArrays();
		}
		
		public void AddEnemySprite(ISpriteComponentReferences component)
		{
			if (component == null)
			{
				Debug.LogWarning("Cannot add null enemy sprite", this);
				return;
			}
			
			_componentReferences.Add(component);
		}
		
		public void RemoveEnemySprite(ISpriteComponentReferences component)
		{
			if (component == null)
			{
				Debug.LogWarning("Cannot remove null enemy sprite", this);
				return;
			}
			
			_componentReferences.Remove(component);
		}
		
		public bool ContainsEnemySprite(ISpriteComponentReferences component)
		{
			if (component == null)
			{
				Debug.LogWarning("Cannot check for null enemy sprite", this);
				return false;
			}
			
			return _componentReferences.Contains(component);
		}
		
		private void DisposeAllPersistantArrays()
		{
			DisposeActivePersistantArrays();
			DisposeDirectionsArray();
		}

		private void DisposeDirectionsArray()
		{
			if (_directionsNativeArray.IsCreated) _directionsNativeArray.Dispose();
		}
		
		private void DisposeActivePersistantArrays()
		{
			if (_enemyPositionsNativeArray.IsCreated) _enemyPositionsNativeArray.Dispose();
			if (_enemyDirectionsNativeArray.IsCreated) _enemyDirectionsNativeArray.Dispose();
			if (_outRotationIndexes.IsCreated) _outRotationIndexes.Dispose();
			if (_outRotationsY.IsCreated) _outRotationsY.Dispose();
		}
	}
}