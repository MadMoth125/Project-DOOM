using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
	public int targetFrameRate = 60;

	private void OnValidate()
	{
		Application.targetFrameRate = targetFrameRate;
	}
}