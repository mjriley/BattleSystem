using UnityEngine;
using System.Collections;

namespace PokemonGUI {

public class WindAnimation : MonoBehaviour {
	
	Mesh mesh;
	
	public float x_offset = 0.05f;

	void Start()
	{
		mesh = GetComponent<MeshFilter>().mesh;
	}
	
	void Update()
	{
		Vector2[] uvs = mesh.uv;
		for (int i=0; i<uvs.Length; ++i)
		{
			uvs[i].x += x_offset * Time.deltaTime;
		}
		
		mesh.uv = uvs;
	}
}

}