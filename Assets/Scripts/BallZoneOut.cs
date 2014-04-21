using UnityEngine;

public class BallZoneOut : MonoBehaviour
{ 
	private SpriteRenderer m_renderer;
	
	public int numFrames = 60;
	
	private int m_currentFrame = 0;
	private float m_decrement = 0.0f;
	
	void Awake()
	{
		m_renderer = gameObject.GetComponent<SpriteRenderer>();
	}
	
	private void Init()
	{
		SpriteRenderer parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
		Vector3 parentExtents = parentRenderer.sprite.bounds.extents;
		
		float maxExtent = Mathf.Max(parentExtents.x, parentExtents.y);
		
		float circleExtent = m_renderer.sprite.bounds.extents.x;
		
		float ratio = maxExtent / circleExtent;
		
		m_decrement = ratio / numFrames;
		
		// set the scale to that ratio
		gameObject.transform.localScale = new Vector3(ratio, ratio, 1.0f);
		gameObject.transform.localPosition = parentRenderer.sprite.bounds.center;
		
		m_currentFrame = 0;
		
		parentRenderer.enabled = false;
		m_renderer.enabled = true;
	}
	
	void OnEnable()
	{
		Init();
	}
	
	void Update ()
	{
		if (m_currentFrame < numFrames)
		{
			gameObject.transform.localScale -= new Vector3(m_decrement, m_decrement, 0.0f);
			++m_currentFrame;
		}
		else
		{
			m_renderer.enabled = false;
			this.enabled = false;
		}
	}
}
