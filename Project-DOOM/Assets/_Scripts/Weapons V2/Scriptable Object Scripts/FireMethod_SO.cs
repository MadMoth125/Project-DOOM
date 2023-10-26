using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectDOOM.Weapons.V2
{
	public abstract class FireMethod_SO : ScriptableObject
	{
		public Vector3 origin;
		public Vector3 direction;
	
		public abstract void Fire();
	}

	[CreateAssetMenu(fileName = "HitScanMethod", menuName = "Weapons/HitScan Fire Method", order = 1)]
	public class HitScanFireMethodSo : FireMethod_SO
	{
		[Space]
		public float distance;
	
		public override void Fire()
		{
			Debug.DrawLine(origin, origin + direction * distance, Color.red, 1f);
		}
	}

	[CreateAssetMenu(fileName = "ProjectileMethod", menuName = "Weapons/Projectile Fire Method", order = 1)]
	public class ProjectileFireMethodSo : FireMethod_SO
	{
		[Space]
		public GameObject projectilePrefab;
		public float projectileSpeed;
		public float projectileLifeTime;
	
		public override void Fire()
		{
			if (projectilePrefab == null)
			{
				Debug.LogError("No projectile prefab assigned.");
				return;
			}
		
			GameObject projectile = Instantiate(projectilePrefab, origin, Quaternion.identity);
			projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
			Destroy(projectile, projectileLifeTime);
		}
	}
}