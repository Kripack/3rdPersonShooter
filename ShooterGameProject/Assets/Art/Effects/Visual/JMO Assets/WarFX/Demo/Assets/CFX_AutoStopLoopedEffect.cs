using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

// Cartoon FX  - (c) 2015 Jean Moreno
//
// Script handling looped effect in the Demo Scene, so that they eventually stop

[RequireComponent(typeof(ParticleSystem))]
public class CfxAutoStopLoopedEffect : MonoBehaviour
{
	public float effectDuration = 2.5f;
	private float _d;
	
	void OnEnable()
	{
		_d = effectDuration;
	}
	
	void Update()
	{
		if(_d > 0)
		{
			_d -= Time.deltaTime;
			if(_d <= 0)
			{
				this.GetComponent<ParticleSystem>().Stop(true);
				
				CfxDemoTranslate translation = this.gameObject.GetComponent<CfxDemoTranslate>();
				if(translation != null)
				{
					translation.enabled = false;
				}
			}
		}
	}
}
