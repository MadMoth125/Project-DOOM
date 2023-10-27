using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace ProjectDOOM.Weapons.V2
{
	public abstract class FireMethod
	{
		public Vector3 originOffset = Vector3.zero;
		public Vector3 direction = Vector3.forward;
	
		[HideInInspector]
		public Vector3 origin = Vector3.zero;
		
		public abstract void Fire();
	}
	
	[Serializable]
	public class HitScanFireMethod : FireMethod
	{
		[Space(10)]
		public float distance;

		public RaycastHit HitResults => _hitResults;
		private RaycastHit _hitResults;

		public override void Fire()
		{
			float tempDist = distance > 0 ? distance : Mathf.Infinity;
			
			Physics.SphereCast(origin + originOffset, 0.1f, direction, out _hitResults, tempDist);
		}
	}
	
	[Serializable]
	public class ProjectileFireMethod : FireMethod
	{
		[Space(10)]
		public GameObject projectilePrefab;
		public float projectileSpeed;
		public float projectileLifeTime;
	
		[HideInInspector]
		public List<GameObject> fireProjectiles = new List<GameObject>();
		
		public override void Fire()
		{
			if (projectilePrefab == null)
			{
				Debug.LogError("No projectile prefab assigned.");
				return;
			}
		
			GameObject projectile = UnityEngine.Object.Instantiate(projectilePrefab, origin, Quaternion.identity);
			projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
			UnityEngine.Object.Destroy(projectile, projectileLifeTime);
		}
	}
}