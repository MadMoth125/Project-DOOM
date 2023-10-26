using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunFireParameters_SO : ScriptableObject
{
	public delegate void FireProjectileDelegate();
	
	public float distance;
	public Vector3 origin;
	public Vector3 direction;
	public abstract void FireProjectile();
}

public class HitScanFireParameters_SO : GunFireParameters_SO
{
	public RaycastType raycastType;
	
	public override void FireProjectile()
	{
		switch (raycastType)
		{
			case RaycastType.Line:
				Debug.DrawLine(origin, origin + direction * distance, Color.red, 1f);
				break;
		}
	}

	public enum RaycastType
	{
		Line,
		Capsule
	}
}