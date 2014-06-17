using UnityEngine;
using System.Collections;
using PokeCore.Pokemon;

namespace PokemonGUI {
namespace Scenes {
namespace Battle {

public class BallZoneIn : MonoBehaviour {


	public int totalFrames = 60;
	private int currentFrame;
	private Sprite previousSprite;
	
	private Vector3 initialPosition;
	private Vector3 initialScale;
	
	float amount = 1.0f / 30.0f;
	
	public BattleDisplay.AnimationCallback callback;
	
	SpriteRenderer m_renderer;
	SpriteRenderer m_parentRenderer;
	Animator m_parentAnimator;
	
	Species m_species;
	public Species Species { set { m_species = value; } }
	
	void Awake()
	{
		m_renderer = gameObject.GetComponent<SpriteRenderer>();
		m_parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
		m_parentAnimator = transform.parent.GetComponent<Animator>();
	}	
	
	private void Init()
	{
		currentFrame = totalFrames;
		
		Bounds parentBounds = m_parentRenderer.bounds;
		
		float extentX = parentBounds.extents.x;
		float extentY = parentBounds.extents.y;
		float maxExtent = Mathf.Max(extentX, extentY);
		
		m_renderer.enabled = true;
		
		float circleExtent = m_renderer.sprite.bounds.extents.x;
		
		float totalScale = maxExtent / circleExtent;
		
		gameObject.transform.position = parentBounds.center;
		gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
		
		amount = totalScale / (float)totalFrames;
	}
	
	void OnEnable()
	{
		Init();
	}
	
	void Update ()
	{
		if (currentFrame > 0)
		{
			transform.localScale += new Vector3(amount, amount, 0.0f);
		}
		
		if (currentFrame == 0)
		{
			m_parentAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Controllers/" + m_species.ToString());
			m_parentRenderer.enabled = true;
			m_renderer.enabled = false;
			this.enabled = false;
			//gameObject.transform.localPosition = initialPosition;
			//gameObject.transform.localScale = initialScale;
			//m_renderer.sprite = previousSprite;
			//callback(this);
			//this.enabled = false;
		}
		else
		{
			--currentFrame;
		}
	}
}


}}}