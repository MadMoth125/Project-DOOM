using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolTest : Weapon
{
	private void Start()
	{
		fireConditions = new bool[] { false };
	}

	protected override void Fire()
	{
		Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100, Color.red, 1f);
	}
}