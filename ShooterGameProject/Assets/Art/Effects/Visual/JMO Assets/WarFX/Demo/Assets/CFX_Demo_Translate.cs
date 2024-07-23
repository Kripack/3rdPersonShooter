using UnityEngine;
using System.Collections;

// Cartoon FX  - (c) 2015, Jean Moreno

public class CfxDemoTranslate : MonoBehaviour
{
	public float speed = 30.0f;
	public Vector3 rotation = Vector3.forward;
	public Vector3 axis = Vector3.forward;
	public bool gravity;
	private Vector3 _dir;
	
	void Start ()
	{
		_dir = new Vector3(Random.Range(0.0f,360.0f),Random.Range(0.0f,360.0f),Random.Range(0.0f,360.0f));
		_dir.Scale(rotation);
		this.transform.localEulerAngles = _dir;
	}
	
	void Update ()
	{
		this.transform.Translate(axis * speed * Time.deltaTime, Space.Self);
	}
}
