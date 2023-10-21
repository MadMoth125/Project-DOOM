using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunParameters_SO : ScriptableObject
{
	public event Action OnGunFired;
	
	[field: SerializeField]
	public TriggerType TriggerMethod { get; private set; } = TriggerType.Single;
	
	[field: SerializeField]
	public BulletType ProjectileMethod { get; private set; } = BulletType.Hitscan;
	
	public void Fire()
	{
		switch (TriggerMethod)
		{
			case TriggerType.Single:
				if (Input.GetMouseButtonDown(0)) OnGunFired?.Invoke();
				break;
			case TriggerType.Automatic:
				if (Input.GetMouseButton(0)) OnGunFired?.Invoke();
				break;
		}
	}
	
	public void HandleProjectile(Action hitscan, Action projectile)
	{
		switch (ProjectileMethod)
		{
			case BulletType.Hitscan:
				hitscan?.Invoke();
				break;
			case BulletType.Projectile:
				projectile?.Invoke();
				break;
		}
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