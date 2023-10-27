using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance { get; private set; }

	[SerializeField]
	private AudioSource musicSource;
	[SerializeField]
	private AudioSource sfxSource;
	
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
			
			return;
		}
		
		Destroy(gameObject);
	}

	public void PlaySound(AudioClip clip)
	{
		sfxSource.PlayOneShot(clip);
	}
	
	public void PlaySoundAtPoint(AudioClip clip, Vector3 position)
	{
		AudioSource.PlayClipAtPoint(clip, position);
	}
	
	public void SetMusicVolume(float volume)
	{
		musicSource.volume = Mathf.Clamp01(volume);
	}
	
	public void SetSfxVolume(float volume)
	{
		sfxSource.volume = Mathf.Clamp01(volume);
	}
}