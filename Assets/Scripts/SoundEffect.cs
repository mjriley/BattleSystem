using UnityEngine;

public class SoundEffect : IAnimationEffect
{
	AudioSource m_source;
	AudioClip m_clip;
	
	public SoundEffect(AudioSource source, AudioClip clip)
	{
		m_source = source;
		m_clip = clip;
	}
	
	public void Start()
	{
		m_source.clip = m_clip;
		m_source.Play();
	}
	
	public void Update()
	{
		// do nothing
	}
	
	public bool Done { get { return !m_source.isPlaying; } }
}
