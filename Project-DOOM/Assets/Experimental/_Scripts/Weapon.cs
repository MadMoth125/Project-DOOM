using System;
using System.Collections;
using System.Collections.Generic;
using Attempt1;
using Attempt2;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
	[SerializeField]
	protected GunParameters_SO1 gunParameters;

	protected bool[] fireConditions;
	
	protected virtual void OnEnable()
	{
		gunParameters.OnGunFired += Fire;
	}

	protected virtual void OnDisable()
	{
		gunParameters.OnGunFired -= Fire;
	}

	protected virtual void Update()
	{
		gunParameters.Fire(fireConditions);
	}

	protected virtual void Fire()
	{
		
	}
}