using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DotTest : MonoBehaviour
{
	public Transform targetA;
	public Transform targetB;
	
	private void Update()
	{
		if (!targetA || !targetB) return;
		
		Debug.Log(Vector3.Dot(targetA.forward, targetB.forward));
	}
}