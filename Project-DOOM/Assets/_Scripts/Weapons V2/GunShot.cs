using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShot : MonoBehaviour
{
	[SerializeField]
	private AudioClip gunShot;
	
	public void PlaySoundEffect()
	{
		AudioManager.Instance.PlaySound(gunShot);
	}
}