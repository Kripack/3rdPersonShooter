using UnityEngine;
using System.Collections;

/**
 *	Rapidly sets a light on/off.
 *	
 *	(c) 2015, Jean Moreno
**/

[RequireComponent(typeof(Light))]
public class WfxLightFlicker : MonoBehaviour
{
	public float time = 0.05f;
	
	private float _timer;
	
	void Start ()
	{
		_timer = time;
		StartCoroutine("Flicker");
	}
	
	IEnumerator Flicker()
	{
		while(true)
		{
			GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
			
			do
			{
				_timer -= Time.deltaTime;
				yield return null;
			}
			while(_timer > 0);
			_timer = time;
		}
	}
}
