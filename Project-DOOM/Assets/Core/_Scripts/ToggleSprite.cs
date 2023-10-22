using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSprite : MonoBehaviour
{
	public List<SpriteRenderer> sprites = new List<SpriteRenderer>();
	
	public void Toggle(bool state)
	{
		if (state)
		{
			ShowSprite();
		}
		else
		{
			HideSprite();
		}
	}

	public void ShowSprite()
	{
		foreach (SpriteRenderer sprite in sprites)
		{
			sprite.enabled = true;
		}
	}
	
	public void HideSprite()
	{
		foreach (SpriteRenderer sprite in sprites)
		{
			sprite.enabled = false;
		}
	}
}