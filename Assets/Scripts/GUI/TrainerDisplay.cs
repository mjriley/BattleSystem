using UnityEngine;

public class TrainerDisplay : MonoBehaviour
{
	public Texture2D trainerTexture;
	
	public float m_x;
	
	Rect m_topScreen = new Rect(0, 0, 400, 240);
	
	public int m_distanceLeft = 40;
	public int m_distanceRight = 400;
	
	public int m_timeLeft = 1000;
	public int m_timeRight = 1000;
	
	int m_currentTime = 0;
	float m_currentDistance = 0;
	
	bool m_animate = false;
	
	float m_updateDistancePerSecondLeft;
	float m_updateDistancePerSecondRight;
	
	public void UpdateTrainer(Trainer trainer)
	{
		trainerTexture = Resources.Load<Texture2D>(trainer.TexturePath);
	}
	
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			Reset();
			return;
		}
		
		if (!m_animate)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				StartEffect();
			}
		}
		else
		{
			UpdateEffect();
		}
	}
	
	void StartEffect()
	{
		m_animate = true;
		m_updateDistancePerSecondLeft = (float)m_distanceLeft / (float)m_timeLeft * 1000;
		m_updateDistancePerSecondRight = (float)m_distanceRight / (float)m_timeRight * 1000;
	}
	
	void UpdateEffect()
	{
		if (m_currentTime < m_timeLeft)
		{
			float updateAmount = m_updateDistancePerSecondLeft * Time.deltaTime;
			
			if (m_currentDistance < m_distanceLeft)
			{
				if (m_currentDistance + updateAmount < m_distanceLeft)
				{
					m_currentDistance += updateAmount;
				}
				else
				{
					updateAmount = m_distanceLeft - m_currentDistance;
					
					m_currentDistance = 0;
				}
				
				m_x -= updateAmount;
			}
			
			m_currentTime += (int)(Time.deltaTime * 1000);
		}
		else if (m_currentTime < m_timeLeft + m_timeRight)
		{
			float updateAmount = m_updateDistancePerSecondRight * Time.deltaTime;
			
			if (m_currentDistance < m_distanceRight)
			{
				if (m_currentDistance + updateAmount < m_distanceRight)
				{
					m_currentDistance += updateAmount;
				}
				else
				{
					updateAmount = m_distanceRight - m_currentDistance;
				}
				
				m_x += updateAmount;
			}
			
			m_currentTime += (int)(Time.deltaTime * 1000);
		}
		else
		{
			m_animate = false;
			return;
		}
	}
	
	void Reset()
	{
		m_currentTime = 0;
		m_currentDistance = 0;
		m_x = 0;
		m_animate = false; 
	}
	
	public void OnGUI()
	{
		GUI.depth = 10;
		DrawTopScreen();
	}
	
	void DrawTopScreen()
	{
		if (trainerTexture == null)
		{
			return;
		}
		
		GUIUtils.DrawGroup(m_topScreen, delegate(Rect bounds)
		{
			GUI.DrawTexture(new Rect(m_x, 0.0f, bounds.width, bounds.height), trainerTexture);
		});
	}
}
