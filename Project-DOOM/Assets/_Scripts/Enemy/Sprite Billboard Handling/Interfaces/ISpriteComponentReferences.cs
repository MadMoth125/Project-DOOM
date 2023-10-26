using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemySpriteHandling
{
	public interface ISpriteComponentReferences
	{
		public void UpdateAnimatorIndex(int index);
	
		public void UpdateSpriteDirection(float yRotation);
	
		public Transform GetSpriteTransform();
	
		public Transform GetTransform();
	}	
}