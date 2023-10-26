using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[RequireComponent(typeof(Light))]
public class MuzzleFlash : MonoBehaviour
{
	private Light _muzzleFlash;

	private void Awake()
	{
		_muzzleFlash = gameObject.SearchForComponent<Light>();
		_muzzleFlash.enabled = false;
	}
	
	public void Play()
	{
		StartCoroutine(FlashLight());
	}

	private IEnumerator FlashLight(float brightness = 50)
	{
		_muzzleFlash.enabled = true;
		_muzzleFlash.intensity = brightness;

		yield return new WaitForFixedUpdate();
		yield return new WaitForFixedUpdate();
		
		_muzzleFlash.enabled = false;

	}
}