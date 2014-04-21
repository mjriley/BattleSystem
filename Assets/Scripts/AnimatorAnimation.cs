using UnityEngine;

public class AnimatorAnimation : IAnimationEffect
{

	private Animator m_animator;
	private SpriteRenderer m_renderer;
	
	public AnimatorAnimation(Animator animator)
	{
		m_animator = animator;
		
		m_renderer = animator.transform.gameObject.GetComponent<SpriteRenderer>();
	}
	
	public bool Done { get { return !m_renderer.enabled; } }
	
	public void Update()
	{
		// do nothing -- the animator automates this
	}
	
	public void Start()
	{
		m_renderer.enabled = true;
		m_animator.enabled = true;
		m_animator.Play(0);
	}
}
