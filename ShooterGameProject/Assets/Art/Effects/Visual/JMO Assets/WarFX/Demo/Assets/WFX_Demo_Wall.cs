using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/**
 *	Demo Scene Script for WAR FX
 *	
 *	(c) 2015, Jean Moreno
**/

public class WfxDemoWall : MonoBehaviour
{
	public WfxDemoNew demo;
	
	void OnMouseDown()
	{
		RaycastHit hit = new RaycastHit();
		if(this.GetComponent<Collider>().Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 9999f))
		{
			GameObject particle = demo.SpawnParticle();
			particle.transform.position = hit.point;
			particle.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
		}
	}
}
