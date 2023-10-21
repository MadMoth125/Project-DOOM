using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolTest : Weapon
{
	public WeaponAnimationEventHandler eventHandler;
	
	private void Start()
	{
		fireConditions = new bool[1] { false };
	}

	protected override void Update()
	{
		fireConditions[0] = eventHandler.CanFire;
		base.Update();
	}

	protected override void Fire()
	{
		base.Fire();
		Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 50, Color.red, 1f);
	}
}