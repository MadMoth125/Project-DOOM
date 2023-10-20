using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorSineLerp<T> where T : struct
{
	public T A { get; private set; }
	public T B { get; private set; }

	public float Frequency
	{
		get => _frequency;
		set => _frequency = Mathf.Max(value, 0f);
	}
	
	private float _frequency = 1f;
	
	public VectorSineLerp(T a, T b)
	{
		A = a;
		B = b;
	}
	
	public VectorSineLerp(T a, T b, float frequency)
	{
		A = a;
		B = b;
		Frequency = frequency;
	}
	
	public T SineLerp()
	{
		// Ensure T is a Vector type
		if (!(A is Vector2 || A is Vector3 || A is Vector4))
		{
			Debug.LogError("VectorSineLerp can only be used with Vector types (Vector2, Vector3, Vector4).");
			return default(T);
		}

		// Calculate the interpolation value using a sine wave
		float t = (Mathf.Sin(Time.time * Frequency) + 1f) / 2f;

		// Perform the sine lerp between A and B
		object result;

		if (A is Vector2)
			result = Vector2.Lerp((Vector2)(object)A, (Vector2)(object)B, t);
		else if (A is Vector3)
			result = Vector3.Lerp((Vector3)(object)A, (Vector3)(object)B, t);
		else if (A is Vector4)
			result = Vector4.Lerp((Vector4)(object)A, (Vector4)(object)B, t);
		else
			result = null;

		return (T)result;
	}
	
}