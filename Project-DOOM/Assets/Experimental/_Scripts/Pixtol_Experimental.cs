using System;
using System.Collections;
using System.Collections.Generic;
using Attempt1;
using Attempt2;
using UnityEngine;

public class Pixtol_Experimental : MonoBehaviour
{
	[SerializeField]
	private GunParameters_SO gunParameters;
	
	private void OnEnable()
	{
		gunParameters.OnGunFired += OnGunFired;
	}

	private void OnDisable()
	{
		gunParameters.OnGunFired -= OnGunFired;
	}

	private void Start()
	{
		
	}

	private void Update()
	{
		gunParameters.Fire();
	}
	
	private void OnGunFired()
	{
		gunParameters.HandleProjectile(
			() =>
			{
				Debug.Log("Hitscan fired!");
			},
			() =>
			{
				Debug.Log("Projectile fired!");
			}
			);
	}
}