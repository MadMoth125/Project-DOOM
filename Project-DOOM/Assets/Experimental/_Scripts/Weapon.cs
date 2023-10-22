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
	protected IWeaponEventHandler eventHandler;

	protected bool[] fireConditions;

	protected void Awake()
	{
		eventHandler ??= GetComponent<IWeaponEventHandler>();
		eventHandler ??= GetComponentInChildren<IWeaponEventHandler>();
		
		eventHandler.SetWeaponComponent(this);
	}

	protected virtual void Update()
	{
		gunParameters.Fire(fireConditions, Fire);
	}

	protected virtual void Fire()
	{
		OnFireConditionMet?.Invoke();
	}
}