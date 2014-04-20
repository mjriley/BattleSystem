using UnityEngine;
using System.Collections;

public class BallZoneIn : MonoBehaviour {


	public int totalFrames = 60;
	private int currentFrame;
	private Sprite previousSprite;
	
	private Vector3 initialPosition;
	private Vector3 initialScale;
	
	float amount = 1.0f / 30.0f;
	
	public Sprite sprite;
	public BattleDisplay.AnimationCallback callback;
	
	SpriteRenderer m_renderer;
	
	// Use this for initialization
	void Start () {
		currentFrame = totalFrames;
		
		m_renderer = gameObject.GetComponent<SpriteRenderer>();
		
		float extentX = m_renderer.sprite.bounds.extents.x;
		float extentY = m_renderer.sprite.bounds.extents.y;
		float maxExtent = Mathf.Max(extentX, extentY);
		
		
		initialPosition = gameObject.transform.localPosition;
		gameObject.transform.localPosition = m_renderer.bounds.center;
		
		previousSprite = m_renderer.sprite;
		m_renderer.sprite = sprite;
		
		float circleExtent = sprite.bounds.extents.x;
		
		float totalScale = maxExtent / circleExtent;
		//gameObject.transform.localScale = new Vector3(totalScale, totalScale, 1.0f);
		
		initialScale = gameObject.transform.localScale;
		gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
		
		amount = totalScale / (float)totalFrames;
		
		//Debug.Log(m_renderer.bounds.center.x + ", " + m_renderer.bounds.center.y);
	}
	
	// Update is called once per frame
	void Update () {
		if (currentFrame > 0)
		{
			transform.localScale += new Vector3(amount, amount, 0.0f);
		}
		
		if (currentFrame == 0)
		{
			gameObject.transform.localPosition = initialPosition;
			gameObject.transform.localScale = initialScale;
			m_renderer.sprite = previousSprite;
			callback(this);
			this.enabled = false;
		}
		else
		{
			--currentFrame;
		}
	}
}
