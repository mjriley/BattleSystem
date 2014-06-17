using System;

namespace PokemonGUI {
	using Controls;
namespace Scenes {
namespace Battle {

public class GradualDamageAnimation : IAnimationEffect
{
	PlayerStatusDisplay m_display;
	int m_amount;
	int m_numFrames;
	int m_currentFrame;
	
	float m_perFrameAmount;
	int m_finalHealth;
	
	public GradualDamageAnimation(PlayerStatusDisplay display, int amount, int numFrames)
	{
		m_display = display;
		m_amount = amount;
		m_numFrames = numFrames;
	}
	
	public bool Done { get { return (m_currentFrame == m_numFrames); } }
	
	public void Start()
	{
		m_currentFrame = 0;
		
		// determine actual amount
		float currentHealth = m_display.CurrentHealth;
		
		int amount = Math.Min((int)currentHealth, m_amount);
		
		m_finalHealth = (int)m_display.CurrentHealth - amount;
		
		m_perFrameAmount = (float)amount / (float)m_numFrames;
	}
	
	public void Update()
	{
		if (m_currentFrame < m_numFrames)
		{
			m_display.CurrentHealth -= m_perFrameAmount;
			++m_currentFrame;
			
			if (m_currentFrame == m_numFrames)
			{
				// clean up the health bar so we don't leave a fractional amount or accumulate fractional issues
				m_display.CurrentHealth = m_finalHealth;
			}
		}
	}
}

}}}
