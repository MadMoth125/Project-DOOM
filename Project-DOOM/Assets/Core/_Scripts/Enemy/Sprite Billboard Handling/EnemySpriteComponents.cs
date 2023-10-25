using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemySpriteHandling
{
	public class EnemySpriteComponents : MonoBehaviour, ISpriteComponentReferences
	{
		private SpriteRenderer _spriteRendererComponent;
		private Animator _animatorComponent;
		
		private void Awake()
		{
			_spriteRendererComponent = gameObject.SearchForComponent<SpriteRenderer>();
			_animatorComponent = gameObject.SearchForComponent<Animator>();
		}

		private void OnDisable()
		{
			RemoveSelfFromManager();
		}

		private void Start()
		{
			AddSelfToManager();
		}

		private void OnDestroy()
		{
			RemoveSelfFromManager();
		}

		public void UpdateAnimatorIndex(int index)
		{
			// setting the index for the animator's blend tree
			_animatorComponent.SetFloat(PropertyReferences.RotationIndexPropertyHash, index);
			
			// flipping the sprite based on which index we're at
			_spriteRendererComponent.flipX = index > 0 && index < 4;
		}

		public void UpdateSpriteDirection(float yRotation)
		{
			// only rotating on the y axis
			_spriteRendererComponent.transform.rotation = Quaternion.Euler(Vector3.up * yRotation);
		}

		public Transform GetSpriteTransform()
		{
			return _spriteRendererComponent.transform;
		}

		public Transform GetTransform()
		{
			return transform;
		}

		private void AddSelfToManager()
		{
			if (!EnemySpriteManager.Instance.ContainsEnemySprite(this))
			{
				EnemySpriteManager.Instance.AddEnemySprite(this);
			}
			else
			{
				Debug.LogWarning($"Cannot add {this} to {EnemySpriteManager.Instance} because it already contains it", this);
			}
		}

		private void RemoveSelfFromManager()
		{
			if (EnemySpriteManager.Instance.ContainsEnemySprite(this))
			{
				EnemySpriteManager.Instance.RemoveEnemySprite(this);
			}
			else
			{
				Debug.LogWarning($"Cannot remove {this} from {EnemySpriteManager.Instance} because it doesn't contain it", this);
			}
		}
	}
}