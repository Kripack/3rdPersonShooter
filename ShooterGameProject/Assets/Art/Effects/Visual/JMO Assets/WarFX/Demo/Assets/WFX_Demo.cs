using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.Serialization;

/**
 *	Demo Scene Script for WAR FX
 *	
 *	(c) 2015, Jean Moreno
**/

public class WfxDemo : MonoBehaviour
{
	public float cameraSpeed = 10f;
	
	public bool orderedSpawns = true;
	public float step = 1.0f;
	public float range = 5.0f;
	private float _order = -5.0f;
	
	public GameObject walls;
	public GameObject bulletholes;
	
	[FormerlySerializedAs("ParticleExamples")] public GameObject[] particleExamples;
	
	private int _exampleIndex;
	private string _randomSpawnsDelay = "0.5";
	private bool _randomSpawns;
	
	private bool _slowMo;
	private bool _rotateCam = true;
	
	public Material wood,concrete,metal,checker;
	public Material woodWall,concreteWall,metalWall,checkerWall;
	private string _groundTextureStr = "Checker";
	private List<string> _groundTextures = new List<string>(new string[]{"Concrete","Wood","Metal","Checker"});
		
	void OnMouseDown()
	{
		RaycastHit hit = new RaycastHit();
		if(this.GetComponent<Collider>().Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 9999f))
		{
			GameObject particle = SpawnParticle();
			if(!particle.name.StartsWith("WFX_MF"))
				particle.transform.position = hit.point + particle.transform.position;
		}
	}
	
	public GameObject SpawnParticle()
	{
		GameObject particles = (GameObject)Instantiate(particleExamples[_exampleIndex]);
		
		if(particles.name.StartsWith("WFX_MF"))
		{
			particles.transform.parent = particleExamples[_exampleIndex].transform.parent;
			particles.transform.localPosition = particleExamples[_exampleIndex].transform.localPosition;
			particles.transform.localRotation = particleExamples[_exampleIndex].transform.localRotation;
		}
		else if(particles.name.Contains("Hole"))
		{
			particles.transform.parent = bulletholes.transform;
		}
		
		SetActiveCrossVersions(particles, true);
		
		return particles;
	}
	
	void SetActiveCrossVersions(GameObject obj, bool active)
	{
		#if UNITY_3_5
				obj.SetActiveRecursively(active);
		#else
				obj.SetActive(active);
				for(int i = 0; i < obj.transform.childCount; i++)
					obj.transform.GetChild(i).gameObject.SetActive(active);
		#endif
	}
	
	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(5,20,Screen.width-10,60));
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Effect: " + particleExamples[_exampleIndex].name, GUILayout.Width(280));
		if(GUILayout.Button("<", GUILayout.Width(30)))
		{
			PrevParticle();
		}
		if(GUILayout.Button(">", GUILayout.Width(30)))
		{
			NextParticle();
		}
		
		GUILayout.FlexibleSpace();
		GUILayout.Label("Click on the ground to spawn the selected effect");
		GUILayout.FlexibleSpace();
		
		if(GUILayout.Button(_rotateCam ? "Pause Camera" : "Rotate Camera", GUILayout.Width(110)))
		{
			_rotateCam = !_rotateCam;
		}
		
		/*
		if(GUILayout.Button(randomSpawns ? "Stop Random Spawns" : "Start Random Spawns", GUILayout.Width(140)))
		{
			randomSpawns = !randomSpawns;
			if(randomSpawns)	StartCoroutine("RandomSpawnsCoroutine");
			else 				StopCoroutine("RandomSpawnsCoroutine");
		}
		
		randomSpawnsDelay = GUILayout.TextField(randomSpawnsDelay, 10, GUILayout.Width(42));
		randomSpawnsDelay = Regex.Replace(randomSpawnsDelay, @"[^0-9.]", "");
		*/
		
		if(GUILayout.Button(this.GetComponent<Renderer>().enabled ? "Hide Ground" : "Show Ground", GUILayout.Width(90)))
		{
			this.GetComponent<Renderer>().enabled = !this.GetComponent<Renderer>().enabled;
		}
		
		if(GUILayout.Button(_slowMo ? "Normal Speed" : "Slow Motion", GUILayout.Width(100)))
		{
			_slowMo = !_slowMo;
			if(_slowMo)	Time.timeScale = 0.33f;
			else  		Time.timeScale = 1.0f;
		}
		
		GUILayout.EndHorizontal();
		
		//--------------------
		
		GUILayout.BeginHorizontal();
		
		GUILayout.Label("Ground texture: " + _groundTextureStr, GUILayout.Width(160));
		if(GUILayout.Button("<", GUILayout.Width(30)))
		{
			PrevTexture();
		}
		if(GUILayout.Button(">", GUILayout.Width(30)))
		{
			NextTexture();
		}
		
		GUILayout.EndHorizontal();
		
		GUILayout.EndArea();
		
		//--------------------
		
		if(m4.GetComponent<Renderer>().enabled)
		{
			GUILayout.BeginArea(new Rect(5, Screen.height - 100, Screen.width - 10, 90));
			_rotateM4 = GUILayout.Toggle(_rotateM4, "AutoRotate Weapon", GUILayout.Width(250));
			GUI.enabled = !_rotateM4;
			float rx = m4.transform.localEulerAngles.x;
			rx = rx > 90 ? rx-180 : rx;
			float ry = m4.transform.localEulerAngles.y;
			float rz = m4.transform.localEulerAngles.z;
			rx = GUILayout.HorizontalSlider(rx, 0, 179, GUILayout.Width(256));
			ry = GUILayout.HorizontalSlider(ry, 0, 359, GUILayout.Width(256));
			rz = GUILayout.HorizontalSlider(rz, 0, 359, GUILayout.Width(256));
			if(GUI.changed)
			{
				if(rx > 90)
					rx += 180;
				
				m4.transform.localEulerAngles = new Vector3(rx,ry,rz);
				Debug.Log(rx);
			}
			GUILayout.EndArea();
		}
	}

	public GameObject m4;
	[FormerlySerializedAs("m4fps")] public GameObject m4FPS;
	private bool _rotateM4 = true;
	
	IEnumerator RandomSpawnsCoroutine()
	{
		
	LOOP:
		GameObject particles = SpawnParticle();
		
		if(orderedSpawns)
		{
			particles.transform.position = this.transform.position + new Vector3(_order,particles.transform.position.y,0);
			_order -= step;
			if(_order < -range) _order = range;
		}
		else 				particles.transform.position = this.transform.position + new Vector3(Random.Range(-range,range),0,Random.Range(-range,range)) + new Vector3(0,particles.transform.position.y,0);
		
		yield return new WaitForSeconds(float.Parse(_randomSpawnsDelay));
		
		goto LOOP;
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			PrevParticle();
		}
		else if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			NextParticle();
		}
		
		if(_rotateCam)
		{
			Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, cameraSpeed*Time.deltaTime);
		}
		if(_rotateM4)
		{
			m4.transform.Rotate(new Vector3(0,40f,0) * Time.deltaTime, Space.World);
		}
	}
	
	private void PrevTexture()
	{
		int index = _groundTextures.IndexOf(_groundTextureStr);
		index--;
		if(index < 0)
			index = _groundTextures.Count-1;
		
		_groundTextureStr = _groundTextures[index];
		
		SelectMaterial();
		
	}
	private void NextTexture()
	{
		int index = _groundTextures.IndexOf(_groundTextureStr);
		index++;
		if(index >= _groundTextures.Count)
			index = 0;
		
		_groundTextureStr = _groundTextures[index];
		
		SelectMaterial();
	}
	
	private void SelectMaterial()
	{
		switch(_groundTextureStr)
		{
		case "Concrete":
			this.GetComponent<Renderer>().material = concrete;
			walls.transform.GetChild(0).GetComponent<Renderer>().material = concreteWall;
			walls.transform.GetChild(1).GetComponent<Renderer>().material = concreteWall;
			break;
			
		case "Wood":
			this.GetComponent<Renderer>().material = wood;
			walls.transform.GetChild(0).GetComponent<Renderer>().material = woodWall;
			walls.transform.GetChild(1).GetComponent<Renderer>().material = woodWall;
			break;

		case "Metal":
			this.GetComponent<Renderer>().material = metal;
			walls.transform.GetChild(0).GetComponent<Renderer>().material = metalWall;
			walls.transform.GetChild(1).GetComponent<Renderer>().material = metalWall;
			break;
			
		case "Checker":
			this.GetComponent<Renderer>().material = checker;
			walls.transform.GetChild(0).GetComponent<Renderer>().material = checkerWall;
			walls.transform.GetChild(1).GetComponent<Renderer>().material = checkerWall;
			break;
		}
	}
	
	private void PrevParticle()
	{
		_exampleIndex--;
		if(_exampleIndex < 0) _exampleIndex = particleExamples.Length - 1;
		
		ShowHideStuff();
	}
	private void NextParticle()
	{
		_exampleIndex++;
		if(_exampleIndex >= particleExamples.Length) _exampleIndex = 0;
		
		ShowHideStuff();
	}
	
	private void ShowHideStuff()
	{
		//Show m4
		if(particleExamples[_exampleIndex].name.StartsWith("WFX_MF Spr"))
			m4.GetComponent<Renderer>().enabled = true;
		else
			m4.GetComponent<Renderer>().enabled = false;
		
		if(particleExamples[_exampleIndex].name.StartsWith("WFX_MF FPS"))
			m4FPS.GetComponent<Renderer>().enabled = true;
		else
			m4FPS.GetComponent<Renderer>().enabled = false;
		
		//Show walls
		if(particleExamples[_exampleIndex].name.StartsWith("WFX_BImpact"))
		{
			SetActiveCrossVersions(walls, true);
			
			Renderer[] rs = bulletholes.GetComponentsInChildren<Renderer>();
			foreach(Renderer r in rs)
				r.enabled = true;
		}
		else
		{
			SetActiveCrossVersions(walls, false);
			
			Renderer[] rs = bulletholes.GetComponentsInChildren<Renderer>();
			foreach(Renderer r in rs)
				r.enabled = false;
		}
		
		//Change ground texture
		if(particleExamples[_exampleIndex].name.Contains("Wood"))
		{
			_groundTextureStr = "Wood";
			SelectMaterial();
		}
		else if(particleExamples[_exampleIndex].name.Contains("Concrete"))
		{
			_groundTextureStr = "Concrete";
			SelectMaterial();
		}
		else if(particleExamples[_exampleIndex].name.Contains("Metal"))
		{
			_groundTextureStr = "Metal";
			SelectMaterial();
		}
		else if(particleExamples[_exampleIndex].name.Contains("Dirt")
			|| particleExamples[_exampleIndex].name.Contains("Sand")
			|| particleExamples[_exampleIndex].name.Contains("SoftBody"))
		{
			_groundTextureStr = "Checker";
			SelectMaterial();
		}
		else if(particleExamples[_exampleIndex].name == "WFX_Explosion")
		{
			_groundTextureStr = "Checker";
			SelectMaterial();
		}
	}
}
