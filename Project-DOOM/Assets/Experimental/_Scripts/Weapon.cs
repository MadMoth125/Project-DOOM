using System;
using System.Collections;
using System.Collections.Generic;
using Attempt2;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
	public event Action OnFireConditionMet;
	
	[SerializeField]
	protected GunParameters_SO gunParameters;

	protected bool[] fireConditions;

	protected virtual void Update()
	{
		gunParameters.Fire(fireConditions, Fire);
	}

	protected virtual void Fire()
	{
		OnFireConditionMet?.Invoke();
	}
}