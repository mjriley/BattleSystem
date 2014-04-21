using UnityEngine;

public class ScriptAnimation : IAnimationEffect
{
	MonoBehaviour m_script;
	
	public ScriptAnimation(MonoBehaviour script)
	{
		m_script = script;
	}
	
	public bool Done { get { return !m_script.enabled; } }
	
	public void Start()
	{
		m_script.enabled = true;
	}
	
	public void Update()
	{
		// do nothing -- the animation is contained within the script
	}
}
