using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.Serialization;

// Cartoon FX - (c) 2015 - Jean Moreno
//
// Script handling the Demo scene of the Cartoon FX Packs

public class WfxDemoNew : MonoBehaviour
{
	public Renderer groundRenderer;
	public Collider groundCollider;
	[Space]
	[Space]
	public Image slowMoBtn;
	public Text slowMoLabel;
	public Image camRotBtn;
	public Text camRotLabel;
	public Image groundBtn;
	public Text groundLabel;
	[FormerlySerializedAs("EffectLabel")] [Space]
	public Text effectLabel;
	[FormerlySerializedAs("EffectIndexLabel")] public Text effectIndexLabel;

	//WFX
	[FormerlySerializedAs("AdditionalEffects")] public GameObject[] additionalEffects;
	public GameObject ground;
	public GameObject walls;
	public GameObject bulletholes;
	public GameObject m4;
	[FormerlySerializedAs("m4fps")] public GameObject m4FPS;
	public Material wood,concrete,metal,checker;
	public Material woodWall,concreteWall,metalWall,checkerWall;
	private string _groundTextureStr = "Checker";
	private List<string> _groundTextures = new List<string>(new string[]{"Concrete","Wood","Metal","Checker"});
	
	//-------------------------------------------------------------

	private GameObject[] _particleExamples;
	private int _exampleIndex;
	private bool _slowMo;
	private Vector3 _defaultCamPosition;
	private Quaternion _defaultCamRotation;
	
	private List<GameObject> _onScreenParticles = new List<GameObject>();
	
	//-------------------------------------------------------------
	
	void Awake()
	{
		List<GameObject> particleExampleList = new List<GameObject>();
		int nbChild = this.transform.childCount;
		for(int i = 0; i < nbChild; i++)
		{
			GameObject child = this.transform.GetChild(i).gameObject;
			particleExampleList.Add(child);
		}
		particleExampleList.AddRange(additionalEffects);
		_particleExamples = particleExampleList.ToArray();
		
		_defaultCamPosition = Camera.main.transform.position;
		_defaultCamRotation = Camera.main.transform.rotation;
		
		StartCoroutine("CheckForDeletedParticles");
		
		UpdateUI();
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
		else if(Input.GetKeyDown(KeyCode.Delete))
		{
			DestroyParticles();
		}
		
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hit = new RaycastHit();
			if(groundCollider.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 9999f))
			{
				GameObject particle = SpawnParticle();
				if(!particle.name.StartsWith("WFX_MF"))
				particle.transform.position = hit.point + particle.transform.position;
			}
		}
		
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if(scroll != 0f)
		{
			Camera.main.transform.Translate(Vector3.forward * (scroll < 0f ? -1f : 1f), Space.Self);
		}
		
		if(Input.GetMouseButtonDown(2))
		{
			Camera.main.transform.position = _defaultCamPosition;
			Camera.main.transform.rotation = _defaultCamRotation;
		}
	}

	//-------------------------------------------------------------
	// MESSAGES

	public void OnToggleGround()
	{
		var c = Color.white;
		groundRenderer.enabled = !groundRenderer.enabled;
		c.a = groundRenderer.enabled ? 1f : 0.33f;
		groundBtn.color = c;
		groundLabel.color = c;
	}

	public void OnToggleCamera()
	{
		var c = Color.white;
		CfxDemoRotateCamera.rotating = !CfxDemoRotateCamera.rotating;
		c.a = CfxDemoRotateCamera.rotating ? 1f : 0.33f;
		camRotBtn.color = c;
		camRotLabel.color = c;
	}
	
	public void OnToggleSlowMo()
	{
		var c = Color.white;

		_slowMo = !_slowMo;
		if(_slowMo)
		{
			Time.timeScale = 0.33f;
			c.a = 1f;
		}
		else
		{
			Time.timeScale = 1.0f;
			c.a = 0.33f;
		}

		slowMoBtn.color = c;
		slowMoLabel.color = c;
	}

	public void OnPreviousEffect()
	{
		PrevParticle();
	}

	public void OnNextEffect()
	{
		NextParticle();
	}
	
	//-------------------------------------------------------------
	// UI
	
	private void UpdateUI()
	{
		effectLabel.text = _particleExamples[_exampleIndex].name;
		effectIndexLabel.text = string.Format("{0}/{1}", (_exampleIndex+1).ToString("00"), _particleExamples.Length.ToString("00"));
	}
	
	//-------------------------------------------------------------
	// SYSTEM
	
	public GameObject SpawnParticle()
	{
		GameObject particles = (GameObject)Instantiate(_particleExamples[_exampleIndex]);
		particles.transform.position = new Vector3(0,particles.transform.position.y,0);
		#if UNITY_3_5
			particles.SetActiveRecursively(true);
		#else
			particles.SetActive(true);
//			for(int i = 0; i < particles.transform.childCount; i++)
//				particles.transform.GetChild(i).gameObject.SetActive(true);
		#endif
		
		if(particles.name.StartsWith("WFX_MF"))
		{
			particles.transform.parent = _particleExamples[_exampleIndex].transform.parent;
			particles.transform.localPosition = _particleExamples[_exampleIndex].transform.localPosition;
			particles.transform.localRotation = _particleExamples[_exampleIndex].transform.localRotation;
		}
		else if(particles.name.Contains("Hole"))
		{
			particles.transform.parent = bulletholes.transform;
		}

		ParticleSystem ps = particles.GetComponent<ParticleSystem>();
#if UNITY_5_5_OR_NEWER
		if (ps != null)
		{
			var main = ps.main;
			if (main.loop)
			{
				ps.gameObject.AddComponent<CfxAutoStopLoopedEffect>();
				ps.gameObject.AddComponent<CfxAutoDestructShuriken>();
			}
		}
#else
		if(ps != null && ps.loop)
		{
			ps.gameObject.AddComponent<CFX_AutoStopLoopedEffect>();
			ps.gameObject.AddComponent<CFX_AutoDestructShuriken>();
		}
#endif

		_onScreenParticles.Add(particles);
		
		return particles;
	}
	
	IEnumerator CheckForDeletedParticles()
	{
		while(true)
		{
			yield return new WaitForSeconds(5.0f);
			for(int i = _onScreenParticles.Count - 1; i >= 0; i--)
			{
				if(_onScreenParticles[i] == null)
				{
					_onScreenParticles.RemoveAt(i);
				}
			}
		}
	}
	
	private void PrevParticle()
	{
		_exampleIndex--;
		if(_exampleIndex < 0) _exampleIndex = _particleExamples.Length - 1;
		
		UpdateUI();
		ShowHideStuff();
	}
	private void NextParticle()
	{
		_exampleIndex++;
		if(_exampleIndex >= _particleExamples.Length) _exampleIndex = 0;
		
		UpdateUI();
		ShowHideStuff();
	}
	
	private void DestroyParticles()
	{
		for(int i = _onScreenParticles.Count - 1; i >= 0; i--)
		{
			if(_onScreenParticles[i] != null)
			{
				GameObject.Destroy(_onScreenParticles[i]);
			}
			
			_onScreenParticles.RemoveAt(i);
		}
	}
	
	// Change Textures
	
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
			ground.GetComponent<Renderer>().material = concrete;
			walls.transform.GetChild(0).GetComponent<Renderer>().material = concreteWall;
			walls.transform.GetChild(1).GetComponent<Renderer>().material = concreteWall;
			break;
			
		case "Wood":
			ground.GetComponent<Renderer>().material = wood;
			walls.transform.GetChild(0).GetComponent<Renderer>().material = woodWall;
			walls.transform.GetChild(1).GetComponent<Renderer>().material = woodWall;
			break;

		case "Metal":
			ground.GetComponent<Renderer>().material = metal;
			walls.transform.GetChild(0).GetComponent<Renderer>().material = metalWall;
			walls.transform.GetChild(1).GetComponent<Renderer>().material = metalWall;
			break;
			
		case "Checker":
			ground.GetComponent<Renderer>().material = checker;
			walls.transform.GetChild(0).GetComponent<Renderer>().material = checkerWall;
			walls.transform.GetChild(1).GetComponent<Renderer>().material = checkerWall;
			break;
		}
	}
	
	private void ShowHideStuff()
	{
		//Show m4
		if(_particleExamples[_exampleIndex].name.StartsWith("WFX_MF Spr"))
		{
			m4.GetComponent<Renderer>().enabled = true;
			Camera.main.transform.position = new Vector3(-2.482457f, 3.263842f, -0.004924395f);
			Camera.main.transform.eulerAngles = new Vector3(20f, 90f, 0f);
		}
		else
			m4.GetComponent<Renderer>().enabled = false;
		
		if(_particleExamples[_exampleIndex].name.StartsWith("WFX_MF FPS"))
			m4FPS.GetComponent<Renderer>().enabled = true;
		else
			m4FPS.GetComponent<Renderer>().enabled = false;
		
		//Show walls
		if(_particleExamples[_exampleIndex].name.StartsWith("WFX_BImpact"))
		{
			walls.SetActive(true);
			
			Renderer[] rs = bulletholes.GetComponentsInChildren<Renderer>();
			foreach(Renderer r in rs)
				r.enabled = true;
		}
		else
		{
			walls.SetActive(false);
			
			Renderer[] rs = bulletholes.GetComponentsInChildren<Renderer>();
			foreach(Renderer r in rs)
				r.enabled = false;
		}
		
		//Change ground texture
		if(_particleExamples[_exampleIndex].name.Contains("Wood"))
		{
			_groundTextureStr = "Wood";
			SelectMaterial();
		}
		else if(_particleExamples[_exampleIndex].name.Contains("Concrete"))
		{
			_groundTextureStr = "Concrete";
			SelectMaterial();
		}
		else if(_particleExamples[_exampleIndex].name.Contains("Metal"))
		{
			_groundTextureStr = "Metal";
			SelectMaterial();
		}
		else if(_particleExamples[_exampleIndex].name.Contains("Dirt")
			|| _particleExamples[_exampleIndex].name.Contains("Sand")
			|| _particleExamples[_exampleIndex].name.Contains("SoftBody"))
		{
			_groundTextureStr = "Checker";
			SelectMaterial();
		}
		else if(_particleExamples[_exampleIndex].name == "WFX_Explosion")
		{
			_groundTextureStr = "Checker";
			SelectMaterial();
		}
	}
}
