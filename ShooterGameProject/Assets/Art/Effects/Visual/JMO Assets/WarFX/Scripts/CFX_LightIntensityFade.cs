using UnityEngine;
using System.Collections;

// Cartoon FX  - (c) 2015, Jean Moreno

// Decreases a light's intensity over time.

[RequireComponent(typeof(Light))]
public class CfxLightIntensityFade : MonoBehaviour
{
	// Duration of the effect.
	public float duration = 1.0f;
	
	// Delay of the effect.
	public float delay = 0.0f;
	
	/// Final intensity of the light.
	public float finalIntensity = 0.0f;
	
	// Base intensity, automatically taken from light parameters.
	private float _baseIntensity;
	
	// If <c>true</c>, light will destructs itself on completion of the effect
	public bool autodestruct;
	
	private float _pLifetime = 0.0f;
	private float _pDelay;
	
	void Start()
	{
		_baseIntensity = GetComponent<Light>().intensity;
	}
	
	void OnEnable()
	{
		_pLifetime = 0.0f;
		_pDelay = delay;
		if(delay > 0) GetComponent<Light>().enabled = false;
	}
	
	void Update ()
	{
		if(_pDelay > 0)
		{
			_pDelay -= Time.deltaTime;
			if(_pDelay <= 0)
			{
				GetComponent<Light>().enabled = true;
			}
			return;
		}
		
		if(_pLifetime/duration < 1.0f)
		{
			GetComponent<Light>().intensity = Mathf.Lerp(_baseIntensity, finalIntensity, _pLifetime/duration);
			_pLifetime += Time.deltaTime;
		}
		else
		{
			if(autodestruct)
				GameObject.Destroy(this.gameObject);
		}
		
	}
}
