using UnityEngine;
using System.Collections;

public class BallRotation : MonoBehaviour {

	public int distance = 50;
	public float time = 5.0f;
	public float rotSpeed = 3.0f;
	
	float dist_per_sec = 0.0f;
	float total_distance = 0.0f;
	
	
	bool animating = false;
	
	Vector3 m_initialPosition;
	Quaternion m_initialRotation;
	
	WhiteOut m_whiteOut;

	// Use this for initialization
	void Start () {
		m_whiteOut = GetComponent<WhiteOut>();
		m_initialPosition = transform.position;
		m_initialRotation = transform.localRotation;
	}
	
	void Reset()
	{
		animating = false;
		transform.position = m_initialPosition;
		transform.rotation = m_initialRotation;
		
		total_distance = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Reset();
			animating = true;
			dist_per_sec = distance / time;
		}
		
		if (animating)
		{
			if (total_distance < distance)
			{
				total_distance += dist_per_sec * Time.deltaTime;
				transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - dist_per_sec * Time.deltaTime);
				//transform.Rotate(0.0f, 0.0f, -1.0f); 
				//transform.localRotation = new Quaternion(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z + 0.1f, transform.localRotation.w);
				transform.Rotate(new Vector3(rotSpeed * Time.deltaTime, 0.0f, 0.0f));
				
				float percent_travelled = total_distance / distance;
				if (percent_travelled > 0.75f)
				{
					float opacity = (percent_travelled - 0.75f) * 4;
					m_whiteOut.Opacity = opacity;
				}
			}
			else
			{
				animating = false;
			}
		}
	
	}
}
