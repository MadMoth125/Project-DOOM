using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Light))]
public class MuzzleFlash : MonoBehaviour
{
	private Light _lightSource;

	[SerializeField]
	private Color color = new Color(1f,0.66f,0.5f);
	[SerializeField]
	private float intensity = 100f ;
	[SerializeField]
	private float range = 20f;
	
	private void Awake()
	{
		_lightSource = GetComponent<Light>();
		
		if (_lightSource)
		{
			_lightSource.intensity = intensity;
			_lightSource.color = color;
			_lightSource.range = range;
			_lightSource.shadows = LightShadows.Hard;
			_lightSource.shadowResolution = LightShadowResolution.Low;
			_lightSource.enabled = false;
		}
		else
		{
			Debug.LogError($"{GetType()}: No Light Component found on {gameObject.name}");
		}
	}
	
	public void PlayLightAnimation()
	{
		StartCoroutine(FlashLight());
	}

	private IEnumerator FlashLight(float brightness = 100)
	{
		if (_lightSource)
		{
			_lightSource.enabled = true;
			_lightSource.intensity = brightness;

			// wait for 2 fixed updates (can change this to whatever delay you want)
			for (int i = 0; i < 2; i++) yield return new WaitForFixedUpdate();

			_lightSource.enabled = false;
		}
		else
		{
			Debug.LogError($"{GetType()}: No Light Component found on {gameObject.name}");
		}
	}
}