using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDOOM.deprecated.V2
{
	public class NewEnemy : MonoBehaviour
	{
		private SpriteRenderer _spriteRendererComponent;

		private void Awake()
		{
			_spriteRendererComponent = GetComponentInChildren<SpriteRenderer>();
		}

		public void ApplyRotationAndIndex(Quaternion rotation, int index)
		{
			// Implement how you update the rotation and index for an enemy.
			_spriteRendererComponent.transform.rotation = rotation;
			// You can use the index as needed.
		}
	}
}