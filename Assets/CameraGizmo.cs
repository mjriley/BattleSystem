using UnityEngine;
using System.Collections;

public class CameraGizmo : MonoBehaviour
{
	void OnDrawGizmos()
	{
		Camera.main.aspect = 16.0f / 9.0f;
		float verticalHeightSeen = Camera.main.orthographicSize * 2.0f;
		
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(transform.position, new Vector3((verticalHeightSeen * Camera.main.aspect), verticalHeightSeen, 0));
		//Gizmos.DrawWireCube(transform.position, new Vector3((verticalHeightSeen * 16.0f / 9.0f), verticalHeightSeen, 0));
	}
}
