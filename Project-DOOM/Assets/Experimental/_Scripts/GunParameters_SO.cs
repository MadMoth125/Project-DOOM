using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu]
public class GunParameters_SO : ScriptableObject
{
	public event Action OnGunFired;
	
	[field: SerializeField]
	public TriggerType TriggerMethod { get; private set; } = TriggerType.Single;
	
	[field: SerializeField]
	public BulletType ProjectileMethod { get; private set; } = BulletType.Hitscan;
	
	public void Fire(KeyCode key = KeyCode.Mouse0)
	{
		switch (TriggerMethod)
		{
			default:
			case TriggerType.Single:
				if (Input.GetKeyDown(key)) OnGunFired?.Invoke();
				break;
			case TriggerType.Automatic:
				if (Input.GetKey(key)) OnGunFired?.Invoke();
				break;
		}
	}
	
	public void Fire(bool[] conditions, KeyCode key = KeyCode.Mouse0)
	{
		switch (TriggerMethod)
		{
			default:
			case TriggerType.Single:
				if (Input.GetKeyDown(key) && CheckConditionals(conditions)) OnGunFired?.Invoke();
				break;
			case TriggerType.Automatic:
				if (Input.GetKey(key) && CheckConditionals(conditions)) OnGunFired?.Invoke();
				break;
		}
	}
	
	public void HandleProjectile(Action hitscan, Action projectile)
	{
		switch (ProjectileMethod)
		{
			default:
			case BulletType.Hitscan:
				hitscan?.Invoke();
				break;
			case BulletType.Projectile:
				projectile?.Invoke();
				break;
		}
	}
	
	private bool CheckConditionals(bool[] conditions)
	{
		bool result = true;
		foreach (bool condition in conditions)
		{
			result &= condition;
		}

		return result;
	}
}

// e.g., pistol vs rifle
public enum TriggerType
{
	Single,
	Automatic
}

// e.g., bullet vs rocket
public enum BulletType
{
	Hitscan,
	Projectile
}