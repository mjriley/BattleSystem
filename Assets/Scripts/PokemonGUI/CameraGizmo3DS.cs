using UnityEngine;
using System.Collections;

namespace PokemonGUI {

public class CameraGizmo3DS : MonoBehaviour
{
	void OnDrawGizmos()
	{
		Camera.main.aspect = 400.0f / 480.0f;
		
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(400, 240, 0));
	}
}

}