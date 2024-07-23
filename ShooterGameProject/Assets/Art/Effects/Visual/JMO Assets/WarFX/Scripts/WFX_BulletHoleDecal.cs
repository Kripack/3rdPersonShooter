using UnityEngine;
using System.Collections;

/**
 *	Handles the bullet hole decals:
 *	- Sets a random frame
 *	- Fades the material according to the defined lifetime
 *	- Optionally rotates the decal
 *	
 *	(c) 2015, Jean Moreno
**/

[RequireComponent(typeof(MeshFilter))]
public class WfxBulletHoleDecal : MonoBehaviour
{
	static private Vector2[] _quadUVs = new Vector2[]{new Vector2(0,0), new Vector2(0,1), new Vector2(1,0), new Vector2(1,1)};
	
	public float lifetime = 10f;
	public float fadeoutpercent = 80;
	public Vector2 frames;
	public bool randomRotation = false;
	public bool deactivate = false;
	
	private float _life;
	private float _fadeout;
	private Color _color;
	private float _orgAlpha;
	
	void Awake()
	{
		_color = this.GetComponent<Renderer>().material.GetColor("_TintColor");
		_orgAlpha = _color.a;
	}
	
	void OnEnable()
	{
		//Random UVs
		int random = Random.Range(0, (int)(frames.x*frames.y));
		int fx = (int)(random%frames.x);
		int fy = (int)(random/frames.y);
		//Set new UVs
		Vector2[] meshUvs = new Vector2[4];
		for(int i = 0; i < 4; i++)
		{
			meshUvs[i].x = (_quadUVs[i].x + fx) * (1.0f/frames.x);
			meshUvs[i].y = (_quadUVs[i].y + fy) * (1.0f/frames.y);
		}
		this.GetComponent<MeshFilter>().mesh.uv = meshUvs;
		
		//Random rotate
		if(randomRotation)
			this.transform.Rotate(0f,0f,Random.Range(0f,360f), Space.Self);
		
		//Start lifetime coroutine
		_life = lifetime;
		_fadeout = _life * (fadeoutpercent/100f);
		_color.a = _orgAlpha;
		this.GetComponent<Renderer>().material.SetColor("_TintColor", _color);
		StopAllCoroutines();
		StartCoroutine("HoleUpdate");
	}
	
	IEnumerator HoleUpdate()
	{
		while(_life > 0f)
		{
			_life -= Time.deltaTime;
			if(_life <= _fadeout)
			{
				_color.a = Mathf.Lerp(0f, _orgAlpha, _life/_fadeout);
				this.GetComponent<Renderer>().material.SetColor("_TintColor", _color);
			}
			
			yield return null;
		}
	}
}
