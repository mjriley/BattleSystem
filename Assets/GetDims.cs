using UnityEngine;
using System.Collections;

public class GetDims : MonoBehaviour {

	//public float width = 100.0f;
	public int height = 100;
	
	SpriteRenderer m_renderer;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("Clicked Mouse @ " + Input.mousePosition);
		}
	
	}
}
